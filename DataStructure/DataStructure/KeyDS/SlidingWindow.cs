using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DataStructure
{
    internal class SlidingWindow
    {
        // Sample: Maximum Sum Subarray of Size K (LeetCode 643)
        // This demonstrates the Sliding Window technique.
        // Time Complexity: O(n) - each element is visited once
        // Space Complexity: O(1) - constant extra space
        public static int MaxSumSubarray(int[] nums, int k)
        {
            int maxSum = 0, windowSum = 0;
            for (int i = 0; i < k; i++)
                windowSum += nums[i];
            maxSum = windowSum;
            //•	windowSum += nums[i] adds the new element entering the window.
            //•	windowSum -= nums[i - k] removes the element leaving the window(the one that is now outside the window).
            //•	maxSum = Math.Max(maxSum, windowSum); updates the maximum sum found so far.
            for (int i = k; i < nums.Length; i++)
            {
                windowSum += nums[i] - nums[i - k];
                maxSum = Math.Max(maxSum, windowSum);
            }
            return maxSum;
        }

        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 2, 1, 5, 1, 3, 2 };
            int k = 3;
            int result = MaxSumSubarray(arr, k);
            Console.WriteLine($"Maximum sum of subarray of size {k}: {result}");
        }
    }
}
