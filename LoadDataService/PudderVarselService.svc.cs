using System;
using System.IO;
using System.Reflection;
using PudderVarsel.Data;

namespace LoadDataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PudderVarselService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PudderVarselService.svc or PudderVarselService.svc.cs at the Solution Explorer and start debugging.
    public class PudderVarselService : IPudderVarselService
    {
        public void LoadData(System.Xml.Linq.XElement location, string name)
        {
            var data = new WeatherData();

            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var dir =  Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)) + "\\Data";
            var path = dir + "\\" + name + ".xml";
            data.SaveForecastToFile(location, path, dir);
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
