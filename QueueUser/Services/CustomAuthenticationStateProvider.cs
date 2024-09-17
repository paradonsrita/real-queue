using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public async Task NotifyUserAuthentication(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        return await Task.FromResult(new AuthenticationState(user));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public void SetUserAuthenticated(string citizenIdNumber)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, citizenIdNumber),
        }, "apiauth_type");

        var user = new ClaimsPrincipal(identity);
        _currentUser = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); // แจ้งเตือนการเปลี่ยนแปลงสถานะ
    }

    public void SetUserLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()); // แจ้งเตือนการเปลี่ยนแปลงสถานะ
    }
}
