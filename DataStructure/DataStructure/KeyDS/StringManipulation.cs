using System;
using System.Text.RegularExpressions;

namespace DataStructure
{
    internal class StringManipulation
    {
        // Sample: Reverse Words in a String (LeetCode 151)
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public static string ReverseWords(string s)
        {
            var words = Regex.Split(s.Trim(), @"\s+");
            Array.Reverse(words);
            return string.Join(" ", words);
        }
        // Example usage:
        public static void ExampleUsage()
        {
            string s = "  the sky   is blue  ";
            Console.WriteLine($"Reversed words: '{ReverseWords(s)}'");
        }
    }
}
