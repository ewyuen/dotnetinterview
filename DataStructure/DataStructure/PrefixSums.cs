using System;

namespace DataStructure
{
    internal class PrefixSums
    {
        // Sample: Range Sum Query - Immutable (LeetCode 303)
        // This demonstrates the Prefix Sums technique.
        // Time Complexity: O(n) for preprocessing, O(1) for each query
        // Space Complexity: O(n) for storing prefix sums
        private int[] prefix;

        public PrefixSums(int[] nums)
        {
            prefix = new int[nums.Length + 1];
            for (int i = 0; i < nums.Length; i++)
            {
                prefix[i + 1] = prefix[i] + nums[i];
            }
        }

        // Returns the sum of elements nums[left..right] (inclusive)
        public int SumRange(int left, int right)
        {
            return prefix[right + 1] - prefix[left];
        }

        // Example usage:
        // Prefix Sums allow fast range sum queries.
        // After building the prefix array, you can get the sum of any subarray [left, right] in O(1) time.
        // Example: SumRange(1, 3) returns the sum of elements at indices 1, 2, and 3.
        public static void ExampleUsage()
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            // Print the prefix array
            Console.WriteLine("inout array:");
            Console.WriteLine(string.Join(", ", arr));
            var ps = new PrefixSums(arr);
            Console.WriteLine($"Sum from index 1 to 3: {ps.SumRange(1, 3)}"); // Output: 9
            Console.WriteLine($"Sum from index 0 to 0: {ps.SumRange(0, 0)}"); // Output: 1

            // Print the prefix array
            Console.WriteLine("Prefix array:");
            Console.WriteLine(string.Join(", ", ps.prefix));
        }
    }
}
