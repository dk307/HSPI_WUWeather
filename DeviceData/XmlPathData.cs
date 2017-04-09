using System;
using System.Collections.Generic;

namespace Hspi
{
    public sealed class XmlPathData
    {
        public XmlPathData(string universal)
        {
            paths = new Dictionary<Unit, string>();
            foreach (var value in Enum.GetValues(typeof(Unit)))
            {
                paths[(Unit)value] = universal;
            }
        }

        public XmlPathData(IDictionary<Unit, string> paths)
        {
            this.paths = paths;
        }

        public XmlPathData(string us, string si)
        {
            paths = new Dictionary<Unit, string>();
            paths[Unit.US] = us;
            paths[Unit.SI] = si;
        }

        public string GetPath(Unit unit)
        {
            paths.TryGetValue(unit, out string value);
            return value;
        }

        private readonly IDictionary<Unit, string> paths;
    }
}