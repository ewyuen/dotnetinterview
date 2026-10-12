using System;

namespace DataStructure
{
    internal class DynamicProgramming
    {
        // Sample: Climbing Stairs (LeetCode 70)
        // Problem: You are climbing a staircase. It takes n steps to reach the top.
        //           Each time you can climb 1 or 2 steps. How many distinct ways can you climb to the top?
        // Approach: Dynamic Programming
        //   - dp[i] represents the number of ways to reach step i.
        //   - To reach step i, you can come from step i-1 (1 step) or i-2 (2 steps).
        //   - So, dp[i] = dp[i-1] + dp[i-2].
        //   - Initialize dp[1] = 1, dp[2] = 2.
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static int ClimbStairs(int n)
        {
            // Base cases: 1 or 2 steps
            if (n <= 2) return n;
            int[] dp = new int[n+1];
            dp[1] = 1; dp[2] = 2;
            // Fill dp array using the recurrence relation
            for (int i = 3; i <= n; i++)
                dp[i] = dp[i-1] + dp[i-2];
            // The answer is the number of ways to reach step n
            return dp[n];
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Input: number of stairs
            int n = 5;
            // Output: number of distinct ways to climb to the top
            Console.WriteLine($"Ways to climb {n} stairs: {ClimbStairs(n)}");
        }
    }
}
