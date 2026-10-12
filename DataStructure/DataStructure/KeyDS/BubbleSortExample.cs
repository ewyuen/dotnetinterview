using System;

namespace DataStructure
{
    internal class BubbleSortExample
    {
        // Sample: Bubble Sort
        // Problem: Sort an array of integers in ascending order using bubble sort.
        // Approach: Repeatedly swap adjacent elements if they are in the wrong order.
        // Time Complexity: O(n^2) worst/average, O(n) best (already sorted)
        // Space Complexity: O(1) (in-place sort)
        public static void BubbleSort(int[] nums)
        {
            int n = nums.Length;
            bool swapped;
            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        (nums[j], nums[j + 1]) = (nums[j + 1], nums[j]);
                        swapped = true;
                    }
                }
                // If no swaps, array is sorted
                if (!swapped) break;
            }
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 5, 2, 9, 1, 5, 6 };
            BubbleSort(arr);
            Console.WriteLine("Bubblesorted: " + string.Join(",", arr));
        }
    }
}
