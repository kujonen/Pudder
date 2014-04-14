using System;
using System.Collections.Generic;

namespace PudderVarsel.Dat
{
    public class Lokasjon
    {
        public enum AreaEnum
        {
            Østlandet = 0,
            Sørlandet = 1,
            Vestlandet = 2,
            Trøndelag = 3,
            NordNorge = 4,
            Ukjent = 5
        }

        public enum PrecipitationTypeEnum
        {
            Snø = 0,
            MestSnø = 1,
            Sludd = 2,
            MestRegn = 3,
            Regn = 4,
            IkkeNedbør = 5
        }

        public String Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public decimal TotalPrecipitation { get; set; }
        public decimal ThreeDaysPrecipitation { get; set; }
        public int Altitude { get; set; }
        public decimal Temperature { get; set; }
        public AreaEnum Area { get; set; }
        public double Distance { get; set; }
        public PrecipitationTypeEnum PrecipitationType { get; set; }
        public DateTime OppdatertDato { get; set; }
        public DateTime NesteOppdateringDato { get; set; }
        public IEnumerable<DagligPuddervarsel> DagligVarsel { get; set; }
        public IEnumerable<DagligPuddervarsel> DetaljertVarsel { get; set; }
        public string ImageUrl
        {
            get { return TotalPrecipitation > 0 ? "Snow.png" : "None.png"; }
        }

        /*
                             var location = new Lokasjon();
                    location.Name = name;
                    var area = int.Parse(XmlHelper.GetAttributeValue("area", xElement));
                    location.Area = (LocationPowderForecast.AreaEnum)area;

                    location.Longitude = double.Parse(XmlHelper.GetAttributeValue("longitude", xElement).Replace('.', ','), ciNo);
                    location.Latitude = double.Parse(XmlHelper.GetAttributeValue("latitude", xElement).Replace('.', ','), ciNo);
                    location.Distance = GetDistance(currentLat, currentLon, location.Latitude, location.Longitude);*/
    }
}
