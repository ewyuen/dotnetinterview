using System;

namespace DataStructure
{
    internal class DivideAndConquer
    {
        // Sample: Maximum Subarray (LeetCode 53)
        // Problem: Find the contiguous subarray with the largest sum.
        // Approach: Divide and Conquer
        //   - Recursively split the array into two halves.
        //   - The maximum subarray is either:
        //     1. Entirely in the left half
        //     2. Entirely in the right half
        //     3. Crossing the midpoint (includes elements from both halves)
        //   - Compute all three and return the maximum.
        // Time Complexity: O(n log n)
        // Space Complexity: O(log n) for recursion stack
        public static int MaxSubArray(int[] nums)
        {
            // Start recursion on the whole array
            return Helper(nums, 0, nums.Length - 1);
        }

        // Recursively find the maximum subarray sum in nums[left..right]
        private static int Helper(int[] nums, int left, int right)
        {
            // Base case: only one element
            if (left == right) return nums[left];

            int mid = left + (right - left) / 2;

            // Find max subarray sum in left half
            int leftSum = Helper(nums, left, mid);
            // Find max subarray sum in right half
            int rightSum = Helper(nums, mid + 1, right);
            // Find max subarray sum crossing the midpoint
            int crossSum = CrossSum(nums, left, right, mid);

            // Return the largest of the three
            return Math.Max(Math.Max(leftSum, rightSum), crossSum);
        }

        // Find the maximum subarray sum that crosses the midpoint
        private static int CrossSum(int[] nums, int left, int right, int mid)
        {
            // Find max sum starting from mid and expanding left
            int leftSubsum = int.MinValue, curr = 0;
            for (int i = mid; i >= left; i--)
            {
                curr += nums[i];
                leftSubsum = Math.Max(leftSubsum, curr);
            }
            // Find max sum starting from mid+1 and expanding right
            int rightSubsum = int.MinValue; curr = 0;
            for (int i = mid + 1; i <= right; i++)
            {
                curr += nums[i];
                rightSubsum = Math.Max(rightSubsum, curr);
            }
            // Combine both sides for the crossing sum
            return leftSubsum + rightSubsum;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input array
            int[] arr = {-2,1,-3,4,-1,2,1,-5,4};
            // Find maximum subarray sum
            int result = MaxSubArray(arr);
            Console.WriteLine($"Maximum subarray sum: {result}");
        }
    }
}
