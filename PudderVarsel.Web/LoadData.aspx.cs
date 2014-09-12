﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using PudderVarsel.Data;

namespace PudderVarsel.Web
{
    public partial class LoadData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadDataButton_Click(object sender, EventArgs e)
        {
            while (true)
            {
                LoadFreshData();
                Thread.Sleep(500000);
            }
            
        }

        private void LoadFreshData()
        {
            LoadDataButton.Enabled = false;

            var data = new WeatherData();
            var lokasjonerXml = FetchLocations();
            var locations = data.GetAllLocations(lokasjonerXml);
            var dir = Server.MapPath(@"~/bin/Data/");

            foreach (var lokasjon in locations)
            {
                var grunndata = MetClient.GetForecast(lokasjon.Latitude, lokasjon.Longitude);
                data.SaveForecastToFile(grunndata, Server.MapPath(@"~/bin/Data/" + lokasjon.Name + ".xml"), dir);
            }

            LoadDataButton.Enabled = true;
        }
        private string FetchLocations()
        {
            var doc = new XmlDocument();
            doc.Load(Server.MapPath(@"~/bin/Locations.xml"));
            return doc.InnerXml;

        }
    }
}