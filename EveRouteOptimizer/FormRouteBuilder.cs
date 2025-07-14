using EveRouteOptimizer.Services;
using EveRouteOptimizer.Models;
using EveRouteOptimizer.ESI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EveRouteOptimizer
{
    public partial class FormRouteBuilder : Form
    {
        private readonly Dictionary<long, SolarSystem> _systems;
        private readonly Dictionary<long, List<long>> _graph;
        private readonly string _initialRouteName;

        public SavedRoute Result { get; private set; }
        public List<string> RouteSystems { get; private set; } = new();

        public FormRouteBuilder(Dictionary<long, SolarSystem> systems, Dictionary<long, List<long>> graph)
        {
            InitializeComponent();
            _systems = systems;
            _graph = graph;
        }

        public FormRouteBuilder(Dictionary<long, SolarSystem> systems, Dictionary<long, List<long>> graph, SavedRoute existingRoute)
            : this(systems, graph)
        {
            txtSystems.Text = string.Join(Environment.NewLine, existingRoute.SystemNames);
            _initialRouteName = existingRoute.Name;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            var separators = new[] { '\r', '\n', '\t', ' ' };
            var input = txtSystems.Text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (input.Count < 2)
            {
                MessageBox.Show("Введите как минимум 2 системы.");
                return;
            }

            var systemIds = new List<long>();
            foreach (var name in input)
            {
                var found = _systems.Values.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (found == null)
                {
                    MessageBox.Show($"Система не найдена: {name}");
                    return;
                }
                systemIds.Add(found.Id);
            }

            var (orderedIds, totalJumps) = RouteOptimizer.FindBestRoute(systemIds, _graph);

            if (orderedIds == null || orderedIds.Count == 0)
            {
                MessageBox.Show("Не удалось рассчитать маршрут.");
                return;
            }

            var orderedNames = orderedIds.Select(id => _systems[id].Name).ToList();

            string routeName = PromptForName(_initialRouteName);
            if (string.IsNullOrWhiteSpace(routeName)) return;

            Result = new SavedRoute
            {
                Name = routeName,
                SystemNames = orderedNames,
                TotalJumps = totalJumps
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private string PromptForName(string defaultName = null)
        {
            var dialog = new Form
            {
                Width = 400,
                Height = 150,
                Text = "Имя маршрута",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var txt = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Введите имя маршрута...", Text = defaultName ?? string.Empty };
            var btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };

            dialog.Controls.Add(txt);
            dialog.Controls.Add(btnOk);
            dialog.AcceptButton = btnOk;

            return dialog.ShowDialog(this) == DialogResult.OK ? txt.Text.Trim() : null;
        }
    }
}
