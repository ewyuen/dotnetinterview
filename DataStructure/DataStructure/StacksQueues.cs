using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class StacksQueues
    {
        // Sample: Valid Parentheses (LeetCode 20)
        // Problem: Given a string containing just the characters '(', ')', '{', '}', '[' and ']',
        //           determine if the input string is valid.
        //           A string is valid if open brackets are closed by the same type and in correct order.
        // Approach: Use a stack to track opening brackets.
        //   - Push opening brackets onto the stack.
        //   - For closing brackets, check if the top of the stack matches the type.
        //   - If not, or stack is empty, the string is invalid.
        //   - At the end, stack should be empty for a valid string.
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static bool IsValid(string s)
        {
            var stack = new Stack<char>();
            foreach (var c in s)
            {
                // If opening bracket, push to stack
                if (c == '(' || c == '{' || c == '[')
                    stack.Push(c);
                else
                {
                    // If stack is empty, no matching opening bracket
                    if (stack.Count == 0) return false;
                    char top = stack.Pop();
                    // Check if closing bracket matches the opening bracket
                    if ((c == ')' && top != '(') ||
                        (c == '}' && top != '{') ||
                        (c == ']' && top != '['))
                        return false;
                }
            }
            // If stack is empty, all brackets matched
            return stack.Count == 0;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input: string of brackets
            string s = "()[]{}";
            // Output: true if valid, false otherwise
            Console.WriteLine($"Is valid: {IsValid(s)}");
        }
    }
}
