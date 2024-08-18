using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AirportService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly ApiSettings _apiSettings;


        public AirportService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings
            )
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;

            _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Log Error
                    Console.WriteLine($"Attempt {retryAttempt} didn't make it. Next attempt in {timespan.TotalSeconds} seconds.");
                });

        }
        public async Task<double> GetAirportsDistanceAsync(string iataCode1, string iataCode2)
        {
            var task1 = GetAirportAsync(iataCode1);
            var task2 = GetAirportAsync(iataCode2);

            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; 
            }

            var airport1 = task1.Result;
            var airport2 = task2.Result;

            if (airport1 == null || airport2 == null)
            {
                throw new InvalidOperationException("One or both airport results are null.");
            }

            return CoordinatesHelper.CalculateDistance(airport1.Location, airport2.Location);
        }


        private async Task<Airport?> GetAirportAsync(string iataCode)
        {
            string url = $"{_apiSettings.AirportsApiUrl}/{iataCode}";

            try
            {
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(url));

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Airport>(responseBody);
            }
            catch (Exception e)
            {
                throw new Exception($"Http Error message: {e.Message}", e);
            }
        }



    }

}
