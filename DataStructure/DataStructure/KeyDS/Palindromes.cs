using System;

namespace DataStructure
{
    internal class Palindromes
    {
        // Sample: Valid Palindrome (LeetCode 125)
        // Problem: Given a string, determine if it is a palindrome, considering only alphanumeric characters and ignoring cases.
        // Approach:
        //   - Use two pointers (left and right) to compare characters from both ends.
        //   - Skip non-alphanumeric characters and compare lowercased values.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public static bool IsPalindrome(string s)
        {
            int left = 0, right = s.Length - 1;
            while (left < right)
            {
                while (left < right && !char.IsLetterOrDigit(s[left])) left++;
                while (left < right && !char.IsLetterOrDigit(s[right])) right--;
                if (char.ToLower(s[left]) != char.ToLower(s[right])) return false;
                left++; right--;
            }
            return true;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            string s1 = "A man, a plan, a canal: Panama";
            string s2 = "race a car";
            Console.WriteLine($"Is palindrome: {IsPalindrome(s1)}"); // True
            Console.WriteLine($"Is palindrome: {IsPalindrome(s2)}"); // False
        }
    }
}
