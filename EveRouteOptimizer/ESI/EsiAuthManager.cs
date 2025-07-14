using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;

namespace EveRouteOptimizer.ESI
{
    public static class EsiAuthManager
    {
        private const string ClientId = "8680edc5804c442f9ca93c156a85031f";
        private const string ClientSecret = "jSxZnHNUbAxSXoJS7gOnimdyVeHLdHwRggDp5Idj";
        private const string RedirectUri = "http://localhost:12345/callback/";
        private const string Scopes = "esi-ui.write_waypoint.v1";
        private const string AuthUrl = "https://login.eveonline.com/v2/oauth/authorize";
        private const string TokenUrl = "https://login.eveonline.com/v2/oauth/token";
        private const string VerifyUrl = "https://login.eveonline.com/oauth/verify";

        private static readonly HttpClient Http = new();

        public static string BuildLoginUrl(string state)
        {
            var uri = new UriBuilder(AuthUrl);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["response_type"] = "code";
            query["redirect_uri"] = RedirectUri;
            query["client_id"] = ClientId;
            query["scope"] = Scopes;
            query["state"] = state;
            uri.Query = query.ToString();
            return uri.ToString();
        }

        public static async Task<EsiCharacter> StartAuthenticationAsync()
        {
            string state = Guid.NewGuid().ToString("N");
            string authUrl = BuildLoginUrl(state);

            using var listener = new HttpListener();
            listener.Prefixes.Add(RedirectUri);
            listener.Start();

            Process.Start(new ProcessStartInfo
            {
                FileName = authUrl,
                UseShellExecute = true
            });

            var context = await listener.GetContextAsync();
            var request = context.Request;

            var query = HttpUtility.ParseQueryString(request.Url.Query);
            string code = query["code"];
            string returnedState = query["state"];

            // Отправим сообщение в браузер
            string responseString = "<html><body>Авторизация успешна. Можете закрыть это окно.</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();

            listener.Stop();

            if (returnedState != state)
                throw new Exception("Ошибка авторизации: неверный state");

            return await ExchangeCodeAsync(code);
        }

        public static async Task<EsiCharacter> ExchangeCodeAsync(string code)
        {
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl)
            {
                Content = new FormUrlEncodedContent(values)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}")));

            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(json);

            string accessToken = tokenData.GetProperty("access_token").GetString();
            string refreshToken = tokenData.GetProperty("refresh_token").GetString();

            var characterInfo = await VerifyTokenAsync(accessToken);

            return new EsiCharacter
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CharacterId = characterInfo.CharacterId,
                CharacterName = characterInfo.CharacterName
            };
        }

        public static async Task<(string CharacterName, int CharacterId)> VerifyTokenAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, VerifyUrl);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            string name = data.GetProperty("CharacterName").GetString();
            int id = data.GetProperty("CharacterID").GetInt32();

            return (name, id);
        }

        public static async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            var values = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl)
            {
                Content = new FormUrlEncodedContent(values)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}")));

            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(json);

            return tokenData.GetProperty("access_token").GetString();
        }
    }
}
