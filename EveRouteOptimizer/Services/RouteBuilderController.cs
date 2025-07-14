using EveRouteOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class RouteBuilderController
    {
        private readonly Dictionary<long, SolarSystem> _systems;
        private readonly Dictionary<long, List<long>> _graph;
        private readonly RouteManager _routeManager;

        public RouteBuilderController(
            Dictionary<long, SolarSystem> systems,
            Dictionary<long, List<long>> graph,
            RouteManager routeManager)
        {
            _systems = systems;
            _graph = graph;
            _routeManager = routeManager;
        }

        public bool TryBuildAndAddRoute()
        {
            using var builder = new FormRouteBuilder(_systems, _graph);
            if (builder.ShowDialog() != DialogResult.OK)
                return false;

            var route = builder.Result;
            if (route != null)
            {
                _routeManager.AddOrReplace(route);
                return true;
            }

            MessageBox.Show(
                "Не удалось рассчитать маршрут. Проверьте корректность введённых систем.",
                "Ошибка маршрута",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }
    }
}
