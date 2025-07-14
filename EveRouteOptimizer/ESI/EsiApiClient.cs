using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using EveRouteOptimizer.Models;

namespace EveRouteOptimizer.ESI
{
    public class EsiApiClient
    {
        private readonly EsiCharacter _character;
        private static readonly HttpClient Http = new();

        public EsiApiClient(EsiCharacter character)
        {
            _character = character;
        }

        public async Task<bool> SetWaypointsAsync(int characterId, List<string> systemNames, Dictionary<long, SolarSystem> systems)
        {
            try
            {
                // Обновляем access token перед вызовом
                _character.AccessToken = await EsiAuthManager.RefreshAccessTokenAsync(_character.RefreshToken);

                // Находим ID солнечных систем
                var systemIds = new List<long>();
                foreach (var name in systemNames)
                {
                    var match = systems.Values.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                        systemIds.Add(match.Id);
                }

                if (systemIds.Count == 0)
                    return false;

                // Отправляем маршруты от последней к первой
                for (int i = 0; i < systemIds.Count; i++)
                {
                    bool clearOthers = (i == 0); // только для первой системы
                    bool addToBeginning = false; // всегда добавляем в конец

                    var request = new HttpRequestMessage(HttpMethod.Post,
                        $"https://esi.evetech.net/latest/ui/autopilot/waypoint/?" +
                        $"clear_other_waypoints={clearOthers.ToString().ToLower()}&" +
                        $"add_to_beginning={addToBeginning.ToString().ToLower()}&" +
                        $"destination_id={systemIds[i]}");

                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _character.AccessToken);

                    var response = await Http.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Ошибка при установке waypoint: {response.StatusCode}");
                        return false;
                    }

                    await Task.Delay(250); // пауза между запросами
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка SetWaypointsAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<long> GetCharacterIdAsync()
        {
            var json = await GetVerifyJsonAsync();
            return json.GetProperty("CharacterID").GetInt64();
        }

        public async Task<string> GetCharacterNameAsync(long id)
        {
            var json = await GetVerifyJsonAsync();
            return json.GetProperty("CharacterName").GetString();
        }

        private async Task<JsonElement> GetVerifyJsonAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://login.eveonline.com/oauth/verify");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _character.AccessToken);

            var response = await Http.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<JsonElement>(json);
        }

        private static HttpContent CreateRequest(string token)
        {
            var request = new HttpRequestMessage();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return new StringContent(string.Empty, Encoding.UTF8, "application/json");
        }
    }
}
