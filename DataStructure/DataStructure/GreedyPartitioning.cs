using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class GreedyPartitioning
    {
        // Sample: Partition Labels (LeetCode 763)
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static IList<int> PartitionLabels(string s)
        {
            var last = new Dictionary<char, int>();
            for (int i = 0; i < s.Length; i++) last[s[i]] = i;
            var res = new List<int>();
            int start = 0, end = 0;
            for (int i = 0; i < s.Length; i++)
            {
                end = Math.Max(end, last[s[i]]);
                if (i == end)
                {
                    res.Add(end - start + 1);
                    start = i + 1;
                }
            }
            return res;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            string s = "ababcbacadefegdehijhklij";
            var result = PartitionLabels(s);
            Console.WriteLine("Partition sizes: " + string.Join(",", result));
        }
    }
}
