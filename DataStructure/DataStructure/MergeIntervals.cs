using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class MergeIntervals
    {
        // Sample: Merge Intervals (LeetCode 56)
        // Problem: Given a collection of intervals, merge all overlapping intervals.
        // Example: Input: [[1,3],[2,6],[8,10],[15,18]]
        //          Output: [[1,6],[8,10],[15,18]]
        // Approach:
        //   1. Sort intervals by their start value.
        //   2. Iterate through each interval:
        //      - If the current interval does not overlap with the last merged interval, add it.
        //      - If it overlaps, merge by updating the end of the last interval.
        // Time Complexity: O(n log n) - sorting intervals
        // Space Complexity: O(n) - for output list
        public static int[][] Merge(int[][] intervals)
        {
            // Step 1: Sort intervals by start time
            Array.Sort(intervals, (a, b) => a[0] - b[0]);
            var merged = new List<int[]>();
            // Step 2: Iterate and merge intervals
            foreach (var interval in intervals)
            {
                // If merged is empty or no overlap, add interval
                if (merged.Count == 0 || merged[^1][1] < interval[0])
                    merged.Add(interval);
                else
                    // Overlap: merge by updating the end of the last interval
                    merged[^1][1] = Math.Max(merged[^1][1], interval[1]);
            }
            return merged.ToArray();
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input intervals
            int[][] intervals = { new[] {1,3}, new[] {2,6}, new[] {8,10}, new[] {15,18} };
            var result = Merge(intervals);
            Console.WriteLine("Merged intervals:");
            foreach (var interval in result)
                Console.WriteLine($"[{interval[0]}, {interval[1]}]");
        }
    }
}
