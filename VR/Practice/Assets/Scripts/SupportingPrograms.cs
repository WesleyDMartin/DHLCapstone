using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class SupportingPrograms
    {
        static Task<int> RunProcessAsync(string command, string args, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<int>();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = command;
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            var registration = token.Register(() =>
            {
                process.Kill();
            });

            process.Exited += (sender, arguments) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
                registration.Dispose();
            };


            process.Start();

            return tcs.Task;
        }


        public static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // Store references to the tasks so that we can wait on them and
            // observe their status after cancellation.
            Task t1, t2;
            var tasks = new ConcurrentBag<Task>();



            // Request cancellation of a single task when the token source is canceled.
            // Pass the token to the user delegate, and also to the task so it can
            // handle the exception correctly.
            t1 = Task.Run(() => RunProcessAsync("python", "PythonTest/service.py", token));
            t2 = Task.Run(() => RunProcessAsync(@".\SpeechRecognizer\SpeechRecognizer\bin\Debug\SpeechRecognizer.exe", "google", token));
            Console.WriteLine("Task {0} executing", t1.Id);
            Console.WriteLine("Task {0} executing", t2.Id);
            tasks.Add(t1);
            tasks.Add(t2);

            // Request cancellation from the UI thread. 
            char ch = Console.ReadKey().KeyChar;
            if (ch == 'c' || ch == 'C')
            {
                tokenSource.Cancel();
                Console.WriteLine("\nTask cancellation requested.");

                // Optional: Observe the change in the Status property on the task. 
                // It is not necessary to wait on tasks that have canceled. However, 
                // if you do wait, you must enclose the call in a try-catch block to 
                // catch the TaskCanceledExceptions that are thrown. If you do  
                // not wait, no exception is thrown if the token that was passed to the  
                // Task.Run method is the same token that requested the cancellation.
            }

            try
            {
                await Task.WhenAll(tasks.ToArray());
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
            }
            finally
            {
                tokenSource.Dispose();
            }

            // Display status of all tasks. 
            foreach (var task in tasks)
                Console.WriteLine("Task {0} status is now {1}", task.Id, task.Status);
        }
    }

}

