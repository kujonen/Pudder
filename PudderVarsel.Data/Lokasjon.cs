using System;
using System.Collections.Generic;

namespace PudderVarsel.Data
{
    public class Lokasjon
    {
        public enum AreaEnum
        {
            �stlandet = 0,
            S�rlandet = 1,
            Vestlandet = 2,
            Tr�ndelag = 3,
            NordNorge = 4,
            Ukjent = 5
        }

        public enum PrecipitationTypeEnum
        {
            Sn� = 0,
            MestSn� = 1,
            Sludd = 2,
            MestRegn = 3,
            Regn = 4,
            IkkeNedb�r = 5
        }

        public String Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public decimal TotalPrecipitation { get; set; }
        public decimal TotalPowder { get; set; }
        public decimal ThreeDaysPrecipitation { get; set; }
        public decimal ThreeDaysPowder { get; set; }
        public int Altitude { get; set; }
        public decimal Temperature { get; set; }
        public AreaEnum Area { get; set; }
        public double Distance { get; set; }
        public PrecipitationTypeEnum PrecipitationType { get; set; }
        public DateTime OppdatertDato { get; set; }
        public DateTime NesteOppdateringDato { get; set; }
        public DateTime SisteDataHenting { get; set; }
        public bool HentetFraMet { get; set; }

        public IEnumerable<DagligPuddervarsel> DagligVarsel { get; set; }
        public string ImageUrl
        {
            get
            {
                if (TotalPrecipitation == 0)
                    return "None.png";
                if (Temperature < 0)
                    return "Snow.png";
                if (Temperature > 0 && Temperature < 2)
                    return "Sleet.png";
                return "Rain.png";
            }
        }
    }
}
