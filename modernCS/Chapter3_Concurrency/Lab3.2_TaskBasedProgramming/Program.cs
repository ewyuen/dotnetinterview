using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_2_TaskBasedProgramming
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 3.2: Task-Based Programming ===\n");

            // Demo 1: Basic Task Creation and Execution
            Console.WriteLine("1. Basic Task Creation and Execution");
            await DemonstrateBasicTasks();

            // Demo 2: Task Continuation
            Console.WriteLine("\n2. Task Continuation");
            await DemonstrateTaskContinuation();

            // Demo 3: Task with Result
            Console.WriteLine("\n3. Task with Result");
            await DemonstrateTaskWithResult();

            // Demo 4: Exception Handling in Tasks
            Console.WriteLine("\n4. Exception Handling in Tasks");
            await DemonstrateTaskExceptionHandling();

            // Demo 5: Task Cancellation
            Console.WriteLine("\n5. Task Cancellation");
            await DemonstrateTaskCancellation();

            // Demo 6: Parallel Tasks
            Console.WriteLine("\n6. Parallel Tasks");
            await DemonstrateParallelTasks();

            Console.WriteLine("\n=== Lab 3.2 Complete ===");
        }

        static async Task DemonstrateBasicTasks()
        {
            Console.WriteLine("Creating and running basic tasks...");

            // Task.Run - creates and starts a task
            Task task1 = Task.Run(() =>
            {
                Console.WriteLine($"Task 1 running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(100);
                Console.WriteLine("Task 1 completed");
            });

            Task task2 = Task.Run(() =>
            {
                Console.WriteLine($"Task 2 running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(50);
                Console.WriteLine("Task 2 completed");
            });

            // Wait for both tasks to complete
            await Task.WhenAll(task1, task2);
            Console.WriteLine("Both tasks completed");
        }

        static async Task DemonstrateTaskContinuation()
        {
            Console.WriteLine("Demonstrating task continuation...");

            Task<int> task = Task.Run(() =>
            {
                Console.WriteLine("Primary task: calculating...");
                Thread.Sleep(100);
                return 42;
            });

            // Continue with another task when the first completes
            Task continuation = task.ContinueWith(previousTask =>
            {
                int result = previousTask.Result;
                Console.WriteLine($"Continuation: received result {result}, processing...");
                Thread.Sleep(50);
                Console.WriteLine($"Continuation: final result is {result * 2}");
            });

            await continuation;
        }

        static async Task DemonstrateTaskWithResult()
        {
            Console.WriteLine("Demonstrating tasks with results...");

            // Task that returns a result
            Task<int> calculationTask = Task.Run(() =>
            {
                Console.WriteLine("Calculating factorial of 5...");
                return CalculateFactorial(5);
            });

            int result = await calculationTask;
            Console.WriteLine($"Factorial result: {result}");

            // Multiple tasks with results
            Task<int>[] tasks = new Task<int>[3];
            for (int i = 0; i < 3; i++)
            {
                int num = i + 1;
                tasks[i] = Task.Run(() => CalculateFactorial(num));
            }

            int[] results = await Task.WhenAll(tasks);
            Console.WriteLine($"Factorials: {string.Join(", ", results)}");
        }

        static int CalculateFactorial(int n)
        {
            if (n <= 1) return 1;
            return n * CalculateFactorial(n - 1);
        }

        static async Task DemonstrateTaskExceptionHandling()
        {
            Console.WriteLine("Demonstrating exception handling in tasks...");

            try
            {
                Task task = Task.Run(() =>
                {
                    Console.WriteLine("Task about to throw exception...");
                    throw new InvalidOperationException("Something went wrong in the task!");
                });

                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }

            // Handling multiple tasks with potential exceptions
            Task[] tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(() =>
                {
                    if (taskId == 1)
                        throw new ArgumentException($"Task {taskId} failed");
                    Console.WriteLine($"Task {taskId} completed successfully");
                });
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"One or more tasks failed: {ex.Message}");
            }
        }

        static async Task DemonstrateTaskCancellation()
        {
            Console.WriteLine("Demonstrating task cancellation...");

            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            // Long-running task that can be cancelled
            Task longRunningTask = Task.Run(async () =>
            {
                Console.WriteLine("Long-running task started...");
                for (int i = 0; i < 10; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine($"Working... {i + 1}/10");
                    await Task.Delay(100, token);
                }
                Console.WriteLine("Long-running task completed");
            }, token);

            // Cancel after 3 seconds
            await Task.Delay(300);
            Console.WriteLine("Requesting cancellation...");
            cts.Cancel();

            try
            {
                await longRunningTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Task failed with: {ex.Message}");
            }
        }

        static async Task DemonstrateParallelTasks()
        {
            Console.WriteLine("Demonstrating parallel task execution...");

            var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Process numbers in parallel
            Task[] processingTasks = numbers.Select(async num =>
            {
                Console.WriteLine($"Processing {num} on thread {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(50); // Simulate work
                int result = num * num;
                Console.WriteLine($"Result for {num}: {result}");
            }).ToArray();

            await Task.WhenAll(processingTasks);
            Console.WriteLine("All parallel tasks completed");
        }
    }
}