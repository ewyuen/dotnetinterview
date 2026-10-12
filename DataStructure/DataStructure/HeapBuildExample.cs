using System;

namespace DataStructure
{
    internal class HeapBuildExample
    {
        // Sample: Build a Max-Heap from an array
        // Problem: Rearrange an array in-place to satisfy the max-heap property.
        // Approach: Heapify from the last non-leaf node up to the root.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static void BuildMaxHeap(int[] arr)
        {
            int n = arr.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(arr, n, i);
            }
        }
        private static void Heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && arr[left] > arr[largest])
                largest = left;
            if (right < n && arr[right] > arr[largest])
                largest = right;
            if (largest != i)
            {
                (arr[i], arr[largest]) = (arr[largest], arr[i]);
                Heapify(arr, n, largest);
            }
        }
        // Example usage:
        public static void ExampleUsage()
        {
            int[] arr = { 3, 1, 4, 1, 5, 9, 2 };
            BuildMaxHeap(arr);
            Console.WriteLine("Max-Heap: " + string.Join(",", arr));
        }

        // Sample: Build a Min-Heap from an array
        // Problem: Rearrange an array in-place to satisfy the min-heap property.
        // Approach: Heapify from the last non-leaf node up to the root.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static void BuildMinHeap(int[] arr)
        {
            int n = arr.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                MinHeapify(arr, n, i);
            }
        }
        private static void MinHeapify(int[] arr, int n, int i)
        {
            int smallest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && arr[left] < arr[smallest])
                smallest = left;
            if (right < n && arr[right] < arr[smallest])
                smallest = right;
            if (smallest != i)
            {
                (arr[i], arr[smallest]) = (arr[smallest], arr[i]);
                MinHeapify(arr, n, smallest);
            }
        }
        // Example usage for min-heap:
        public static void ExampleMinHeapUsage()
        {
            int[] arr = { 3, 1, 4, 1, 5, 9, 2 };
            BuildMinHeap(arr);
            Console.WriteLine("Min-Heap: " + string.Join(",", arr));
        }
    }
}
