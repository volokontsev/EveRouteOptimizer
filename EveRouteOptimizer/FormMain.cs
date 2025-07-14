using EveRouteOptimizer.ESI;
using EveRouteOptimizer.Models;
using EveRouteOptimizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EveRouteOptimizer
{
    public partial class FormMain : Form
    {
        private RouteManager routeManager;
        private CharacterManager characterManager;
        private RouteTableController routeTableController;
        private CharacterDropdownController characterDropdownController;
        private RouteSender routeSender;
        private RouteBuilderController routeBuilderController;
        private RouteEditorController routeEditorController;

        private Dictionary<long, SolarSystem> _systems = new();
        private Dictionary<long, List<long>> _graph = new();

        public FormMain()
        {
            InitializeComponent();
        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            characterManager = new CharacterManager();
            characterManager.Load();

            characterDropdownController = new CharacterDropdownController(cmbCharacters, pictureAvatar, characterManager);
            characterDropdownController.Refresh();

            var result = CsvLoader.LoadEmbedded();
            _systems = result.Systems;
            _graph = result.Graph;

            routeManager = new RouteManager();
            routeSender = new RouteSender(routeManager, characterManager, _systems);
            routeBuilderController = new RouteBuilderController(_systems, _graph, routeManager);
            routeTableController = new RouteTableController(dgvRoutes, routeManager, _systems, _graph);
            routeEditorController = new RouteEditorController(dgvRoutes, routeManager, _systems, _graph);
            routeEditorController.RouteUpdated += routeTableController.Refresh;

            routeTableController.Refresh();
        }

        private async void btnAddCharacter_Click(object sender, EventArgs e)
        {
            using var login = new FormOAuthLogin();
            var result = login.ShowDialog();

            if (result != DialogResult.OK || login.AccessToken == null || login.RefreshToken == null)
                return;

            var tempChar = new EsiCharacter
            {
                AccessToken = login.AccessToken,
                RefreshToken = login.RefreshToken
            };

            var client = new EsiApiClient(tempChar);
            long charIdLong = await client.GetCharacterIdAsync();
            string charName = await client.GetCharacterNameAsync(charIdLong);

            characterManager.AddOrUpdate(new EsiCharacter
            {
                CharacterId = (int)charIdLong,
                CharacterName = charName,
                AccessToken = login.AccessToken,
                RefreshToken = login.RefreshToken
            });

            characterDropdownController.Refresh();
        }

        private void btnRemoveCharacter_Click(object sender, EventArgs e)
        {
            if (cmbCharacters.SelectedItem is not EsiCharacter selected)
            {
                MessageBox.Show("Выберите персонажа для удаления.");
                return;
            }

            var confirm = MessageBox.Show($"Удалить персонажа {selected.CharacterName}?", "Удаление", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            characterManager.Remove(selected.CharacterId);
            characterDropdownController.Refresh();
        }

        private void btnNewRoute_Click(object sender, EventArgs e)
        {
            if (routeBuilderController.TryBuildAndAddRoute())
            {
                routeTableController.Refresh();
            }
        }

        private async void btnSendSelectedRoute_Click(object sender, EventArgs e)
        {
            if (dgvRoutes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите маршрут.");
                return;
            }

            var selectedRow = dgvRoutes.SelectedRows[0];
            var name = selectedRow.Cells["Название"]?.Value?.ToString();

            await routeSender.SendByRouteNameAsync(name);
        }
    }
}
