using System.Text.Json.Serialization;

namespace EveRouteOptimizer.ESI
{
    public class EsiCharacter
    {
        [JsonPropertyName("characterId")]
        public int CharacterId { get; set; }

        [JsonPropertyName("characterName")]
        public string CharacterName { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        public override string ToString()
        {
            return CharacterName;
        }
    }
}
