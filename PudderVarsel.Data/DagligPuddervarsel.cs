using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace PudderVarsel.Data
{
    public class DagligPuddervarsel
    {
        public decimal Precipitation { get; set; }
        public decimal Powder { get; set; }
        public DateTime Day { get; set; }
        public int Altitude { get; set; }
        public decimal AverageTemperature { get; set; }
        public decimal MaxTemperature { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public IEnumerable<DetailedPowder> DetailedPowderList { get; set; }

        public string ImageUrl 
        {
            get
            {
                var daysWithPrecipitation = DetailedPowderList.Where(p => p.Precipitation > 0);
                if (daysWithPrecipitation.Any())
                {
                    var temp = daysWithPrecipitation.Sum(t => t.Temperature);
                    var averageTemp = temp/daysWithPrecipitation.Count();
                    if (averageTemp < 0.1m)
                        return "Snow.png";
                    if (averageTemp > 0.1m && averageTemp < 2.1m)
                        return "Sleet.png";
                    return "Rain.png";
                }
                return "None.png";
            }
        }
        

        public static DagligPuddervarsel Create(IEnumerable<DetailedPowder> detailedPowderList)
        {
            if (detailedPowderList == null) return null;

            var dagligPudder = new DagligPuddervarsel();
            dagligPudder.DetailedPowderList = detailedPowderList;
            dagligPudder.Day = detailedPowderList.FirstOrDefault().From;
            dagligPudder.Precipitation = detailedPowderList.Sum(t => t.Precipitation);

            dagligPudder.Powder = detailedPowderList.Sum(periode => periode.Powder);
            var temp = detailedPowderList.Average(t => t.Temperature);
            dagligPudder.AverageTemperature = Math.Round(temp, 1);
            dagligPudder.MaxTemperature = detailedPowderList.Max(t => t.Temperature);
            return dagligPudder;
        }

    }
}
