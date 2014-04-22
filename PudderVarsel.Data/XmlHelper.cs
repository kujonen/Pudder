using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PudderVarsel.Data
{
    public class XmlHelper
    {
        public static string GetElementValue(string elementName1, string elementName2, string attributeName, XElement element)
        {
            var value = string.Empty;
            var xElement1 = element.Element(elementName1);
            if (xElement1 == null) return value;
            var xElement2 = xElement1.Element(elementName2);

            if (xElement2 != null)
            {
                var xAttribute = xElement2.Attribute(attributeName);

                if (xAttribute != null)
                {
                    value = xAttribute.Value;
                }
            }
            return value;
        }

        public static string GetAttributeValue(string attributeName, XElement element)
        {
            var xAttribute = element.Attribute(attributeName);

            var value = string.Empty;
            if (xAttribute != null)
            {
                value = xAttribute.Value;
            }
            return value;
        }

        public static DateTime GetDate(XElement element, string attributeValue)
        {
            const string format = "yyyy-MM-ddTHH:mm:ssZ";
            var ciNo = new CultureInfo("nb-NO");

            var value = XmlHelper.GetAttributeValue(attributeValue, element);
            var date = DateTime.ParseExact(value, format, ciNo);

            var timeInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            return TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, timeInfo).ToUniversalTime();
        }
    }
}