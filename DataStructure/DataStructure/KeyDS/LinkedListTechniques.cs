using System;

namespace DataStructure
{
    internal class LinkedListTechniques
    {
        // Sample: Reverse Linked List (LeetCode 206)
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public class ListNode {
            public int val;
            public ListNode next;
            public ListNode(int x) { val = x; next = null; }
        }
        public static ListNode ReverseList(ListNode head)
        {
            ListNode prev = null, curr = head;
            while (curr != null)
            {
                ListNode nextTemp = curr.next;
                curr.next = prev;
                prev = curr;
                curr = nextTemp;
            }
            return prev;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            var node1 = new ListNode(1);
            var node2 = new ListNode(2);
            var node3 = new ListNode(3);
            node1.next = node2; node2.next = node3;
            var reversed = ReverseList(node1);
            Console.Write("Reversed list: ");
            while (reversed != null) { Console.Write(reversed.val + " "); reversed = reversed.next; }
            Console.WriteLine();
        }
    }
}
