using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            var powderData = (Lokasjon)Session["powderLocation"];
            if (powderData == null)
                Response.Redirect("Default.aspx");

            powderDetailResult.DataSource = powderData.DagligVarsel;
            powderDetailResult.DataBind();
        }

        protected void Date_Click(object sender, CommandEventArgs e)
        {
            var date = Convert.ToDateTime(e.CommandArgument.ToString());
            Response.Redirect("DateDetails.aspx?Day=" + date.Day);
        }
    }
}