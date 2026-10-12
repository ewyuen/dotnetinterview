using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{

    internal class TwoPointers
    {
        // Sample: Remove Duplicates from Sorted Array using Two Pointers
        // Time Complexity: O(n) - each element is visited once
        // Space Complexity: O(1) - in-place modification, no extra space used
        public static int RemoveDuplicates(int[] nums)
        {
            if (nums.Length == 0) return 0;
            int left = 0;
            for (int right = 1; right < nums.Length; right++)
            {
                if (nums[right] != nums[left])
                {
                    left++;
                    nums[left] = nums[right];
                }
            }
            return left + 1;
        }

        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 1, 1, 2, 2, 3 };
            int length = RemoveDuplicates(arr);
            Console.WriteLine($"Length after removing duplicates: {length}");
            for (int i = 0; i < length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
