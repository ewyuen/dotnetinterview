using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class MergeKSortedLists
    {
        // Sample: Merge k Sorted Lists (LeetCode 23)
        // Time Complexity: O(N log k) where N is total nodes
        // Space Complexity: O(k)
        public class ListNode {
            public int val;
            public ListNode next;
            public ListNode(int x) { val = x; next = null; }
        }
        public static ListNode MergeKLists(ListNode[] lists)
        {
            var pq = new SortedSet<(int, ListNode)>(Comparer<(int, ListNode)>.Create((a, b) => a.Item1 == b.Item1 ? a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()) : a.Item1.CompareTo(b.Item1)));
            foreach (var node in lists)
                if (node != null) pq.Add((node.val, node));
            var dummy = new ListNode(0);
            var curr = dummy;
            while (pq.Count > 0)
            {
                var min = pq.Min;
                pq.Remove(min);
                curr.next = min.Item2;
                curr = curr.next;
                if (curr.next != null) pq.Add((curr.next.val, curr.next));
            }
            return dummy.next;
        }
        // Example usage:
        public static void ExampleUsage()
        {
            var l1 = new ListNode(1) { next = new ListNode(4) { next = new ListNode(5) } };
            var l2 = new ListNode(1) { next = new ListNode(3) { next = new ListNode(4) } };
            var l3 = new ListNode(2) { next = new ListNode(6) };
            var result = MergeKLists(new[] { l1, l2, l3 });
            Console.Write("Merged list: ");
            while (result != null) { Console.Write(result.val + " "); result = result.next; }
            Console.WriteLine();
        }
    }
}
