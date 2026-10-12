using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class CompleteBinaryTreeChecker
    {
        // Problem: Check if a binary tree is a complete binary tree.
        // Definition: Every level except possibly the last is completely filled,
        // and all nodes in the last level are as far left as possible.
        // Approach: Level-order traversal (BFS). After a null child is seen,
        // no more non-null children should be found.
        // Time Complexity: O(n)
        // Space Complexity: O(n)
        public class TreeNode {
            public int val;
            public TreeNode left, right;
            public TreeNode(int x) { val = x; }
        }
        public static bool IsCompleteTree(TreeNode root)
        {
            if (root == null) return true;
            var queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            bool seenNull = false;
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node == null)
                {
                    seenNull = true;
                }
                else
                {
                    if (seenNull) return false; // Found a non-null after a null
                    queue.Enqueue(node.left);
                    queue.Enqueue(node.right);
                }
            }
            return true;
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
