using System;
using System.Collections.Generic;

namespace DataStructure
{
    // Dictionary with access history tracking for last accessed element (excluding deleted)
    internal class AccessHistoryDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dict = new();
        private readonly LinkedList<TKey> accessOrder = new();
        private readonly HashSet<TKey> deletedKeys = new();
        private LinkedListNode<TKey>? lastAccessedNode = null;

        // Put a key-value pair into the collection
        public void Put(TKey key, TValue value)
        {
            dict[key] = value;
            RecordAccess(key);
        }

        // Get the value for a key
        public TValue? Get(TKey key)
        {
            if (!dict.TryGetValue(key, out var value)) return default;
            RecordAccess(key);
            return value;
        }

        // Delete a key from the collection
        public void Delete(TKey key)
        {
            if (dict.Remove(key))
            {
                deletedKeys.Add(key);
                // If the deleted key is the last accessed, move to previous
                if (lastAccessedNode != null && EqualityComparer<TKey>.Default.Equals(lastAccessedNode.Value, key))
                {
                    MoveToPreviousValid();
                }
            }
        }

        // Get the last accessed element (excluding deleted)
        public (TKey key, TValue value)? Last()
        {
            MoveToPreviousValid();
            if (lastAccessedNode != null && dict.TryGetValue(lastAccessedNode.Value, out var value))
            {
                return (lastAccessedNode.Value, value);
            }
            return null;
        }

        // Record access in history
        private void RecordAccess(TKey key)
        {
            // Remove if already in accessOrder
            var node = accessOrder.Find(key);
            if (node != null) accessOrder.Remove(node);
            // Add to end
            lastAccessedNode = accessOrder.AddLast(key);
        }

        // Move lastAccessedNode to previous valid (not deleted) node
        private void MoveToPreviousValid()
        {
            while (lastAccessedNode != null && deletedKeys.Contains(lastAccessedNode.Value))
            {
                lastAccessedNode = lastAccessedNode.Previous;
            }
        }

        // Example usage
        public static void ExampleUsage()
        {
            var dict = new AccessHistoryDictionary<string, int>();
            dict.Put("a", 1);
            dict.Put("b", 2);
            dict.Get("a");
            dict.Put("c", 3);
            dict.Delete("c");
            var last = dict.Last();
            Console.WriteLine(last != null ? $"Last accessed: {last.Value.key}={last.Value.value}" : "No last accessed element");
        }
    }
}
