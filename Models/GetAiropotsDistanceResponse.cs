namespace WebApplication1.Models
{
    public class GetAiropotsDistanceResponse
    {
        //in miles
        public double Distance { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage{ get; set; }
    }
}
