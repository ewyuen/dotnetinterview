using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace DataStructure
{
    internal class CompleteBinaryTreeRecursive
    {
        // Problem: Check if a binary tree is a complete binary tree using recursion.
        // Approach: Assign indices to nodes as if the tree were a perfect binary tree.
        //   - For a complete tree with n nodes, all indices from 0 to n-1 should be filled.
        //   - Traverse the tree recursively, counting nodes and checking indices.
        // Time Complexity: O(n)
        // Space Complexity: O(h) (recursion stack, h = tree height)
        public class TreeNode {
            public int val;
            public TreeNode left, right;
            public TreeNode(int x) { val = x; }
        }
        public static bool IsCompleteTree(TreeNode root)
        {
            int nodeCount = CountNodes(root);
            return IsComplete(root, 0, nodeCount);
        }

        // Alternative simpler recursive check (not index-based)
        // No, the IsMyCompleteTree method does not correctly check for a complete binary tree according to the standard definition.
        //      Why?
        //  •	It only checks if a node has a right child without a left child, which is not allowed in a complete tree.
        //  •	It does not ensure that all nodes in the last level are as far left as possible.
        //  •	It does not check for gaps in the last level or for nodes after a missing child.
        //  Counterexample: A tree like this:
        //       1
        //      / \
        //     2   3
        //    /     \
        //   4       5
        //
        public static bool IsMyCompleteTree(TreeNode root)
        {
            if (root == null) return true;

            if (root.right != null && root.left == null) return false;

            if (root.left != null && root.right != null) { 
                return IsMyCompleteTree(root.left) && IsMyCompleteTree(root.right);
            }
            else if (root.left != null && root.right == null) {
                return IsMyCompleteTree(root.left);
            }
            else {
                return true;
            }
        }

        // Count total nodes in the tree
        private static int CountNodes(TreeNode node)
        {
            if (node == null) return 0;
            return 1 + CountNodes(node.left) + CountNodes(node.right);
        }
        // Recursively check if all indices are filled
        private static bool IsComplete(TreeNode node, int index, int nodeCount)
        {
            if (node == null) return true;
            if (index >= nodeCount) return false;
            return IsComplete(node.left, 2 * index + 1, nodeCount) &&
                   IsComplete(node.right, 2 * index + 2, nodeCount);
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Complete tree
            var root1 = new TreeNode(1) {
                left = new TreeNode(2) {
                    left = new TreeNode(4),
                    right = new TreeNode(5)
                },
                right = new TreeNode(3) {
                    left = new TreeNode(6)
                }
            };
            // Incomplete tree
            var root2 = new TreeNode(1) {
                left = new TreeNode(2),
                right = new TreeNode(3) {
                    right = new TreeNode(4)
                }
            };
            Console.WriteLine($"Is complete tree (root1): {IsCompleteTree(root1)}"); // True
            Console.WriteLine($"Is complete tree (root2): {IsCompleteTree(root2)}"); // False
        }
    }
}
