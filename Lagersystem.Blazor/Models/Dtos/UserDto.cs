namespace Lagersystem.Blazor.Models.Dtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }
}