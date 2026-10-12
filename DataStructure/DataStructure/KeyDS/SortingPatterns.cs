using System;
using System.Linq;

namespace DataStructure
{
    internal class SortingPatterns
    {
        // Sample: Sort Colors (LeetCode 75)
        // Problem: Given an array with values 0, 1, and 2, sort them in-place so that all 0s come first, then 1s, then 2s.
        // Approach: Dutch National Flag algorithm using three pointers (low, mid, high).
        //   - low: next position for 0
        //   - mid: current element
        //   - high: next position for 2
        //   - Swap elements to move 0s to the front and 2s to the back.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static void SortColors(int[] nums)
        {
            int low = 0, mid = 0, high = nums.Length - 1;
            // Process elements until mid passes high
            while (mid <= high)
            {
                if (nums[mid] == 0)
                {
                    // Swap 0 to the front
                    (nums[low], nums[mid]) = (nums[mid], nums[low]);
                    low++; mid++;
                }
                else if (nums[mid] == 1)
                {
                    // Leave 1 in the middle
                    mid++;
                }
                else
                {
                    // Swap 2 to the back
                    (nums[mid], nums[high]) = (nums[high], nums[mid]);
                    high--;
                }
            }
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input: unsorted array of 0s, 1s, and 2s  
            int[] arr = {2,0,2,1,1,0};
            SortColors(arr);
            // Output: sorted array
            Console.WriteLine("Sorted colors: " + string.Join(",", arr));
        }
    }
}
