using EveRouteOptimizer.ESI;
using EveRouteOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class RouteSender
    {
        private readonly RouteManager _routeManager;
        private readonly CharacterManager _characterManager;
        private readonly Dictionary<long, SolarSystem> _systems;
        private readonly Action<string> _notify;

        public RouteSender(RouteManager routeManager, CharacterManager characterManager, Dictionary<long, SolarSystem> systems, Action<string> notify)
        {
            _routeManager = routeManager;
            _characterManager = characterManager;
            _systems = systems;
            _notify = notify;
        }

        public async Task SendByRouteNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Название маршрута не указано.");
                return;
            }

            var route = _routeManager.FindByName(name);
            if (route == null)
            {
                MessageBox.Show("Маршрут не найден. Возможно, он был удалён или не загружен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var active = _characterManager.GetActive();
            if (active is null)
            {
                MessageBox.Show("Нет активного персонажа.");
                return;
            }

            var client = new EsiApiClient(active);
            bool ok = await client.SetWaypointsAsync(active.CharacterId, route.SystemNames, _systems);
            _notify(ok ? "Маршрут установлен!" : "Ошибка установки маршрута.");
        }
    }
}
