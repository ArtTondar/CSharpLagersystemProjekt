namespace Lagersystem.Blazor.Models.Dtos;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string ZipCode { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string CVR { get; set; } = string.Empty;
}