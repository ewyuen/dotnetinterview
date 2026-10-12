using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class Backtracking
    {
        // Sample: Subsets (LeetCode 78)
        // Problem: Given a set of distinct integers, return all possible subsets (the power set).
        // Approach: Use backtracking to explore all combinations.
        //   - At each step, decide whether to include the current element.
        //   - Recursively build subsets by adding/removing elements.
        // Time Complexity: O(2^n) - each element can be included or not
        // Space Complexity: O(2^n) - for storing all subsets
        public static IList<IList<int>> Subsets(int[] nums)
        {
            var res = new List<IList<int>>();
            // Helper function for backtracking
            void Backtrack(int start, List<int> path)
            {
                // Add current subset to result
                res.Add(new List<int>(path));
                // Try including each remaining element
                for (int i = start; i < nums.Length; i++)
                {
                    path.Add(nums[i]);         // include nums[i]
                    Backtrack(i + 1, path);   // recurse
                    path.RemoveAt(path.Count - 1); // backtrack: remove nums[i]
                }
            }
            Backtrack(0, new List<int>()); // start with empty subset
            return res;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input: array of distinct integers
            int[] arr = {1,2,3};
            var result = Subsets(arr);
            Console.WriteLine("Subsets:");
            foreach (var subset in result)
                Console.WriteLine("[" + string.Join(",", subset) + "]");
        }
    }
}
