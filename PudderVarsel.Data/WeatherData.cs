using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace PudderVarsel.Data
{
    public class WeatherData
    {
        public string locationsString { get; set; }
        const string Format = "yyyy-MM-ddTHH:mm:ssZ";

        public IEnumerable<Lokasjon> GetAllLocations(string locations)
        {
            return GetLocationsFromXml(0, 0, locations);
        }

        public IEnumerable<Lokasjon> GetLocationForecast(double currentLat, double currentLon, string locations, int maxDistance, string searchText)
        {
            var lokasjoner = GetLocationsFromXml(currentLat, currentLon, locations);
            if (!string.IsNullOrEmpty(searchText))
            {
                var searchLocations = lokasjoner.Where(l => l.Name.Contains(searchText));

                return searchLocations;
            }
            
            var locationsInArea = lokasjoner.Where(l => l.Distance < maxDistance);

            return locationsInArea;
        }


        private Lokasjon[] GetLocationsFromXml(double currentLat, double currentLon, string locationsXml)
        {
            var reader = XmlReader.Create(new StringReader(locationsXml));

            var ciNo = new CultureInfo("nb-NO");
            var element = XElement.Load(reader, LoadOptions.SetBaseUri);

            var items = element.DescendantsAndSelf("Location");

            var xElements = items as XElement[] ?? items.ToArray();

            var locations = new Lokasjon[xElements.Count()+1];
            var i = 0;
            foreach (var xElement in xElements)
            {
                var name = XmlHelper.GetAttributeValue("name", xElement);

                var location = new Lokasjon();
                location.Name = name;
                var area = int.Parse(XmlHelper.GetAttributeValue("area", xElement));
                location.Area = (Lokasjon.AreaEnum)area;

                location.Longitude = double.Parse(XmlHelper.GetAttributeValue("longitude", xElement).Replace('.', ','), ciNo);
                location.Latitude = double.Parse(XmlHelper.GetAttributeValue("latitude", xElement).Replace('.', ','), ciNo);
                location.Distance = GetDistance(currentLat, currentLon, location.Latitude, location.Longitude);
                locations[i] = location;
                i++;
            }

            var currentLocation = new Lokasjon();
            currentLocation.Name = "Din lokasjon";
            currentLocation.Latitude = currentLat;
            currentLocation.Longitude = currentLon;
            currentLocation.Distance = 0;
            locations[locations.Length-1] = currentLocation;
            return locations;
        }

        public bool SaveForecastToFile(XElement forecastResponse, string path, string dir)
        {
            var xdoc = new XDocument();
            xdoc.Add(forecastResponse);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //if (!File.Exists(path))
            //{
            //    File.Create(path);
            //}

            xdoc.Save(path);
            return true;
        }


        public XElement GetForecastFromFile(string path)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var forecast = XElement.Parse(xmlDoc.InnerXml);
            return forecast;

        }


        public DagligPuddervarsel[] ProcessResponse(XElement forecastResponse)
        {
            var timeInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var ciNo = new CultureInfo("nb-NO");
            var items = forecastResponse.DescendantsAndSelf("time");
            var t = 0;
            var xElements = items as XElement[] ?? items.ToArray();
            var powderForecastDays = new DagligPuddervarsel[12];
            var detailedPowderList = new DetailedPowder[4];
            var detailedTeller = 0;
            var i = 0;
            var lastDay = DateTime.Now.Date;
            var temperatureList = new Dictionary<string, string>();
            foreach (var xElement in xElements)
            {
                var fromDateTime = TimeZoneInfo.ConvertTime(DateTime.ParseExact(XmlHelper.GetAttributeValue("from", xElement), Format, ciNo), TimeZoneInfo.Local, timeInfo).ToUniversalTime();
                var toDateTime = TimeZoneInfo.ConvertTime(DateTime.ParseExact(XmlHelper.GetAttributeValue("to", xElement), Format, ciNo), TimeZoneInfo.Local, timeInfo).ToUniversalTime();

                if (IsRelevant(fromDateTime, toDateTime))
                {
                    //Ny DagligPuddervarsel
                    if (fromDateTime.Date > lastDay)
                    {
                        powderForecastDays[i] = DagligPuddervarsel.Create(detailedPowderList.Where(p => p != null)); ;
                        detailedPowderList = new DetailedPowder[4];
                        i++;
                        detailedTeller = 0;
                        lastDay = lastDay.AddDays(1);
                    }
                    
                    var detailedPowder = new DetailedPowder();
                    var precipitation = XmlHelper.GetElementValue("location", "precipitation", "value", xElement);
                    detailedPowder.Precipitation = Convert.ToDecimal(precipitation.Replace('.', ','), ciNo);

                    if (!System.Diagnostics.Debugger.IsAttached)
                    {
                        detailedPowder.From = fromDateTime.AddHours(-2);
                        detailedPowder.To = toDateTime.AddHours(-2);
                    }
                    else
                    {
                        detailedPowder.From = fromDateTime;
                        detailedPowder.To = toDateTime;
                    }
                    detailedPowder.Temperature = GetAverageTemp(temperatureList, ciNo);

                    detailedPowder.Powder = detailedPowder.Temperature < 2 ? detailedPowder.Precipitation : 0;

                    temperatureList = new Dictionary<string, string>();
                    if (detailedTeller < detailedPowderList.Count())
                        detailedPowderList[detailedTeller] = detailedPowder;
                    else
                    {
                        //Todo: Check data
                        t = 1;
                    }

                    detailedTeller++;
                }

                var temperature = XmlHelper.GetElementValue("location", "temperature", "value", xElement);
                if (!string.IsNullOrEmpty(temperature))
                {
                    temperatureList.Add(fromDateTime.ToString(), temperature);
                }
                
            }
            return powderForecastDays;
        }

        private static decimal GetAverageTemp(Dictionary<string,string> temperatureList, CultureInfo ciNo)
        {
            if (temperatureList.Count == 0)
                return 0;
            var temp = temperatureList.Sum(temperature => Convert.ToDecimal(temperature.Value.Replace('.', ','), ciNo));

            var average = temp/temperatureList.Count;
            return Math.Round(average, 1);
        }

        private static bool IsRelevant(DateTime from, DateTime to)
        {
            //if (from.Hour != to.Hour)
            //    return true;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                if (from.Hour == 18 && to.Hour == 0)
                    return true;
                if (from.Hour == 0 && to.Hour == 6)
                    return true;
                if (from.Hour == 6 && to.Hour == 12)
                    return true;
                if (from.Hour == 12 && to.Hour == 18)
                    return true;
            }
            else
            {
                //if (from.Hour == 14 && to.Hour == 20)
                //    return true;
                //if (from.Hour == 2 && to.Hour == 8)
                //    return true;
                //if (from.Hour == 8 && to.Hour == 14)
                //    return true;
                //if (from.Hour == 20 && to.Hour == 2)
                //    return true;

                if (to.Hour - from.Hour == 6)
                    return true;
                if (to.Hour - from.Hour == -18)
                    return true;
                //if (from.Hour == 5 && to.Hour == 11)
                //    return true;
                //if (from.Hour == 11 && to.Hour == 17)
                //    return true;
                //if (from.Hour == 17 && to.Hour == 23)
                //    return true;
            }
            return false;
        }

        private double GetDistance(double currentLat, double currentLong, double locationLat, double locationLong)
        {
            var R = 6371; // km
            var dLat = DegToRad(locationLat - currentLat);
            var dLon = DegToRad(locationLong - currentLong);
            var lat1 = DegToRad(currentLat);
            var lat2 = DegToRad(locationLat);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }

        public static double DegToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

    }

}
