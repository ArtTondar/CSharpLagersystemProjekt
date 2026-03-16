using Lagersystem.Blazor.Models.Dtos;

namespace Lagersystem.Blazor.Models.Responses
{
    // Placeholder:
    // Bruges ikke aktivt endnu, fordi API'et returnerer Order direkte.
    // Klassen er klar til fremtidige response-wrappers.
    public class OrderResponse
    {
        public IReadOnlyList<OrderDto> Data { get; set; } = new List<OrderDto>();

        public string? Message { get; set; }
    }
}