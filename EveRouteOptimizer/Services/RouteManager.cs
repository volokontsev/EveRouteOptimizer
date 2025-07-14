using EveRouteOptimizer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EveRouteOptimizer.Services
{
    public class RouteManager
    {
        private List<SavedRoute> _routes = new();

        public RouteManager()
        {
            LoadRoutes();
        }

        public IReadOnlyList<SavedRoute> GetAll() => _routes;

        public SavedRoute? FindByName(string? name)
        {
            return _routes.FirstOrDefault(r => r?.Name == name);
        }

        public void AddOrReplace(SavedRoute route)
        {
            if (route == null || string.IsNullOrWhiteSpace(route.Name)) return;

            var existingIndex = _routes.FindIndex(r => r?.Name == route.Name);
            if (existingIndex >= 0)
                _routes[existingIndex] = route;
            else
                _routes.Add(route);

            SaveRoutes();
        }

        public void RemoveByName(string name)
        {
            var route = FindByName(name);
            if (route != null)
            {
                _routes.Remove(route);
                SaveRoutes();
            }
        }

        public void SaveRoutes()
        {
            RouteStorage.SaveRoutes(_routes);
        }

        public void LoadRoutes()
        {
            _routes = RouteStorage.LoadRoutes().Where(r => r != null).ToList();
        }

        public void Update(string originalName, SavedRoute updated)
        {
            if (string.IsNullOrWhiteSpace(originalName) || updated == null)
                return;

            var index = _routes.FindIndex(r => r.Name == originalName);
            if (index >= 0)
            {
                _routes[index] = updated;
                SaveRoutes();
            }
        }

        public List<object> GetViewModels()
        {
            return _routes
                .Where(r => r != null)
                .Select(r => new
                {
                    Название = r.Name,
                    Систем = r.SystemCount,
                    Прыжков = r.TotalJumps
                })
                .Cast<object>()
                .ToList();
        }
    }
}