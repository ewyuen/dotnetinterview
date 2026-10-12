using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class KthElement
    {
        // Sample: Kth Largest Element in an Array (LeetCode 215)
        // Problem: Find the kth largest element in an unsorted array.
        // Approach: Use QuickSelect algorithm (similar to QuickSort partitioning).
        //   - Partition the array so that elements greater than the pivot are on the left.
        //   - Recursively select the partition containing the kth largest until found.
        // Time Complexity: O(n) average (QuickSelect)
        // Space Complexity: O(1)
        public static int FindKthLargest(int[] nums, int k)
        {
            // QuickSelect helper function
            int QuickSelect(int left, int right, int kSmallest)
            {
                // If the list contains only one element
                if (left == right) return nums[left];
                // Select a pivotIndex
                int pivotIndex = left + (right - left) / 2;
                // Partition the array around the pivot
                pivotIndex = Partition(left, right, pivotIndex);
                // If the pivot is at the kth position, return its value
                if (kSmallest == pivotIndex) return nums[kSmallest];
                // Otherwise, recurse into the correct partition
                else if (kSmallest < pivotIndex) return QuickSelect(left, pivotIndex - 1, kSmallest);
                else return QuickSelect(pivotIndex + 1, right, kSmallest);
            }
            // Partition function: places all elements greater than pivot to the left
            int Partition(int left, int right, int pivotIndex)
            {
                int pivot = nums[pivotIndex];
                // Move pivot to end
                (nums[pivotIndex], nums[right]) = (nums[right], nums[pivotIndex]);
                int storeIndex = left;
                // Move all greater elements to the left
                for (int i = left; i < right; i++)
                {
                    if (nums[i] > pivot)
                    {
                        (nums[storeIndex], nums[i]) = (nums[i], nums[storeIndex]);
                        storeIndex++;
                    }
                }
                // Move pivot to its final place
                (nums[right], nums[storeIndex]) = (nums[storeIndex], nums[right]);
                return storeIndex;
            }
            // Start QuickSelect for kth largest (convert to kth smallest by index)
            return QuickSelect(0, nums.Length - 1, k - 1);
        }
        // Approach: Sort the array in descending order and return the kth largest element
        // Time Complexity: O(n log n) (due to sorting)
        // Space Complexity: O(1) (in-place sort)
        public static int FindKthLargestBySorting(int[] nums, int k)
        {
            Array.Sort(nums, (a, b) => b.CompareTo(a)); // Sort in descending order
            return nums[k - 1];
        }

        // Approach: Use a min-heap to extract the kth largest element
        // Time Complexity: O(n log k) (building the heap)
        // Space Complexity: O(k) (size of the heap)

        // minHeap = new PriorityQueue<int, int>();
        // minHeap.Enqueue(5, 5); // Priority = 5
        // minHeap.Enqueue(2, 2); // Priority = 2
        // minHeap.Enqueue(8, 8); // Priority = 8
        // minHeap.Peek() returns 2 (smallest priority)

        // var maxHeap = new PriorityQueue<int, int>();
        // maxHeap.Enqueue(5, -5); // Priority = -5
        // maxHeap.Enqueue(2, -2); // Priority = -2
        // maxHeap.Enqueue(8, -8); // Priority = -8
        // maxHeap.Peek() returns 8 (largest original value)

        public static int FindKthLargestWithHeap(int[] nums, int k)
        {
            // The heap always contains the k largest elements seen so far.
            // The smallest of these k elements(the root) is the kth largest overall.
            var minHeap = new PriorityQueue<int, int>();
            foreach (var num in nums)
            {
                minHeap.Enqueue(num, num);
                if (minHeap.Count > k)
                    minHeap.Dequeue();
            }
            return minHeap.Peek();
        }


        // Example usage:
        public static void ExampleUsage()
        {
            // Input array
            int[] arr = {3,2,1,5,6,4};
            int k = 2;
            // Output: kth largest element
            Console.WriteLine($"Kth largest: {FindKthLargest(arr, k)}");
        }
    }
}
