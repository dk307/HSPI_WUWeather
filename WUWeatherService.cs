using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Hspi.WUWeather
{
    using static Hspi.StringUtil;

    /// <summary>
    /// The WU Weather service. Returns weather data for given locations
    /// </summary>
    public class WUWeatherService
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
        /// <param name="includeYesterdayHistory">if set to <c>true</c> [include yesterday history].</param>
        /// <param name="includeCurrent">if set to <c>true</c> [include current].</param>
        /// <param name="includeForecast">if set to <c>true</c> [include forecast].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A <see cref="Task" /> for a <see cref="XmlDocument" /> containing the
        /// weather data from the response.
        /// </returns>
        public Task<XmlDocument> GetDataForStationAsync(string station, bool includeYesterdayHistory, bool includeCurrent, bool includeForecast, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(station))
            {
                throw new ArithmeticException("station");
            }

            StringBuilder featureBuilder = new StringBuilder();
            if (includeYesterdayHistory)
            {
                featureBuilder.Append("yesterday/");
            }
            if (includeForecast)
            {
                featureBuilder.Append("forecast/");
            }
            if (includeCurrent)
            {
                featureBuilder.Append("conditions/");
            }

            string stationUrl = INV($"http://api.wunderground.com/api/{apiKey}/{featureBuilder.ToString()}/q/pws:{station}.xml");
            return GetStringFromUrlAsync(stationUrl, cancellationToken);
        }

        /// <summary>
        /// Creates a HttpClientHandler that supports compression for responses.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpClientHandler"/> with compression support.
        /// </returns>
        private static HttpClientHandler GetCompressionHandler()
        {
            var compressionHandler = new HttpClientHandler();
            if (compressionHandler.SupportsAutomaticDecompression)
            {
                compressionHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return compressionHandler;
        }

        /// <summary>
        /// Throws an exception if the given response didn't have a status code
        /// indicating success, with the status code included in the exception message.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <exception cref="HttpRequestException">
        /// Thrown if the response did not have a status code indicating success.
        /// </exception>
        private static void ThrowExceptionIfResponseError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Couldn't retrieve data: status " + response.StatusCode);
            }
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
        private async Task<XmlDocument> GetStringFromUrlAsync(string requestUrl, CancellationToken cancellationToken)
        {
            using (var compressionHandler = GetCompressionHandler())
            {
                using (var client = new HttpClient(compressionHandler))
                {
                    var response = await client.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);
                    ThrowExceptionIfResponseError(response);
                    return await ParseStringFromResponse(response).ConfigureAwait(false);
                }
            }
        }
    }
}