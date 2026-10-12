using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class ExpressionEvaluation
    {
        // Sample: Basic Calculator II (LeetCode 227)
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static int Calculate(string s)
        {
            var stack = new Stack<int>();
            int num = 0;
            char sign = '+';
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                    num = num * 10 + (s[i] - '0');
                if (!char.IsDigit(s[i]) && s[i] != ' ' || i == s.Length - 1)
                {
                    if (sign == '+') stack.Push(num);
                    else if (sign == '-') stack.Push(-num);
                    else if (sign == '*') stack.Push(stack.Pop() * num);
                    else if (sign == '/') stack.Push(stack.Pop() / num);
                    sign = s[i];
                    num = 0;
                }
            }
            int result = 0;
            while (stack.Count > 0) result += stack.Pop();
            return result;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            string expr = "3+2*2";
            Console.WriteLine($"Result: {Calculate(expr)}");
        }
    }
}
