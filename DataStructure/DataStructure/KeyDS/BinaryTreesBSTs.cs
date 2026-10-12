using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class BinaryTreesBSTs
    {
        // Sample: Inorder Traversal (LeetCode 94)
        // Sample: Preorder Traversal (LeetCode 144)
        // Sample: Postorder Traversal (LeetCode 145)
        // Time Complexity: O(n) for all traversals
        // Space Complexity: O(n) for all traversals
        public class TreeNode {
            public int val;
            public TreeNode left, right;
            public TreeNode(int x) { val = x; }
        }
        // Inorder Traversal: left, root, right
        public static IList<int> InorderTraversal(TreeNode root)
        {
            var res = new List<int>();
            void Traverse(TreeNode node)
            {
                if (node == null) return;
                Traverse(node.left);
                res.Add(node.val);
                Traverse(node.right);
            }
            Traverse(root);
            return res;
        }
        // Preorder Traversal: root, left, right
        public static IList<int> PreorderTraversal(TreeNode root)
        {
            var res = new List<int>();
            void Traverse(TreeNode node)
            {
                if (node == null) return;
                res.Add(node.val);
                Traverse(node.left);
                Traverse(node.right);
            }
            Traverse(root);
            return res;
        }
        // Postorder Traversal: left, right, root
        public static IList<int> PostorderTraversal(TreeNode root)
        {
            var res = new List<int>();
            void Traverse(TreeNode node)
            {
                if (node == null) return;
                Traverse(node.left);
                Traverse(node.right);
                res.Add(node.val);
            }
            Traverse(root);
            return res;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            var root = new TreeNode(1) { right = new TreeNode(2) { left = new TreeNode(3) } };
            var inorder = InorderTraversal(root);
            var preorder = PreorderTraversal(root);
            var postorder = PostorderTraversal(root);
            Console.WriteLine("Inorder traversal: " + string.Join(",", inorder));
            Console.WriteLine("Preorder traversal: " + string.Join(",", preorder));
            Console.WriteLine("Postorder traversal: " + string.Join(",", postorder));
        }
    }
}
