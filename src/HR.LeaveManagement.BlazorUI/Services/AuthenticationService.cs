using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Providers;
using HR.LeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace HR.LeaveManagement.BlazorUI.Services;

public class AuthenticationService : BaseHttpService, IAuthenticationService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(
        IClient client,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider) : base(client, localStorageService)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        try
        {
            AuthRequest authRequest = new AuthRequest() { Email = email, Password = password };

            var authResponse = await _client.LoginAsync(authRequest);
            if (authResponse.Token != string.Empty)
            {
                await _localStorage.SetItemAsync("token", authResponse.Token);

                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
    }

    public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
    {
        RegistrationRequest registrationRequest = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = userName,
            Password = password
        };

        var response = await _client.RegisterAsync(registrationRequest);
        if (string.IsNullOrEmpty(response.Id))
        {
            return true;
        }
        return false;
    }
}
