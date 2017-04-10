using System;
using System.Runtime.Serialization;

namespace Hspi.Exceptions
{
    [Serializable]
    internal class WUWeatherDataInvalidException : HspiException
    {
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