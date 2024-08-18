using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiroportController : ControllerBase
    {
        private readonly AirportService _airportService;

        public AiroportController(AirportService airportService)
        {
            this._airportService = airportService;
        }

        [HttpPost]
        public async Task<GetAiropotsDistanceResponse> GetAiropotsDistance(GetAiropotsDistanceRequest req)
        {
            try
            {
                var result = await _airportService.GetAirportsDistanceAsync(req.FirstAirportCodeName, req.SecondAirportCodeName);
                return new GetAiropotsDistanceResponse()
                {
                    HasError = false,
                    Distance = result
                };
            }
            catch (Exception ex)
            {
                return new GetAiropotsDistanceResponse()
                {
                    HasError = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
