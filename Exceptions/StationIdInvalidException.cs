using System;
using System.Runtime.Serialization;

namespace Hspi.Exceptions
{
    [Serializable]
    internal class StationIdInvalidException : WUWeatherDataInvalidException
    {
        public StationIdInvalidException() : base("Invalid Station Id")
        {
        }

        protected StationIdInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}