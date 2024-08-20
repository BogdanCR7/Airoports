using Polly;
using WebApplication1.Interfaces;

namespace WebApplication1.Service
{
    public class RetryPolicyService : IRetryPolicyService
    {
        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
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
    }

}
