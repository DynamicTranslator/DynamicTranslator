namespace DynamicTranslator.Core.Extensions
{
    using System;
    using System.Xml.Linq;

    public static class XmlExtensions
    {
        public static bool IsXml(this string @this)
        {
            try
            {
                XElement.Parse(@this);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}