using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class GraphAlgorithms
    {
        // Sample: Dijkstra's Shortest Path (LeetCode 743)
        // Time Complexity: O(E + N log N)
        // Space Complexity: O(N + E)
        public static int NetworkDelayTime(int[][] times, int n, int k)
        {
            var graph = new Dictionary<int, List<(int, int)>>() ;
            foreach (var t in times)
            {
                if (!graph.ContainsKey(t[0]))
                    graph[t[0]] = new List<(int, int)>();
                graph[t[0]].Add((t[1], t[2]));
            }
            var dist = new int[n + 1];
            Array.Fill(dist, int.MaxValue);
            dist[k] = 0;
            var pq = new SortedSet<(int, int)>(Comparer<(int, int)>.Create((a, b) =>
                a.Item1 == b.Item1 ? a.Item2.CompareTo(b.Item2) : a.Item1.CompareTo(b.Item1)));
            pq.Add((0, k));
            while (pq.Count > 0)
            {
                var (d, u) = pq.Min; pq.Remove(pq.Min);
                if (!graph.ContainsKey(u)) continue;
                foreach (var (v, w) in graph[u])
                {
                    if (dist[v] > d + w)
                    {
                        pq.Remove((dist[v], v));
                        dist[v] = d + w;
                        pq.Add((dist[v], v));
                    }
                }
            }
            int ans = 0;
            for (int i = 1; i <= n; i++)
                if (dist[i] == int.MaxValue) return -1;
                else ans = Math.Max(ans, dist[i]);
            return ans;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[][] times = { new[] { 2, 1, 1 }, new[] { 2, 3, 1 }, new[] { 3, 4, 1 } };
            int n = 4, k = 2;
            Console.WriteLine($"Network delay time: {NetworkDelayTime(times, n, k)}");
        }
    }
}
