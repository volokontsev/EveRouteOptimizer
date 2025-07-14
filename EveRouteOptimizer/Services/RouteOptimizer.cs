using System;
using System.Collections.Generic;
using System.Linq;

namespace EveRouteOptimizer.Services
{
    public class RouteOptimizer
    {
        public static int GetDistance(long from, long to, Dictionary<long, List<long>> graph)
        {
            if (from == to) return 0;
            if (!graph.ContainsKey(from) || !graph.ContainsKey(to)) return int.MaxValue;

            var visited = new HashSet<long>();
            var queue = new Queue<(long Node, int Depth)>();
            queue.Enqueue((from, 0));
            visited.Add(from);

            while (queue.Count > 0)
            {
                var (node, depth) = queue.Dequeue();
                if (node == to)
                    return depth;

                foreach (var neighbor in graph[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue((neighbor, depth + 1));
                    }
                }
            }

            return int.MaxValue;
        }

        public static List<long> GetOptimizedRoute(List<long> systemIds, Dictionary<long, List<long>> graph)
        {
            if (systemIds.Count < 2)
                return new List<long>(systemIds);

            var distances = new Dictionary<(long, long), int>();
            foreach (var a in systemIds)
                foreach (var b in systemIds)
                    if (a != b && !distances.ContainsKey((a, b)))
                    {
                        int dist = GetDistance(a, b, graph);
                        distances[(a, b)] = dist;
                        distances[(b, a)] = dist;
                    }

            var route = new List<long>();
            var unvisited = new HashSet<long>(systemIds);
            long current = systemIds.First();
            route.Add(current);
            unvisited.Remove(current);

            while (unvisited.Count > 0)
            {
                long next = unvisited.OrderBy(x => distances[(current, x)]).First();
                route.Add(next);
                unvisited.Remove(next);
                current = next;
            }

            // 2-opt
            bool improved;
            do
            {
                improved = false;
                for (int i = 1; i < route.Count - 2; i++)
                {
                    for (int j = i + 1; j < route.Count - 1; j++)
                    {
                        long A = route[i - 1];
                        long B = route[i];
                        long C = route[j];
                        long D = route[j + 1];

                        int oldDist = distances[(A, B)] + distances[(C, D)];
                        int newDist = distances[(A, C)] + distances[(B, D)];

                        if (newDist < oldDist)
                        {
                            route.Reverse(i, j - i + 1);
                            improved = true;
                        }
                    }
                }
            } while (improved);

            return route;
        }

        public static (List<long> OrderedIds, int TotalJumps) FindBestRoute(List<long> systemIds, Dictionary<long, List<long>> graph)
        {
            var route = GetOptimizedRoute(systemIds, graph);
            int totalJumps = 0;

            for (int i = 0; i < route.Count - 1; i++)
                totalJumps += GetDistance(route[i], route[i + 1], graph);

            return (route, totalJumps);
        }
    }
}
