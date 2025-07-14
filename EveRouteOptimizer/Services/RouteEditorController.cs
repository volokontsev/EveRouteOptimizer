using EveRouteOptimizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class RouteEditorController
    {
        private readonly RouteManager _routeManager;
        private readonly Dictionary<long, SolarSystem> _systems;
        private readonly Dictionary<long, List<long>> _graph;
        private readonly DataGridView _grid;

        public RouteEditorController(DataGridView grid, RouteManager routeManager, Dictionary<long, SolarSystem> systems, Dictionary<long, List<long>> graph)
        {
            _routeManager = routeManager;
            _systems = systems;
            _graph = graph;
            _grid = grid;
            _grid.CellDoubleClick += Grid_CellDoubleClick;
        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var name = _grid.Rows[e.RowIndex].Cells["Название"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(name))
                return;

            var route = _routeManager.FindByName(name);
            if (route == null)
                return;

            using var builder = new FormRouteBuilder(_systems, _graph, route);
            if (builder.ShowDialog() == DialogResult.OK)
            {
                var updated = builder.Result;
                if (updated != null)
                {
                    _routeManager.Update(name, updated);
                }
            }
        }
    }
}
