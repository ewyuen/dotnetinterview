using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class MonotonicStackQueue
    {
        // Sample: Next Greater Element I (LeetCode 496)
        // Problem: For each element in the array, find the next greater element to its right.
        //           If no such element exists, return -1 for that position.
        // Approach: Use a monotonic stack to keep track of indices whose next greater element hasn't been found yet.
        //   - Iterate through the array.
        //   - While the stack is not empty and the current element is greater than the element at the index on top of the stack,
        //     pop the index and set its next greater element in the result array.
        //   - Push the current index onto the stack.
        //   - At the end, any remaining indices in the stack have no greater element to their right (already set to -1).
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static int[] NextGreaterElement(int[] nums)
        {
            var stack = new Stack<int>(); // stores indices of nums
            var res = new int[nums.Length];
            Array.Fill(res, -1); // default to -1
            for (int i = 0; i < nums.Length; i++)
            {
                // While stack is not empty and current element is greater than element at stack's top index
                while (stack.Count > 0 && nums[i] > nums[stack.Peek()])
                {
                    // Set next greater element for index at top of stack
                    res[stack.Pop()] = nums[i];
                }
                // Push current index onto stack
                stack.Push(i);
            }
            // Remaining indices in stack have no greater element to their right
            return res;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input array
            int[] arr = {2,1,2,4,3};
            // Output: next greater elements for each position
            var result = NextGreaterElement(arr);
            Console.WriteLine("Next greater elements: " + string.Join(",", result));
        }
    }
}
