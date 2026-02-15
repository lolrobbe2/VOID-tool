using Google.Apis.Auth.OAuth2.Responses;
using Google.Rpc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoxholeBot.src.Discord
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : Controller
    {
        private JwtSecurityTokenHandler tokenHandler { get; init; }

        private readonly HttpClient client;
        public AuthController()
        {
            client = new HttpClient();
            tokenHandler = new JwtSecurityTokenHandler();
        }
        // Use it like this:
        // await httpClient.PostAsync("https://discord.com/api/oauth2/token", body);
        [HttpPost("Token")]
        public async Task<IActionResult> ExchangeToken([FromBody] string accessToken)
        {
            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = DiscordSDKOptions.ClientId!,
                ["client_secret"] = DiscordSDKOptions.ClientSecret!,
                ["grant_type"] = "authorization_code",
                ["code"] = accessToken
            });
            var response = await client.PostAsync("https://discord.com/api/oauth2/token", body);

            if (!response.IsSuccessStatusCode) return BadRequest();
            // Read the content as a string and return it as JSON
            TokenResponse token = await response.Content.ReadFromJsonAsync<TokenResponse>();

            var keyBytes = Encoding.UTF8.GetBytes(DiscordSDKOptions.JwtSecret);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                // Essential claims for your backend to identify the session
                new Claim("access_token", token.Token),
                new Claim("token_type", token.TokenType),
                new Claim("scope", token.Scope ?? ""),
                new Claim("refresh_token", token.RefreshToken ?? ""),
                new Claim("expires_at", DateTimeOffset.UtcNow.AddSeconds(token.Expires).ToString("o"))
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            // 3. Set Cookie Properties (Persistence, Expiration)
            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(token.Expires)
            };

            // 4. SIGN IN - This is what actually "sets" the cookie in the browser
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);
            return Ok("Cookie has been set and encrypted.");
        }
        [HttpGet("Token")]
        [Authorize] // This ensures the cookie is valid before the code runs
        public IActionResult GetTokenFromClaims()
        {
           
            string? accessToken = User.FindFirstValue("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized("Access token not found in claims.");
            }

            // 2. Return it (usually to your frontend Activity)
            return Ok(accessToken);
        }
    }
}
