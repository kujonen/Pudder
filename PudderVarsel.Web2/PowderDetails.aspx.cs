using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using PudderVarsel.Data;

namespace PudderVarsel.Web
{
    public partial class PowderDetails : Page
    {
        public string Location
        {
            get
            {
                var location = Request.QueryString["Location"];
                return location;
            }

        }

        public string Totalt { get; set; }
        public string TreDager { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var powderData = (Lokasjon)Session["powderLocation"];
            if (powderData == null)
                Response.Redirect("Default.aspx");

            var ciNo = new CultureInfo("nb-NO");

            Totalt = powderData.TotalPrecipitation.ToString();
            TreDager = powderData.ThreeDaysPrecipitation.ToString();

            powderDetailResult.DataSource = powderData.DagligVarsel;
            powderDetailResult.DataBind();

            //Chart1.Series[0].YValueMembers = "Volume1";
            //Chart1.Series[1].YValueMembers = "Volume2";
            //Chart1.Series[0].XValueMember = "Date";

            //Chart1.Titles.Add("Employees");
            //Chart1.Series.Add(new Series("Peter", 2));
            //Chart1.Series.Add(new Series("Jon", 5));
            //Chart1.Series.Add(new Series("Pan", 3));

            foreach (var dagligPuddervarsel in powderData.DagligVarsel)
            {
                var t = dagligPuddervarsel.From.ToString("ddd");
                Chart1.Series["test1"].Points.AddXY(t, dagligPuddervarsel.Precipitation);
            }


            Chart1.Series["test1"].ChartType = SeriesChartType.Line;
            Chart1.Series["test1"].Color = Color.Blue;
            
        }

        protected void Date_Click(object sender, CommandEventArgs e)
        {
            var date = Convert.ToDateTime(e.CommandArgument.ToString());
            Response.Redirect("DateDetails.aspx?Day=" + date.Day);
        }
    }
}