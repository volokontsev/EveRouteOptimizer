using System.Collections.Generic;

namespace EveRouteOptimizer.Models
{
    public class SavedRoute
    {
        public string Name { get; set; } = "";
        public List<string> SystemNames { get; set; } = new();
        public int TotalJumps { get; set; }

        public int SystemCount => SystemNames?.Count ?? 0;
    }
}
