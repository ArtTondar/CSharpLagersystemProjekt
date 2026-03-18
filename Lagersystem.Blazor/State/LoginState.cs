using Lagersystem.Blazor.Services.Abstractions;
using Lagersystem.Blazor.Services.Api;

namespace Lagersystem.Blazor.State
{
    public class LoginState
    {
        public bool IsLoading { get; private set; }

        private readonly ILoginService _loginApiService;

        public LoginState(ILoginService loginApiService)
        {
            _loginApiService = loginApiService;
        }

        public async Task TryLoginAsync(string Username, string Password)
        {
            IsLoading = true;

            try
            {
                await _loginApiService.TryLoginAsync( Username, Password);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
