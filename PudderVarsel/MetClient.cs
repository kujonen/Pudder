using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PudderVarsel
{
    internal class MetClient
    {
        public static XElement GetForecast(string lat, string lon)
        {
            var metRestUrl = "http://api.met.no/weatherapi/locationforecast/1.8/?lat=" + lat + ";" + "lon=" + lon;

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(metRestUrl);
                if (response.Result.IsSuccessStatusCode)
                {
                    // by calling .Result you are performing a synchronous call
                    var responseContent = response.Result.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;

                    var xmlWeather = XElement.Parse(responseString);

                    return xmlWeather;

                }
            }
            return null;
        }
    }
}
