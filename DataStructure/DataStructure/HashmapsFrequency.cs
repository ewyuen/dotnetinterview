using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class HashmapsFrequency
    {
        // Sample: Two Sum (LeetCode 1)
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static int[] TwoSum(int[] nums, int target)
        {
            var map = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                int complement = target - nums[i];
                if (map.ContainsKey(complement))
                    return new[] { map[complement], i };
                map[nums[i]] = i;
            }
            return Array.Empty<int>();
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = {2,7,11,15};
            int target = 9;
            var result = TwoSum(arr, target);
            Console.WriteLine("Indices: " + string.Join(",", result));
        }
    }
}
