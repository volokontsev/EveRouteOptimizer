using EveRouteOptimizer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EveRouteOptimizer.Services
{
    public class RouteTableController
    {
        private readonly DataGridView _grid;
        private readonly RouteManager _routeManager;
        private readonly Dictionary<long, SolarSystem> _systems;
        private readonly Dictionary<long, List<long>> _graph;

        public RouteTableController(DataGridView grid, RouteManager routeManager, Dictionary<long, SolarSystem> systems, Dictionary<long, List<long>> graph)
        {
            _grid = grid;
            _routeManager = routeManager;
            _systems = systems;
            _graph = graph;
            _grid.KeyDown += Grid_KeyDown;
        }

        public void Refresh()
        {
            _grid.DataSource = null;
            _grid.DataSource = _routeManager.GetViewModels();
        }

        private void Grid_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && _grid.SelectedRows.Count > 0)
            {
                e.Handled = true;

                var selectedRow = _grid.SelectedRows[0];
                var name = selectedRow.Cells["Название"]?.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    _routeManager.RemoveByName(name);
                    _grid.ClearSelection();
                    Refresh();
                }
            }
        }
    }
}
