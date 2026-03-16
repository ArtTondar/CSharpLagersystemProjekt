using Lagersystem.Blazor.Models.Dtos;

namespace Lagersystem.Blazor.Models.Responses
{
    // Placeholder:
    // Denne klasse er tænkt til fremtidige API-responses,
    // hvis API'et senere begynder at returnere wrapper-objekter
    // i stedet for rene lister eller objekter.
    public class ProductResponse
    {
        public IReadOnlyList<ProductDto> Data { get; set; } = new List<ProductDto>();

        public string? Message { get; set; }
    }
}