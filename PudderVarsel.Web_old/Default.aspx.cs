using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PudderVarsel.Data;

namespace PudderVarsel.Web
{
    public partial class _Default : Page
    {
        public IEnumerable<Lokasjon> PudderVarsel { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string FetchLocations(double currentLat, double currentLon)
        {
            var doc = new XmlDocument();
            doc.Load(Server.MapPath(@"~/bin/Locations.xml"));
            return doc.InnerXml;
            
        }


        private void LastPudderVarsel()
        {
            var Longitude = 70.4;
            var Latitude = 7.4;
            var data = new WeatherData();
            PudderVarsel = data.GetLocationForecast(Latitude, Longitude, FetchLocations(Latitude, Longitude), 100);
            foreach (var lokasjon in PudderVarsel)
            {
                //var t = new MetClient();
                var grunndata = MetClient.GetForecast(lokasjon.Latitude.ToString(), lokasjon.Longitude.ToString());

                var weatherData = data.ProcessResponse(grunndata);

                var filteredPowderData = Utils.GetRelevantPowderData(weatherData);
                var dayByDayPowderData = Utils.GetDailyPowderData(filteredPowderData);

                var byDayPowderData = dayByDayPowderData as IList<DagligPuddervarsel> ?? dayByDayPowderData.ToList();
                lokasjon.DagligVarsel = byDayPowderData;

                var totalPrecipitation = byDayPowderData.Sum(p => p.Precipitation);
                var threeDays = byDayPowderData.Where(p => p.From < DateTime.Now.AddDays(2)).Sum(t => t.Precipitation);
                lokasjon.ThreeDaysPrecipitation = threeDays;
                lokasjon.TotalPrecipitation = totalPrecipitation;

                //location.LocationUrl = string.Format("http://maps.google.no/maps?q=N+{0}+E+{1}",
                //                                     location.Latitude.ToString(ciUs), location.Longitude.ToString(ciUs));
            }

            var sortedPowder = PudderVarsel.Where(p => p != null).OrderByDescending(p => p.TotalPrecipitation);

            ListViewLocations.ItemsSource = sortedPowder;
        }

        protected void Details_Click(object sender, CommandEventArgs e)
        {
            var powderList = (IEnumerable<Lokasjon>)Session["powderList"];
            var locationName = e.CommandArgument.ToString();
            var location = powderList.FirstOrDefault(p => p.Name == locationName);
            Session["powderLocation"] = location;

            Response.Redirect("PowderDetails.aspx?Location=" + locationName);
        }
    }
}