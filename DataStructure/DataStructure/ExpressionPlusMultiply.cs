using System;

namespace DataStructure
{
    internal class ExpressionPlusMultiply
    {
        // Sample: Expression Evaluation with + and * only, no stack
        // Problem: Evaluate a string expression containing + and * (e.g., "2+3*4+5")
        // Approach:
        //   - Accumulate multiplication in a term variable.
        //   - When + is encountered, add the previous term to the result and start a new term.
        //   - At the end, add the last term to the result.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static int CalculatePlusMultiply(string s)
        {
            int result = 0, term = 1, num = 0;
            char prevOp = '+';
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    num = num * 10 + (s[i] - '0');
                }
                if (!char.IsDigit(s[i]) && s[i] != ' ' || i == s.Length - 1)
                {
                    if (prevOp == '+')
                    {
                        result += term;
                        term = num;
                    }
                    else if (prevOp == '*')
                    {
                        term *= num;
                    }
                    prevOp = s[i];
                    num = 0;
                }
            }
            result += term;
            return result;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            string expr = "2+3*4+5";
            Console.WriteLine($"Result: {CalculatePlusMultiply(expr)}"); // Output: 19
        }
    }
}
