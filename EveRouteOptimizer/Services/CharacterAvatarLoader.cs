using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class CharacterAvatarLoader
    {
        private readonly PictureBox _pictureBox;

        public CharacterAvatarLoader(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
        }

        public async Task LoadAsync(int characterId)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"https://images.evetech.net/characters/{characterId}/portrait?size=64";
                var stream = await httpClient.GetStreamAsync(url);
                _pictureBox.Image = Image.FromStream(stream);
            }
            catch (Exception)
            {
                _pictureBox.Image = null;
            }
        }
    }
}
