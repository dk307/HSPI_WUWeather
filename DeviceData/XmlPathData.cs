using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlPathData"/> class.
        /// </summary>
        /// <param name="us">US Unit XMLPath.</param>
        /// <param name="si">SI Uniy XMLPath</param>
        public XmlPathData(string us, string si)
        {
            paths = new Dictionary<Unit, string>();
            paths[Unit.US] = us;
            paths[Unit.SI] = si;
        }

        /// <summary>
        /// Gets the XMLPath path for a Unit
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns>XMLPath</returns>
        public string GetPath(Unit unit)
        {
            // let it throw on not found
            return paths[unit];
        }

        private readonly IDictionary<Unit, string> paths;
    }
}