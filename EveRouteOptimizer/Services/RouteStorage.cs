using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using EveRouteOptimizer.Models;

namespace EveRouteOptimizer.Services
{
    public static class RouteStorage
    {
        private const string FilePath = "saved_routes.json";

        public static List<SavedRoute> LoadRoutes()
        {
            if (!File.Exists(FilePath)) return new List<SavedRoute>();

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<SavedRoute>>(json)
                       ?? new List<SavedRoute>();
            }
            catch
            {
                return new List<SavedRoute>();
            }
        }

        public static void SaveRoutes(List<SavedRoute> routes)
        {
            try
            {
                string json = JsonSerializer.Serialize(routes, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(FilePath, json);
            }
            catch
            {
                // Log/ignore
            }
        }
    }
}
