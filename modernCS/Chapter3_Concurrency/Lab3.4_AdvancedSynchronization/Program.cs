using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_4_AdvancedSynchronization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 3.4: Advanced Synchronization ===\n");

            // Demo 1: Semaphore for Resource Pooling
            Console.WriteLine("1. Semaphore for Resource Pooling");
            await DemonstrateSemaphore();

            // Demo 2: SemaphoreSlim for Lightweight Synchronization
            Console.WriteLine("\n2. SemaphoreSlim for Lightweight Synchronization");
            await DemonstrateSemaphoreSlim();

            // Demo 3: Mutex for Cross-Process Synchronization
            Console.WriteLine("\n3. Mutex for Cross-Process Synchronization");
            await DemonstrateMutex();

            // Demo 4: ReaderWriterLock for Read-Heavy Scenarios
            Console.WriteLine("\n4. ReaderWriterLock for Read-Heavy Scenarios");
            await DemonstrateReaderWriterLock();

            // Demo 5: ReaderWriterLockSlim for Better Performance
            Console.WriteLine("\n5. ReaderWriterLockSlim for Better Performance");
            await DemonstrateReaderWriterLockSlim();

            // Demo 6: Combining Synchronization Primitives
            Console.WriteLine("\n6. Combining Synchronization Primitives");
            await DemonstrateCombinedSynchronization();

            Console.WriteLine("\n=== Lab 3.4 Complete ===");
        }

        static async Task DemonstrateSemaphore()
        {
            Console.WriteLine("Demonstrating Semaphore for resource pooling...");

            // Semaphore allows up to 3 concurrent accesses
            using var semaphore = new Semaphore(3, 3, "MySemaphore");

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(() => AccessResourceWithSemaphore(semaphore, taskId)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All semaphore tasks completed");
        }

        static void AccessResourceWithSemaphore(Semaphore semaphore, int taskId)
        {
            Console.WriteLine($"Task {taskId} waiting for semaphore...");
            semaphore.WaitOne();

            try
            {
                Console.WriteLine($"Task {taskId} acquired semaphore (Thread {Thread.CurrentThread.ManagedThreadId})");
                Thread.Sleep(1000); // Simulate work
                Console.WriteLine($"Task {taskId} releasing semaphore");
            }
            finally
            {
                semaphore.Release();
            }
        }

        static async Task DemonstrateSemaphoreSlim()
        {
            Console.WriteLine("Demonstrating SemaphoreSlim for lightweight synchronization...");

            using var semaphore = new SemaphoreSlim(2, 2);

            var tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(async () =>
                {
                    Console.WriteLine($"Task {taskId} waiting for SemaphoreSlim...");
                    await semaphore.WaitAsync();

                    try
                    {
                        Console.WriteLine($"Task {taskId} acquired SemaphoreSlim");
                        await Task.Delay(500);
                        Console.WriteLine($"Task {taskId} releasing SemaphoreSlim");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All SemaphoreSlim tasks completed");
        }

        static async Task DemonstrateMutex()
        {
            Console.WriteLine("Demonstrating Mutex for cross-process synchronization...");

            // Create a named mutex that can be shared across processes
            using var mutex = new Mutex(false, "Global\\MySharedMutex");

            var tasks = new List<Task>();
            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(() => AccessSharedResourceWithMutex(mutex, taskId)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All mutex tasks completed");
        }

        static void AccessSharedResourceWithMutex(Mutex mutex, int taskId)
        {
            Console.WriteLine($"Task {taskId} waiting for mutex...");
            mutex.WaitOne();

            try
            {
                Console.WriteLine($"Task {taskId} acquired mutex (exclusive access)");
                Thread.Sleep(800);
                Console.WriteLine($"Task {taskId} releasing mutex");
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static async Task DemonstrateReaderWriterLock()
        {
            Console.WriteLine("Demonstrating ReaderWriterLock for read-heavy scenarios...");

            var rwLock = new ReaderWriterLock();
            var sharedData = new SharedData(rwLock);

            var tasks = new List<Task>();

            // Multiple readers
            for (int i = 0; i < 5; i++)
            {
                int readerId = i;
                tasks.Add(Task.Run(() => sharedData.ReadData(readerId)));
            }

            // Fewer writers
            for (int i = 0; i < 2; i++)
            {
                int writerId = i;
                tasks.Add(Task.Run(() => sharedData.WriteData(writerId)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All ReaderWriterLock tasks completed");
        }

        static async Task DemonstrateReaderWriterLockSlim()
        {
            Console.WriteLine("Demonstrating ReaderWriterLockSlim for better performance...");

            var rwLock = new ReaderWriterLockSlim();
            var sharedData = new SharedDataSlim(rwLock);

            var tasks = new List<Task>();

            // Multiple readers
            for (int i = 0; i < 8; i++)
            {
                int readerId = i;
                tasks.Add(Task.Run(() => sharedData.ReadData(readerId)));
            }

            // Fewer writers
            for (int i = 0; i < 3; i++)
            {
                int writerId = i;
                tasks.Add(Task.Run(() => sharedData.WriteData(writerId)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All ReaderWriterLockSlim tasks completed");
        }

        static async Task DemonstrateCombinedSynchronization()
        {
            Console.WriteLine("Demonstrating combined synchronization primitives...");

            var resourcePool = new ResourcePool();

            var tasks = new List<Task>();

            // Producers
            for (int i = 0; i < 3; i++)
            {
                int producerId = i;
                tasks.Add(Task.Run(() => resourcePool.Produce(producerId)));
            }

            // Consumers
            for (int i = 0; i < 3; i++)
            {
                int consumerId = i;
                tasks.Add(Task.Run(() => resourcePool.Consume(consumerId)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("All combined synchronization tasks completed");
        }
    }

    // Helper class for ReaderWriterLock demonstration
    class SharedData
    {
        private readonly ReaderWriterLock _rwLock;
        private int _data;
        private readonly Random _random = new Random();

        public SharedData(ReaderWriterLock rwLock)
        {
            _rwLock = rwLock;
            _data = 0;
        }

        public void ReadData(int readerId)
        {
            _rwLock.AcquireReaderLock(Timeout.Infinite);

            try
            {
                Console.WriteLine($"Reader {readerId} reading data: {_data}");
                Thread.Sleep(_random.Next(200, 500));
            }
            finally
            {
                _rwLock.ReleaseReaderLock();
            }
        }

        public void WriteData(int writerId)
        {
            _rwLock.AcquireWriterLock(Timeout.Infinite);

            try
            {
                _data++;
                Console.WriteLine($"Writer {writerId} writing data: {_data}");
                Thread.Sleep(_random.Next(300, 700));
            }
            finally
            {
                _rwLock.ReleaseWriterLock();
            }
        }
    }

    // Helper class for ReaderWriterLockSlim demonstration
    class SharedDataSlim
    {
        private readonly ReaderWriterLockSlim _rwLock;
        private int _data;
        private readonly Random _random = new Random();

        public SharedDataSlim(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _data = 0;
        }

        public void ReadData(int readerId)
        {
            _rwLock.EnterReadLock();

            try
            {
                Console.WriteLine($"Reader {readerId} reading data: {_data}");
                Thread.Sleep(_random.Next(100, 300));
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public void WriteData(int writerId)
        {
            _rwLock.EnterWriteLock();

            try
            {
                _data++;
                Console.WriteLine($"Writer {writerId} writing data: {_data}");
                Thread.Sleep(_random.Next(200, 500));
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }

    // Helper class demonstrating combined synchronization
    class ResourcePool
    {
        private readonly SemaphoreSlim _poolSemaphore = new SemaphoreSlim(3, 3);
        private readonly ReaderWriterLockSlim _dataLock = new ReaderWriterLockSlim();
        private readonly Queue<int> _resources = new Queue<int>();
        private int _resourceCounter = 0;
        private readonly Random _random = new Random();

        public async Task Produce(int producerId)
        {
            for (int i = 0; i < 5; i++)
            {
                await _poolSemaphore.WaitAsync();

                _dataLock.EnterWriteLock();
                try
                {
                    int resource = ++_resourceCounter;
                    _resources.Enqueue(resource);
                    Console.WriteLine($"Producer {producerId} produced resource {resource} (Pool size: {_resources.Count})");
                }
                finally
                {
                    _dataLock.ExitWriteLock();
                }

                await Task.Delay(_random.Next(100, 300));
            }
        }

        public async Task Consume(int consumerId)
        {
            for (int i = 0; i < 5; i++)
            {
                _dataLock.EnterReadLock();
                int resource = -1;
                try
                {
                    if (_resources.Count > 0)
                    {
                        resource = _resources.Dequeue();
                    }
                }
                finally
                {
                    _dataLock.ExitReadLock();
                }

                if (resource != -1)
                {
                    Console.WriteLine($"Consumer {consumerId} consumed resource {resource} (Pool size: {_resources.Count})");
                    _poolSemaphore.Release();
                }
                else
                {
                    Console.WriteLine($"Consumer {consumerId} found no resources available");
                }

                await Task.Delay(_random.Next(150, 400));
            }
        }
    }
}