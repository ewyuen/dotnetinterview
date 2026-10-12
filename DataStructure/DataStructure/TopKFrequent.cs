using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructure
{
    internal class TopKFrequent
    {
        // Sample: Top K Frequent Elements (LeetCode 347)
        // Problem: Given an array of integers, return the k most frequent elements.
        // Approach:
        //   1. Count the frequency of each element using a hashmap.
        //   2. Use a min-heap (SortedSet) to keep track of the top k frequent elements.
        //      - If the heap exceeds size k, remove the element with the lowest frequency.
        //   3. Extract the elements from the heap for the result.
        // Time Complexity: O(n log k) - n for counting, log k for heap operations
        // Space Complexity: O(n) - for frequency map and heap
        public static int[] TopKFrequentElements(int[] nums, int k)
        {
            var freq = new Dictionary<int, int>(); // Step 1: Count frequencies
            foreach (var num in nums)
                freq[num] = freq.GetValueOrDefault(num) + 1;
            // Step 2: Min-heap to keep top k frequent elements
            var heap = new SortedSet<(int freq, int num)>(Comparer<(int freq, int num)>.Create(
                (a, b) => a.freq == b.freq ? a.num.CompareTo(b.num) : a.freq.CompareTo(b.freq)));
            foreach (var kv in freq)
            {
                heap.Add((kv.Value, kv.Key)); // Add frequency and number
                if (heap.Count > k) heap.Remove(heap.Min); // Remove lowest frequency if heap too big
            }
            // Step 3: Extract numbers from heap
            return heap.Select(x => x.num).ToArray();
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input array and k
            int[] arr = {1,1,1,2,2,3};
            int k = 2;
            // Output: top k frequent elements
            var result = TopKFrequentElements(arr, k);
            Console.WriteLine("Top K frequent: " + string.Join(",", result));
        }
    }
}
