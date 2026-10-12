using System;

namespace DataStructure
{
    internal class MergeSortExample
    {
        // Sample: MergeSort
        // Problem: Sort an array of integers in ascending order using merge sort.
        // Approach: Recursively split the array, sort each half, and merge them.
        // Time Complexity: O(n log n)
        // Space Complexity: O(n) (temporary array for merging)
        public static void MergeSort(int[] nums, int left, int right)
        {
            if (left >= right) return;
            int mid = left + (right - left) / 2;
            MergeSort(nums, left, mid);
            MergeSort(nums, mid + 1, right);
            Merge(nums, left, mid, right);
        }
        private static void Merge(int[] nums, int left, int mid, int right)
        {
            int[] temp = new int[right - left + 1];
            int i = left, j = mid + 1, k = 0;
            while (i <= mid && j <= right)
            {
                if (nums[i] <= nums[j]) temp[k++] = nums[i++];
                else temp[k++] = nums[j++];
            }
            while (i <= mid) temp[k++] = nums[i++];
            while (j <= right) temp[k++] = nums[j++];
            for (int t = 0; t < temp.Length; t++) nums[left + t] = temp[t];
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 5, 2, 9, 1, 5, 6 };
            MergeSort(arr, 0, arr.Length - 1);
            Console.WriteLine("Mergesorted: " + string.Join(",", arr));
        }
    }
}
