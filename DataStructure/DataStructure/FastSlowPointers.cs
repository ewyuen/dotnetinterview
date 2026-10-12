using System;

namespace DataStructure
{
    internal class FastSlowPointers
    {
        // Sample: Linked List Cycle (LeetCode 141)
        // Problem: Given a linked list, determine if it has a cycle in it.
        // Approach: Use two pointers (slow and fast). Move slow by one step and fast by two steps.
        //   - If there is a cycle, fast and slow will eventually meet.
        //   - If fast reaches the end (null), there is no cycle.
        // Time Complexity: O(n)
        // Space Complexity: O(1)
        public class ListNode {
            public int val;
            public ListNode next;
            public ListNode(int x) { val = x; next = null; }
        }
        public static bool HasCycle(ListNode head)
        {
            var slow = head; // moves one step at a time
            var fast = head; // moves two steps at a time
            while (fast != null && fast.next != null)
            {
                slow = slow.next;         // move slow by one
                fast = fast.next.next;    // move fast by two
                if (slow == fast) return true; // pointers meet: cycle detected
            }
            return false; // fast reached end: no cycle
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Create a cycle: 3 -> 2 -> 0 -> -4 -> (points back to 2)
            var node1 = new ListNode(3);
            var node2 = new ListNode(2);
            var node3 = new ListNode(0);
            var node4 = new ListNode(-4);
            node1.next = node2; node2.next = node3; node3.next = node4; node4.next = node2; // cycle
            Console.WriteLine($"Has cycle: {HasCycle(node1)}");
        }
    }
}
