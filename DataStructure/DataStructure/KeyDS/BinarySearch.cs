using System;

namespace DataStructure
{
    internal class BinarySearch
    {
        // Sample: Binary Search (LeetCode 704)
        // Time Complexity: O(log n)
        // Space Complexity: O(1)
        public static int Search(int[] nums, int target)
        {
            int left = 0, right = nums.Length - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (nums[mid] == target) return mid;
                if (nums[mid] < target) left = mid + 1;
                else right = mid - 1;
            }
            return -1;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { -1, 0, 3, 5, 9, 12 };
            int target = 9;
            int idx = Search(arr, target);
            Console.WriteLine($"Index of {target}: {idx}");
        }
    }
}
