using Hspi.Exceptions;
using NullGuard;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Hspi.WUWeather
{
    using static Hspi.StringUtil;

    /// <summary>
    /// The WU Weather service. Returns weather data for given station
    /// </summary>
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WUWeatherService
    {
        /// <summary>
        /// The API key to use in all requests.
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="WUWeatherService" /> class.
        /// </summary>
        /// <param name="key">The API key to use.</param>
        public WUWeatherService(string key)
        {
            apiKey = key;
        }

        /// <summary>
        /// Asynchronously retrieves weather data for a particular station.
        /// </summary>
        /// <param name="station">The station.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> for a <see cref="XmlDocument" /> containing the
        /// weather data from the response.
        /// </returns>
        public Task<XmlDocument> GetDataForStationAsync(string station, CancellationToken cancellationToken)
        {
            string stationUrl = INV($"http://api.wunderground.com/api/{apiKey}/yesterday/forecast/conditions/q/pws:{station}.xml");
            return GetXmlFromUrlAsync(stationUrl, cancellationToken);
        }

        /// <summary>
        /// Given a successful response from the string service, parses the
        /// weather data contained within and returns it.
        /// </summary>
        /// <param name="response">A successful response containing weather data.</param>
        /// <returns>
        /// A <see cref="Task" /> for a <see cref="XmlDocument" /> containing the
        /// weather data from the response.
        /// </returns>
        private static async Task<XmlDocument> ParseStringFromResponse(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseString);
            return xmlDocument;
        }

        /// <summary>
        /// Given a formatted URL containing the parameters for a string
        /// request, retrieves, parses, and returns weather data from it.
        /// </summary>
        /// <param name="requestUrl">The full URL from which the request should be made, including
        /// the API key and other parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> for a <see cref="XmlDocument" /> containing
        /// weather data.
        /// </returns>
        private static async Task<XmlDocument> GetXmlFromUrlAsync(string requestUrl, CancellationToken cancellationToken)
        {
            using (var compressionHandler = new HttpClientHandler())
            {
                if (compressionHandler.SupportsAutomaticDecompression)
                {
                    compressionHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                }

                using (var client = new HttpClient(compressionHandler))
                {
                    var response = await client.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new WUWeatherDataInvalidException(INV($"Couldn't retrieve data: status {response.StatusCode}"));
                    }
                    var xmlDoument = await ParseStringFromResponse(response).ConfigureAwait(false);
                    CheckErrorinResponse(xmlDoument);
                    return xmlDoument;
                }
            }
        }

        /// <summary>
        /// Checks errors in response.
        /// </summary>
        /// <param name="xmlDoument">The XML response.</param>
        /// <exception cref="ApiKeyInvalidException"></exception>
        /// <exception cref="StationIdInvalidException"></exception>
        /// <exception cref="WUWeatherDataInvalidException"></exception>
        private static void CheckErrorinResponse(XmlDocument xmlDoument)
        {
            var featureNodes = xmlDoument.SelectNodes("/response/*[self::current_observation or self::forecast or self::history]");

            if (featureNodes.Count == 0)
            {
                var errorNode = xmlDoument.SelectSingleNode("/response/error/type");
                string errorDescription = errorNode != null ? errorNode.InnerText : "Unknown";

                switch (errorDescription.ToUpperInvariant())
                {
                    case "KEYNOTFOUND":
                        throw new ApiKeyInvalidException();
                    case "STATION:OFFLINE":
                        throw new StationIdInvalidException();
                }
                throw new WUWeatherDataInvalidException(INV($"Server Returned:{errorDescription}"));
            }
        }
    }
}