using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    /// <summary>
    /// Class to describe XMLPaths for a Device for various  Units
    /// </summary>
    public sealed class XmlPathData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlPathData"/> class.
        /// </summary>
        /// <param name="universal">Single xmlpath for all Unit.</param>
        public XmlPathData(string universal) :
            this(universal, universal)
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