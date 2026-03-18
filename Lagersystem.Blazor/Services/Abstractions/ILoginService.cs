using Lagersystem.Blazor.Models.Dtos;

namespace Lagersystem.Blazor.Services.Abstractions
{
    public interface ILoginService
    {
        Task<CurrentUserDto?> TryLoginAsync(string email, string password);
        Task<CurrentUserDto?> GetCurrentUserAsync();
        Task LogoutAsync();
    }
}