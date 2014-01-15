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
        protected void Page_Load(object sender, EventArgs e)
        {
            var powderData = (Lokasjon)Session["powderLocation"];

            if (powderData == null)
                Response.Redirect("Default.aspx");

            var day = Int32.Parse(Request.QueryString["Day"]);
            var detailedLocation = powderData.DetaljertVarsel;
            var dateForecast = detailedLocation.Where(p => p.From.Day == day);

            dateDetailResult.DataSource = dateForecast;
            dateDetailResult.DataBind();
        }
    }
}