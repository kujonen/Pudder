using System;
using System.Linq;
using PudderVarsel.Data;

namespace PudderVarsel.Web
{
    public partial class DateDetails : System.Web.UI.Page
    {
        public string Date
        {
            get
            {
                var location = Request.QueryString["Date"];
                return location;
            }

        }

        public string Location
        {
            get
            {
                var location = Request.QueryString["Location"];
                return location;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var location = (Lokasjon)Session["powderLocation"];

            if (location == null)
                Response.Redirect("Default.aspx");

            var day = Int32.Parse(Request.QueryString["Day"]);
            var detailedForecast = location.DagligVarsel.FirstOrDefault(p => p.Day.Day == day).DetailedPowderList; ;
            //var dateForecast = detailedLocation.Where(p => p.From.Day == day);

            dateDetailResult.DataSource = detailedForecast;
            dateDetailResult.DataBind();
        }
    }
}