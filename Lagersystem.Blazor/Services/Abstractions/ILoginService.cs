namespace Lagersystem.Blazor.Services.Abstractions
{
    public interface ILoginService
    {
        public Task TryLoginAsync(string Email, string Password);
    }
}
