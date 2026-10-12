using System;
using System.Collections.Generic;

namespace DataStructure
{
    internal class DesignProblems
    {
        // Sample: LRU Cache (LeetCode 146)
        // Problem: Design a data structure that supports get and put operations in O(1) time,
        //           and evicts the least recently used item when capacity is exceeded.
        // Approach:
        //   - Use a dictionary (map) for O(1) access to cache items.
        //   - Use a doubly linked list to track usage order (most recently used at front).
        //   - On get/put, move accessed/updated node to the front.
        //   - On put, if capacity is exceeded, remove the least recently used node from the end.
        // Time Complexity: O(1) for get/put
        // Space Complexity: O(capacity)
        public class LRUCache {
            private readonly int capacity;
            private readonly Dictionary<int, LinkedListNode<(int key, int value)>> map;
            private readonly LinkedList<(int key, int value)> list;
            public LRUCache(int capacity)
            {
                this.capacity = capacity;
                map = new Dictionary<int, LinkedListNode<(int, int)>>();
                list = new LinkedList<(int, int)>();
            }
            // Get the value for the key if present, else return -1
            public int Get(int key)
            {
                if (!map.ContainsKey(key)) return -1;
                var node = map[key];
                // Move accessed node to the front (most recently used)
                list.Remove(node);
                list.AddFirst(node);
                return node.Value.value;
            }
            // Put key-value pair in cache
            public void Put(int key, int value)
            {
                if (map.ContainsKey(key))
                {
                    // Remove old node if key exists
                    list.Remove(map[key]);
                }
                else if (map.Count == capacity)
                {
                    // Remove least recently used node if at capacity
                    var last = list.Last;
                    map.Remove(last.Value.key);
                    list.RemoveLast();
                }
                // Add new node to the front
                var newNode = new LinkedListNode<(int, int)>((key, value));
                list.AddFirst(newNode);
                map[key] = newNode;
            }
        }
        // Alternative: LRU Cache using access count
        // Problem: Instead of tracking usage order with a list, keep a count of accesses for each key.
        // Approach:
        //   - Use a dictionary for key-value pairs.
        //   - Use a dictionary for key-access count.
        //   - On get/put, increment the access count for the key.
        //   - On put, if capacity is exceeded, evict the key with the lowest access count.
        // Time Complexity: O(n) for put (eviction requires scanning all keys), O(1) for get
        // Space Complexity: O(capacity)
        public class LRUCacheWithCount {
            private readonly int capacity;
            private readonly Dictionary<int, int> map; // key-value
            private readonly Dictionary<int, int> accessCount; // key-access count
            public LRUCacheWithCount(int capacity)
            {
                this.capacity = capacity;
                map = new Dictionary<int, int>();
                accessCount = new Dictionary<int, int>();
            }
            public int Get(int key)
            {
                if (!map.ContainsKey(key)) return -1;
                accessCount[key]++;
                return map[key];
            }
            public void Put(int key, int value)
            {
                if (map.ContainsKey(key))
                {
                    map[key] = value;
                    accessCount[key]++;
                }
                else
                {
                    if (map.Count == capacity)
                    {
                        // Find key with minimum access count
                        int minCount = int.MaxValue;
                        int lruKey = -1;
                        foreach (var kv in accessCount)
                        {
                            if (kv.Value < minCount)
                            {
                                minCount = kv.Value;
                                lruKey = kv.Key;
                            }
                        }
                        map.Remove(lruKey);
                        accessCount.Remove(lruKey);
                    }
                    map[key] = value;
                    accessCount[key] = 1;
                }
            }
        }
        // Example usage:
        public static void ExampleUsage()
        {
            // Create LRU cache with capacity 2
            var cache = new LRUCache(2);
            cache.Put(1, 1);
            cache.Put(2, 2);
            Console.WriteLine(cache.Get(1)); // returns 1
            cache.Put(3, 3); // evicts key 2
            Console.WriteLine(cache.Get(2)); // returns -1
            cache.Put(4, 4); // evicts key 1
            Console.WriteLine(cache.Get(1)); // returns -1
            Console.WriteLine(cache.Get(3)); // returns 3
            Console.WriteLine(cache.Get(4)); // returns 4
        }
    }
}
