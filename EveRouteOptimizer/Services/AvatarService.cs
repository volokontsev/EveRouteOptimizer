using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveRouteOptimizer.Services
{
    public static class AvatarService
    {
        public static async Task<Image?> LoadCharacterAvatarAsync(int characterId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"https://images.evetech.net/characters/{characterId}/portrait?size=64";
                var stream = await httpClient.GetStreamAsync(url);
                return Image.FromStream(stream);
            }
            catch
            {
                return null;
            }
        }
    }
}
