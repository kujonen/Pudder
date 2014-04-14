using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace PudderVarsel.Data
{
    public class Utils
    {
        public static IEnumerable<DagligPuddervarsel> GetRelevantPowderData(DagligPuddervarsel[] forecast)
        {
            var filteredForecast = forecast.Where(p => p != null && ((p.From.Hour == 12 && p.To.Hour == 18) ||
                (p.From.Hour == 00 && p.To.Hour == 06) || (p.From.Hour == 06 && p.To.Hour == 12) ||
                (p.From.Hour == 18 && p.To.Hour == 00))).OrderBy(q => q.From);

            var longDate = DateTime.Now.AddDays(3).AddHours(-DateTime.Now.Hour);
            var longForecast = forecast.Where(p => p != null && p.From > longDate && ((p.From.Hour == 01 && p.To.Hour == 07) ||
                (p.From.Hour == 07 && p.To.Hour == 13) || (p.From.Hour == 13 && p.To.Hour == 19) ||
                (p.From.Hour == 19 && p.To.Hour == 01))).OrderBy(q => q.From);

            if (!longForecast.Any()) 
            {
                longForecast = forecast.Where(p => p != null && p.From > longDate && ((p.From.Hour == 02 && p.To.Hour == 08) ||
                (p.From.Hour == 08 && p.To.Hour == 14) || (p.From.Hour == 14 && p.To.Hour == 20) ||
                (p.From.Hour == 20 && p.To.Hour == 02))).OrderBy(q => q.From);
            }
            var totalForecast = filteredForecast.Concat(longForecast);
            return totalForecast;
        }


        public static IEnumerable<DagligPuddervarsel> GetDailyPowderData(IEnumerable<DagligPuddervarsel> filteredPowderData)
        {
            var today = DateTime.Now;
            var todayStart = DateTime.Now.AddHours(-(DateTime.Now.Hour + 1));
            var todayEnd = DateTime.Now.AddDays(1).AddHours(-(DateTime.Now.Hour + 1));

            const int days = 11;
            var dailyPowderData = new DagligPuddervarsel[days - 1];

            for (var i = 1; i < days; i++)
            {
                var dayPowderData = filteredPowderData.Where(p => p.From < todayEnd && p.From > todayStart);
                var totalPowder = dayPowderData.Sum(p => p.Precipitation);

                var puddervarsel = new DagligPuddervarsel();
                if (dayPowderData.Count() > 0)
                {
                    var temp = dayPowderData.Average(t => t.Temperature);
                    puddervarsel.Temperature = Math.Round(temp, 1);
                    puddervarsel.MaxTemperature = dayPowderData.Max(t => t.Temperature);
                }
                
                
                puddervarsel.Precipitation = totalPowder;
                
                
                puddervarsel.From = today;
                dailyPowderData[i - 1] = puddervarsel;
                today = today.AddDays(1);
                todayStart = todayStart.AddDays(1);
                todayEnd = todayEnd.AddDays(1);
            }
            return dailyPowderData;
        }

        public static Lokasjon.PrecipitationTypeEnum CalculatePrecipitationType(IEnumerable<DagligPuddervarsel> data)
        {
            var dagerMedNedbor = data.Where(p => p.Precipitation > 0);
            if (!dagerMedNedbor.Any())
                return Lokasjon.PrecipitationTypeEnum.IkkeNedbør;

            var dagerMedSno = dagerMedNedbor.Count(p => p.Temperature < 1);

            if (dagerMedSno == 0)
                return Lokasjon.PrecipitationTypeEnum.Regn;

            var prosentAndelSno = ProsentAvNedborErSno(dagerMedNedbor.Count(), dagerMedSno);

            if (Equals(prosentAndelSno, 100.0))
                return Lokasjon.PrecipitationTypeEnum.Snø;

            if (prosentAndelSno > 70)
                return Lokasjon.PrecipitationTypeEnum.MestSnø;

            if (prosentAndelSno > 50)
                return Lokasjon.PrecipitationTypeEnum.Sludd;

            if (prosentAndelSno > 30)
                return Lokasjon.PrecipitationTypeEnum.MestRegn;

            return Lokasjon.PrecipitationTypeEnum.Regn;
        }

        public static DateTime GetDate(XElement element, string attributeValue)
        {
            const string format = "yyyy-MM-ddTHH:mm:ssZ";
            var ciNo = new CultureInfo("nb-NO");

            var value = XmlHelper.GetAttributeValue(attributeValue, element);

            var date = DateTime.ParseExact(value, format, ciNo);
            return date;
            //return new DateTime(date.Ticks, DateTimeKind.Local);
        }

        private static double ProsentAvNedborErSno(int dagerMedNedbor, int dagerMedSno)
        {
            var prosent = ((double)dagerMedSno / (double)dagerMedNedbor) * 100;
            return prosent;
        }


    }
}
