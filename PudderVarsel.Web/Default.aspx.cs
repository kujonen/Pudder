using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PudderVarsel.Data;


namespace PudderVarsel.Web
{
    public partial class _Default : Page, IPostBackEventHandler
    {
        public static IEnumerable<Lokasjon> PudderVarsel { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int TimeSpent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            PudderVarsel = (IEnumerable<Lokasjon>) Session["PudderVarsel"];

            var lon = Session["Longitude"];
            if (lon != null)
                Longitude = (double) lon;

            var lat = Session["Latitude"];
            if (lat != null)
                Latitude = (double) lat;

            if (Equals(Longitude, 0.0))
                Page.ClientScript.RegisterStartupScript(GetType(), "Call my function", "requestPosition()", true);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "LocationOK")
            {
                var ciNo = new CultureInfo("nb-NO");
                Longitude = double.Parse(longitude.Text.Replace('.', ','), ciNo);
                Latitude = double.Parse(latitude.Text.Replace('.', ','), ciNo);

                Session["Latitude"] = Latitude;
                Session["Longitude"] = Longitude;

                ButtonSearch.Enabled = true;
            }
        }

        void Page_PreInit(object sender, EventArgs e)
        {
            var p = Context.Handler as Page;
            if (p != null)
            {
                // set master page
                if (Request.Browser.IsMobileDevice)
                {
                    p.MasterPageFile = "Site.Mobile.Master";
                }
                else
                {
                    p.MasterPageFile = "Site.Master";
                }

            }
        } 

        private string FetchLocations()
        {
            var doc = new XmlDocument();
            doc.Load(Server.MapPath(@"~/bin/Locations.xml"));
            return doc.InnerXml;

        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            var completionSet = new List<string>();

            var suggestions = PudderVarsel.Where(p => p.Name.Contains(prefixText));

            foreach (var lokasjon in suggestions)
            {
                completionSet.Add(lokasjon.Name);
            }

            return completionSet.ToArray();

        }

        private void LastPudderVarsel(string searchText, int distance)
        {
            var data = new WeatherData();
            PudderVarsel = data.GetLocationForecast(Latitude, Longitude, FetchLocations(), distance, searchText);


            foreach (var lokasjon in PudderVarsel)
            {
                var grunndata = MetClient.GetForecast(lokasjon.Latitude, lokasjon.Longitude);

                var dagligVarsel = data.ProcessResponse(grunndata).Where(p => p != null);
                var dagligPuddervarselListe = dagligVarsel as IList<DagligPuddervarsel> ?? dagligVarsel.ToList();
                lokasjon.DagligVarsel = dagligPuddervarselListe;

                lokasjon.OppdatertDato = XmlHelper.GetDate(grunndata.DescendantsAndSelf("model").FirstOrDefault(), "runended");
                lokasjon.NesteOppdateringDato = XmlHelper.GetDate(grunndata.DescendantsAndSelf("model").FirstOrDefault(), "nextrun");

                var totalPrecipitation = dagligPuddervarselListe.Sum(p => p.Precipitation);
                var threeDays = dagligPuddervarselListe.Where(p => p.Day < DateTime.Now.AddDays(2)).Sum(t => t.Precipitation);
                lokasjon.ThreeDaysPrecipitation = threeDays;
                lokasjon.TotalPrecipitation = totalPrecipitation;

                //lokasjon.PrecipitationType = Utils.CalculatePrecipitationType(weatherData);

                //location.LocationUrl = string.Format("http://maps.google.no/maps?q=N+{0}+E+{1}",
                //                                     location.Latitude.ToString(ciUs), location.Longitude.ToString(ciUs));
            }

            var sortedPowder = PudderVarsel.Where(p => p != null).OrderByDescending(p => p.TotalPrecipitation);

            ListViewLocations.DataSource = sortedPowder;
            ListViewLocations.DataBind();

            Session["PudderVarsel"] = PudderVarsel;
        }

        protected void DropDownListDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        protected void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {

        }


        protected void Details_Click(object sender, CommandEventArgs e)
        {
            //var powderList = (IEnumerable<Lokasjon>)Session["powderList"];
            var locationName = e.CommandArgument.ToString();
            var location = PudderVarsel.FirstOrDefault(p => p.Name == locationName);

            Session["powderLocation"] = location;

            Response.Redirect("PowderDetails.aspx?Location=" + locationName);
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxSearch.Text) && TextBoxSearch.Text != "Navn på alpinsenter")
            {
                LastPudderVarsel(TextBoxSearch.Text, 0);
            }
            if (DropDownListDistance.SelectedIndex != 0)
            {
                var distance = int.Parse(DropDownListDistance.SelectedValue);
                LastPudderVarsel(string.Empty, distance);
            }
        }

        protected void SortThreeDays_Click(object sender, CommandEventArgs e)
        {
            var sortedPowder = PudderVarsel.OrderByDescending(p => p.ThreeDaysPrecipitation);

            ListViewLocations.DataSource = sortedPowder;
            ListViewLocations.DataBind();

        }

        protected void SortDistance_Click(object sender, CommandEventArgs e)
        {
            var sortedPowder = PudderVarsel.OrderBy(p => p.Distance);

            ListViewLocations.DataSource = sortedPowder;
            ListViewLocations.DataBind();
        }
    }
}