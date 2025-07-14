using System.Collections.Generic;

namespace EveRouteOptimizer.Services
{
    public class GraphBuilder
    {
        private readonly Dictionary<long, List<long>> _graph = new();
        private readonly HashSet<long> _validIds;

        public GraphBuilder(HashSet<long> validSystemIds)
        {
            _validIds = validSystemIds;
        }

        public void Build(List<(long From, long To)> jumps)
        {
            foreach (var (from, to) in jumps)
            {
                if (!_validIds.Contains(from) || !_validIds.Contains(to))
                    continue;

                if (!_graph.ContainsKey(from))
                    _graph[from] = new List<long>();
                if (!_graph[from].Contains(to))
                    _graph[from].Add(to);

                if (!_graph.ContainsKey(to))
                    _graph[to] = new List<long>();
                if (!_graph[to].Contains(from))
                    _graph[to].Add(from);
            }
        }

        public Dictionary<long, List<long>> GetGraph()
        {
            return _graph;
        }
    }
}