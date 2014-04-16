using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace PudderVarsel.Data
{
    public class WeatherData
    {
        public string locationsString { get; set; }
        const string Format = "yyyy-MM-ddTHH:mm:ssZ";

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


        public DagligPuddervarsel[] ProcessResponse(XElement forecastResponse)
        {
            var timeInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var ciNo = new CultureInfo("nb-NO");
            var items = forecastResponse.DescendantsAndSelf("time");

            var xElements = items as XElement[] ?? items.ToArray();
            var powderForecastDays = new DagligPuddervarsel[42];
            var i = 0;

            var temperatureList = new Dictionary<string, string>();
            foreach (var xElement in xElements)
            {
                var fromDateTime = TimeZoneInfo.ConvertTime(DateTime.ParseExact(XmlHelper.GetAttributeValue("from", xElement), Format, ciNo), TimeZoneInfo.Local, timeInfo).ToUniversalTime();
                var toDateTime = TimeZoneInfo.ConvertTime(DateTime.ParseExact(XmlHelper.GetAttributeValue("to", xElement), Format, ciNo), TimeZoneInfo.Local, timeInfo).ToUniversalTime();

                var temperature = XmlHelper.GetElementValue("location", "temperature", "value", xElement);
                if (!string.IsNullOrEmpty(temperature))
                {
                    temperatureList.Add(fromDateTime.ToString(), temperature);
                }

                if (IsRelevant(fromDateTime, toDateTime))
                {
                    var powderForecast = new DagligPuddervarsel();

                    var precipitation = XmlHelper.GetElementValue("location", "precipitation", "value", xElement);
                    powderForecast.Precipitation = Convert.ToDecimal(precipitation.Replace('.', ','), ciNo);
                    powderForecast.From = fromDateTime.AddHours(-2);
                    powderForecast.To = toDateTime.AddHours(-2);

                    powderForecast.Temperature = GetAverageTemp(temperatureList, ciNo);
                    temperatureList = new Dictionary<string, string>();
                    powderForecastDays[i] = powderForecast;
                    i++;
                }
            }
            return powderForecastDays;
            //return testText;
        }

        private static decimal GetAverageTemp(Dictionary<string,string> temperatureList, CultureInfo ciNo)
        {
            var temp = temperatureList.Sum(temperature => Convert.ToDecimal(temperature.Value.Replace('.', ','), ciNo));

            var average = temp/temperatureList.Count;
            return Math.Round(average, 1);
        }

        private static bool IsRelevant(DateTime from, DateTime to)
        {
            if (from.Hour == 14 && to.Hour == 20)
                return true;
            if (from.Hour == 2 && to.Hour == 8)
                return true;
            if (from.Hour == 8 && to.Hour == 14)
                return true;
            if (from.Hour == 20 && to.Hour == 2)
                return true;
            return false;

            var longDate = DateTime.Now.AddDays(3).AddHours(-DateTime.Now.Hour);
            if (from < longDate)
            {
                if (from.Hour == 14 && to.Hour == 20)
                    return true;
                if (from.Hour == 2 && to.Hour == 8)
                    return true;
                if (from.Hour == 8 && to.Hour == 14)
                    return true;
                if (from.Hour == 20 && to.Hour == 2)
                    return true;
            }
            else
            {
                if (from.Hour == 4 && to.Hour == 10)
                    return true;
                if (from.Hour == 10 && to.Hour == 16)
                    return true;
                if (from.Hour == 16 && to.Hour == 22)
                    return true;
                if (from.Hour == 22 && to.Hour == 4)
                    return true;
            }
            return false;
        }

        private double GetDistance(double currentLat, double currentLong, double locationLat, double locationLong)
        {
            //radians = degrees * PI / 180

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
