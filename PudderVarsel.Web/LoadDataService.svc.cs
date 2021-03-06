﻿using System;
using System.IO;
using System.Reflection;
using System.Xml;
using PudderVarsel.Data;

namespace PudderVarsel.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoadDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LoadDataService.svc or LoadDataService.svc.cs at the Solution Explorer and start debugging.
    public class LoadDataService : ILoadDataService
    {
        public void LoadData(System.Xml.Linq.XElement location, string name)
        {
            var data = new WeatherData();

            var dir = GetDir("Data");

            var path =  dir + "\\" + name + ".xml";
            data.SaveForecastToFile(location, path, dir);
        }

        private static string GetDir(string subdir)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var dir = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            if (!String.IsNullOrEmpty(subdir))
                return dir + "\\" + subdir;
            return dir;
        }

        public string FetchLocations()
        {
            var doc = new XmlDocument();
            var path = GetDir(string.Empty) + "\\Locations.xml";
            doc.Load(path);
            return doc.InnerXml;
        }
    }
}
