using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AirportService: IAirportService
    {
        private readonly HttpClient _httpClient;
        private readonly IRetryPolicyService _retryPolicyService;
        private readonly ApiSettings _apiSettings;


        public AirportService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            IRetryPolicyService retryPolicyService
            )
        {
            _httpClient = httpClient;
            _retryPolicyService = retryPolicyService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<double> GetAirportsDistanceAsync(string iataCode1, string iataCode2)
        {
            var airoport1Task = GetAirportAsync(iataCode1);
            var airoport2Task = GetAirportAsync(iataCode2);

            try
            {
                await Task.WhenAll(airoport1Task, airoport2Task);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; 
            }

            var airport1 = airoport1Task.Result;
            var airport2 = airoport2Task.Result;

            if (airport1 == null || airport2 == null)
            {
                throw new InvalidOperationException("One or both airport results are null.");
            }

            return CoordinatesHelper.CalculateDistance(airport1.Location, airport2.Location);
        }

        private async Task<Airport?> GetAirportAsync(string iataCode)
        {
            string url = $"{_apiSettings.AirportsApiUrl}/{iataCode}";
            var _retryPolicy = _retryPolicyService.GetRetryPolicy();
            try
            {
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Airport>(responseBody);
            }
            catch (Exception e)
            {
                //Log
                Console.WriteLine($"Http Error message: {e.Message}");
                throw;
            }
        }
    }

}
