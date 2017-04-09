using System;
using System.Runtime.Serialization;

namespace Hspi.Exceptions
{
    [Serializable]
    public class WUWeatherDataInvalidException : HspiException
    {
        public WUWeatherDataInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WUWeatherDataInvalidException()
        {
        }

        public WUWeatherDataInvalidException(string message) : base(message)
        {
        }

        protected WUWeatherDataInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}