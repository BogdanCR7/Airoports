using Polly;

namespace WebApplication1.Interfaces
{
    public interface IRetryPolicyService
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    }

}
