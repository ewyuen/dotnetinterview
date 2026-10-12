using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class PathSumTechniques
    {
        // Sample: Path Sum (LeetCode 112)
        // Problem: Given a binary tree and a sum, determine if the tree has a root-to-leaf path such that
        //           adding up all the values along the path equals the given sum.
        // Approach: Recursively check each node:
        //   - If the node is null, return false (no path).
        //   - If the node is a leaf, check if its value equals the remaining sum.
        //   - Otherwise, subtract the node's value from sum and recurse on left and right children.
        // Time Complexity: O(n) - visit each node once
        // Space Complexity: O(h) - recursion stack, h = tree height
        public class TreeNode {
            public int val;
            public TreeNode left, right;
            public TreeNode(int x) { val = x; }
        }
        public static bool HasPathSum(TreeNode root, int sum)
        {
            // If node is null, no path
            if (root == null) return false;
            // If leaf node, check if path sum matches
            if (root.left == null && root.right == null) return root.val == sum;
            // Recurse on left and right subtrees with updated sum
            return HasPathSum(root.left, sum - root.val) || HasPathSum(root.right, sum - root.val);
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Construct binary tree
            var root = new TreeNode(5) {
                left = new TreeNode(4) { left = new TreeNode(11) { left = new TreeNode(7), right = new TreeNode(2) } },
                right = new TreeNode(8) { left = new TreeNode(13), right = new TreeNode(4) { right = new TreeNode(1) } }
            };
            // Check if there is a path sum equal to 22
            Console.WriteLine($"Has path sum 22: {HasPathSum(root, 22)}");
        }
    }
}
