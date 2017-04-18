using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    public sealed class XmlPathData
    {
        public XmlPathData(string universal) : this(universal, universal)
        {
        }

        public XmlPathData(string us, string si)
        {
            paths = new Dictionary<Unit, Lazy<XPathExpression>>();
            paths[Unit.US] = new Lazy<XPathExpression>(() => { return XPathExpression.Compile(us); }, true);
            paths[Unit.SI] = new Lazy<XPathExpression>(() => { return XPathExpression.Compile(si); }, true);
        }

        public XPathExpression GetPath(Unit unit)
        {
            if (paths.TryGetValue(unit, out var value))
            {
                return value.Value;
            }
            return null;
        }

        private readonly IDictionary<Unit, Lazy<XPathExpression>> paths;
    }
}