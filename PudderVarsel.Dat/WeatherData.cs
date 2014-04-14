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

            var items = forecastResponse.DescendantsAndSelf("time");

            var xElements = items as XElement[] ?? items.ToArray();
            var powderForecastDays = new DagligPuddervarsel[xElements.Count()];
            var i = 0;
            string temperature = string.Empty;
            foreach (var xElement in xElements)
            {
                var from = XmlHelper.GetAttributeValue("from", xElement);
                var to = XmlHelper.GetAttributeValue("to", xElement);
                var precipitation = XmlHelper.GetElementValue("location", "precipitation", "value", xElement);
                var temp = XmlHelper.GetElementValue("location", "temperature", "value", xElement);
                if (!string.IsNullOrEmpty(temp))
                    temperature = temp;
                if (!string.IsNullOrEmpty(precipitation))
                {
                    const string format = "yyyy-MM-ddTHH:mm:ssZ";
                    var powderForecast = new DagligPuddervarsel();

                    var ciNo = new CultureInfo("nb-NO");
                    powderForecast.Precipitation = Convert.ToDecimal((string) precipitation.Replace('.', ','), ciNo);
                    powderForecast.From = DateTime.ParseExact(from, format, ciNo);
                    powderForecast.To = DateTime.ParseExact(to, format, ciNo);

                    
                    if (!string.IsNullOrEmpty(temperature))
                        powderForecast.Temperature = Math.Round(Convert.ToDecimal((string)temperature.Replace('.', ','), ciNo),1);

                    powderForecastDays[i] = powderForecast;
                    i++;
                }
            }
            return powderForecastDays;
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
        //private void GetPowder(IEnumerable<LocationPowderForecast> powderData)
        //{
        //    powderResult.DataSource = powderData;
        //    powderResult.DataBind();
        //}


        //public static async void GetPowderData(string lat, string lon)
        //{
        //    try
        //    {

        //        HttpClient client = new HttpClient();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
        //        var searchRequestUrl = "http://api.met.no/weatherapi/locationforecast/1.8/?lat=" + lat + ";" + "lon=" + lon;
        //        HttpResponseMessage response = await client.GetAsync(searchRequestUrl);
        //        var responseString = response.Content.ReadAsStringAsync();
        //        //responseString.
        //        //using (var client = new WebClient())
        //        //{
        //        //    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


        //        //    //"lat=60.10;lon=9.58";

        //        //    var request = WebRequest.Create(searchRequestUrl) as HttpWebRequest;
        //        //    if (request != null)
        //        //    {
        //        //        var response = request.GetResponse() as HttpWebResponse;
        //        //        var respondse = request.() as HttpWebResponse;

        //        //        var xmlDoc = new XmlDocument();
        //        //        if (response != null) xmlDoc.Load(response.GetResponseStream());

        //        //        responseWeather = ProcessResponse(xmlDoc);
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error here
        //        // Allow exception to bubble up

        //        throw ex;
        //    }

        //    //return responseWeather;
        //}

    }

}
