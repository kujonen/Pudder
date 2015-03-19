using System;
using System.Globalization;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace PudderVarsel
{
    internal class MetClient
    {
        public static XElement GetForecast(double lat, double lon)
        {
            var ciNo = new CultureInfo("nb-NO");

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");


                    var searchRequestUrl = "http://api.met.no/weatherapi/locationforecast/1.9/?lat=" + lat.ToString(ciNo).Replace(',', '.') + ";" + "lon=" + lon.ToString(ciNo).Replace(',', '.');
                    //"lat=60.10;lon=9.58";

                    var request = WebRequest.Create(searchRequestUrl) as HttpWebRequest;
                    if (request != null)
                    {
                        var response = request.GetResponse() as HttpWebResponse;

                        var xmlDoc = new XmlDocument();
                        if (response != null)
                        {
                            var test = response.GetResponseStream();
                            xmlDoc.Load(test);
                            var t = XElement.Parse(xmlDoc.InnerXml);
                            
                            return t;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error here
                // Allow exception to bubble up

                throw ex;
            }

            return null;
        }


    }
}
