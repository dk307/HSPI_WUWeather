using System;
using System.Runtime.Serialization;

namespace Hspi.Exceptions
{
    [Serializable]
    public class StationIdInvalidException : WUWeatherDataInvalidException
    {
        public StationIdInvalidException() : base("Invalid Station Id")
        {
        }

        protected StationIdInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}