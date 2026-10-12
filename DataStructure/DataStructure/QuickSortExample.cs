using System;

namespace DataStructure
{
    internal class QuickSortExample
    {
        // Sample: QuickSort
        // Problem: Sort an array of integers in ascending order using quicksort.
        // Approach: In-place recursive quicksort with partitioning around a pivot.
        // Time Complexity: O(n log n) average, O(n^2) worst-case
        // Space Complexity: O(log n) recursion stack
        public static void QuickSort(int[] nums, int left, int right)
        {
            if (left >= right) return;
            int pivot = nums[left + (right - left) / 2];
            int i = left, j = right;
            while (i <= j)
            {
                while (nums[i] < pivot) i++;
                while (nums[j] > pivot) j--;
                if (i <= j)
                {
                    // Swap elements at indices i and j using tuple assignment.
                    // This is a concise way to swap two values in C# 7.0+.
                    // After this line, nums[i] holds the previous value of nums[j], and nums[j] holds the previous value of nums[i].
                    (nums[i], nums[j]) = (nums[j], nums[i]);
                    i++; j--;
                }
            }
            QuickSort(nums, left, j);
            QuickSort(nums, i, right);
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 5, 2, 9, 1, 5, 6 };
            QuickSort(arr, 0, arr.Length - 1);
            Console.WriteLine("Quicksorted: " + string.Join(",", arr));
        }
    }
}
