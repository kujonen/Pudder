using System;

namespace PudderVarsel.Data
{
    public class DetailedPowder
    {
        public decimal Precipitation { get; set; }
        public decimal Powder { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Altitude { get; set; }
        public decimal Temperature { get; set; }
        public decimal MaxTemperature { get; set; }

        public string ImageUrl 
        {
            get
            {
                if (Precipitation == 0)
                    return "None.png";
                if (Temperature <= 0.1m)
                    return "Snow.png";
                if (Temperature > 0.1m && Temperature < 2.1m)
                    return "Sleet.png";
                return "Rain.png";
            }
        }

    }
}
