using System;

namespace DataStructure
{
    internal static class UniqueSortedArray
    {
        // Removes duplicates from a sorted array in-place and returns the length of unique elements.
        // After execution, the first 'length' elements of 'nums' are unique.
        public static int RemoveDuplicates(int[] nums)
        {
            if (nums.Length == 0) return 0;
            int uniqueIndex = 0;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[i] != nums[uniqueIndex])
                {
                    uniqueIndex++;
                    nums[uniqueIndex] = nums[i];
                }
            }
            return uniqueIndex + 1;
        }

        // Example usage
        public static void ExampleUsage()
        {
            int[] arr = { 1, 1, 2, 2, 3, 4, 4, 5 };
            int length = RemoveDuplicates(arr);
            Console.WriteLine($"Unique length: {length}");
            Console.WriteLine("Unique elements: " + string.Join(",", arr[..length]));
        }
    }
}
