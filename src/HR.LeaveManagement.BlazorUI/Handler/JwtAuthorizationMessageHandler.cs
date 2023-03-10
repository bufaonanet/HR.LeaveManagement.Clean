using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.BlazorUI.Handler;

public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public JwtAuthorizationMessageHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>("token");
        if (string.IsNullOrEmpty(token) == false)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}