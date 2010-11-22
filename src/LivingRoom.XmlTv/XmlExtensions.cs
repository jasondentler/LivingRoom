using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LivingRoom.XmlTv
{
    public static class XmlExtensions
    {

        public static XElement SafeElement(this XElement element, string name)
        {
            return element.Element(name) ?? new XElement(name);
        }

        public static XAttribute SafeAttribute(this XElement element, string name)
        {
            return element.Attribute(name) ?? new XAttribute(name, "");
        }

        public static string AttrValue(this XElement element, string name)
        {
            return element.SafeAttribute(name).Value;
        }

        public static string ElementValue(this XElement element, string name)
        {
            return element.SafeElement(name).Value;
        }

        public static string ElementValue(this XElement element, string name, string lang)
        {
            var found = element.Elements(name)
                .FirstOrDefault(e => e.Attributes("lang").Any(a => a.Value == lang));
            return found != null ? found.Value : string.Empty;
        }

        public static IEnumerable<string> ElementValues(this XElement element, string name)
        {
            return element.Elements(name).Select(e => e.Value);
        }

        public static IEnumerable<string> ElementValues(this XElement element, string name, string lang)
        {
            return element.Elements(name)
                .Where(e => e.Attributes("lang").Any(a => a.Value == lang))
                .Select(e => e.Value);
        }

    }
}
