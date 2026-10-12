// Lab 3.1: Basic Threading
using System.Diagnostics;

Console.WriteLine("=== Lab 3.1: Basic Threading ===");

// 1. Creating and starting threads
Console.WriteLine("\n1. Creating and Starting Threads:");
Thread thread1 = new Thread(WorkerMethod);
Thread thread2 = new Thread(WorkerMethod);
Thread thread3 = new Thread(() => Console.WriteLine("Hello from lambda thread!"));

thread1.Name = "Worker-1";
thread2.Name = "Worker-2";
thread3.Name = "Lambda-Thread";

thread1.Start("Task A");
thread2.Start("Task B");
thread3.Start();

// Wait for threads to complete
thread1.Join();
thread2.Join();
thread3.Join();

Console.WriteLine("All threads completed.");

// 2. Thread synchronization and race conditions
Console.WriteLine("\n2. Thread Synchronization (Race Condition Demo):");
Counter counter = new Counter();

Thread[] threads = new Thread[5];
for (int i = 0; i < threads.Length; i++)
{
    threads[i] = new Thread(() => counter.IncrementUnsafe());
    threads[i].Name = $"Counter-{i}";
}

Console.WriteLine("Starting unsafe counter threads...");
Stopwatch sw = Stopwatch.StartNew();
foreach (var thread in threads)
{
    thread.Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

Console.WriteLine($"Unsafe counter result: {counter.UnsafeCount} (Expected: {threads.Length * 1000})");
Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds}ms");

// 3. Thread-safe counter
Console.WriteLine("\n3. Thread-Safe Counter:");
CounterSafe safeCounter = new CounterSafe();

threads = new Thread[5];
for (int i = 0; i < threads.Length; i++)
{
    threads[i] = new Thread(() => safeCounter.IncrementSafe());
    threads[i].Name = $"SafeCounter-{i}";
}

Console.WriteLine("Starting safe counter threads...");
sw.Restart();
foreach (var thread in threads)
{
    thread.Start();
}

foreach (var thread in threads)
{
    thread.Join();
}

Console.WriteLine($"Safe counter result: {safeCounter.SafeCount} (Expected: {threads.Length * 1000})");
Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds}ms");

// 4. Thread communication with Monitor
Console.WriteLine("\n4. Thread Communication (Producer-Consumer):");
BoundedBuffer buffer = new BoundedBuffer(3);

// Producer thread
Thread producer = new Thread(() =>
{
    for (int i = 1; i <= 5; i++)
    {
        buffer.Put($"Item {i}");
        Thread.Sleep(100); // Simulate work
    }
    buffer.Put("DONE"); // Signal completion
});
producer.Name = "Producer";

// Consumer thread
Thread consumer = new Thread(() =>
{
    string item;
    while ((item = buffer.Take()) != "DONE")
    {
        Console.WriteLine($"Consumed: {item}");
        Thread.Sleep(150); // Simulate processing
    }
    Console.WriteLine("Consumer finished.");
});
consumer.Name = "Consumer";

producer.Start();
consumer.Start();

producer.Join();
consumer.Join();

// 5. Thread priority and background threads
Console.WriteLine("\n5. Thread Priority and Background Threads:");

// Background thread (won't prevent app exit)
Thread backgroundThread = new Thread(() =>
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Background work: {i}");
        Thread.Sleep(200);
    }
});
backgroundThread.IsBackground = true;
backgroundThread.Name = "Background";
backgroundThread.Start();

// Foreground thread with different priority
Thread priorityThread = new Thread(() =>
{
    Console.WriteLine($"Thread priority: {Thread.CurrentThread.Priority}");
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"Priority work: {i}");
        Thread.Sleep(100);
    }
});
priorityThread.Priority = ThreadPriority.Highest;
priorityThread.Name = "HighPriority";
priorityThread.Start();

// Wait for foreground thread
priorityThread.Join();
Console.WriteLine("Foreground thread completed. Background thread may still be running...");

// Give background thread a moment to finish
Thread.Sleep(500);

static void WorkerMethod(object? data)
{
    string taskName = data?.ToString() ?? "Unknown";
    Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' processing {taskName}");

    // Simulate work
    Thread.Sleep(500);

    Console.WriteLine($"Thread '{Thread.CurrentThread.Name}' completed {taskName}");
}

class Counter
{
    public int UnsafeCount { get; private set; }

    public void IncrementUnsafe()
    {
        for (int i = 0; i < 1000; i++)
        {
            UnsafeCount++; // Race condition!
        }
    }
}

class CounterSafe
{
    private readonly object _lock = new object();
    public int SafeCount { get; private set; }

    public void IncrementSafe()
    {
        for (int i = 0; i < 1000; i++)
        {
            lock (_lock)
            {
                SafeCount++;
            }
        }
    }
}

class BoundedBuffer
{
    private readonly Queue<string> _queue = new Queue<string>();
    private readonly int _capacity;
    private readonly object _lock = new object();

    public BoundedBuffer(int capacity)
    {
        _capacity = capacity;
    }

    public void Put(string item)
    {
        lock (_lock)
        {
            while (_queue.Count >= _capacity)
            {
                Monitor.Wait(_lock);
            }
            _queue.Enqueue(item);
            Console.WriteLine($"Produced: {item} (Queue size: {_queue.Count})");
            Monitor.Pulse(_lock);
        }
    }

    public string Take()
    {
        lock (_lock)
        {
            while (_queue.Count == 0)
            {
                Monitor.Wait(_lock);
            }
            string item = _queue.Dequeue();
            Monitor.Pulse(_lock);
            return item;
        }
    }
}
