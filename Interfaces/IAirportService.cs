using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IAirportService
    {
       public Task<double> GetAirportsDistanceAsync(string iataCode1, string iataCode2);
    }
}
