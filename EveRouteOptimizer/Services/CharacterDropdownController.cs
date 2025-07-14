using EveRouteOptimizer.ESI;
using EveRouteOptimizer.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class CharacterDropdownController
    {
        private readonly ComboBox comboBox;
        private readonly PictureBox avatar;
        private readonly CharacterManager characterManager;

        public CharacterDropdownController(ComboBox comboBox, PictureBox avatar, CharacterManager characterManager)
        {
            this.comboBox = comboBox;
            this.avatar = avatar;
            this.characterManager = characterManager;

            this.comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        }

        public void Refresh()
        {
            comboBox.DataSource = null;
            var characters = characterManager.GetAll();
            comboBox.DataSource = characters;
            comboBox.DisplayMember = "CharacterName";
            comboBox.ValueMember = "CharacterId";

            var active = characterManager.GetActive();
            if (active != null)
            {
                comboBox.SelectedItem = active;
                _ = LoadAvatarAsync(active.CharacterId);
            }

            comboBox.Enabled = characters.Count > 0;
        }

        private void ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBox.SelectedItem is EsiCharacter selected)
            {
                characterManager.SetActive(selected.CharacterId);
                _ = LoadAvatarAsync(selected.CharacterId);
            }
        }

        private async Task LoadAvatarAsync(long characterId)
        {
            try
            {
                using var client = new HttpClient();
                var stream = await client.GetStreamAsync($"https://images.evetech.net/characters/{characterId}/portrait?size=64");
                avatar.Image = System.Drawing.Image.FromStream(stream);
            }
            catch
            {
                avatar.Image = null;
            }
        }
    }
}
