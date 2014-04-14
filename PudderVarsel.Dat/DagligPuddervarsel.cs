using System;

namespace PudderVarsel.Dat
{
    public class DagligPuddervarsel
    {
        public decimal Precipitation { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Altitude { get; set; }
        public decimal Temperature { get; set; }
        public decimal MaxTemperature { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public string ImageUrl 
        { 
            get { return Precipitation > 0 ? "Snow.png" : "None.png"; }
            set { }
        }

    }
}
