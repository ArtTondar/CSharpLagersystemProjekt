namespace Lagersystem.Blazor.Models.Dtos
{
    public class CurrentUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
    }
}