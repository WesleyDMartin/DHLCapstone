using System.Collections;
using UnityEngine;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class ServiceHandler : MonoBehaviour
{
    private CancellationTokenSource tokenSource;
    private CancellationToken token;

    public event EventHandler ServicesReady;

    // Store references to the tasks so that we can wait on them and
    // observe their status after cancellation.
    private Task QuestionMatcher, SpeechRecognizer;
    private ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
    private static bool _servicesRunning = false;

    public static bool ServicesRunning => _servicesRunning;

    // Start is called before the first frame update
    void Awake()
    {
        tokenSource = new CancellationTokenSource();
        token = tokenSource.Token;
        QuestionMatcher = Task.Run(() => RunProcessAsync("python", "PythonTest/service.py", token));
        SpeechRecognizer = Task.Run(() => RunProcessAsync(@".\SpeechRecognizer\SpeechRecognizer\bin\Debug\SpeechRecognizer.exe", "google", token));
        UnityEngine.Debug.Log($"Task {QuestionMatcher.Id} executing");
        UnityEngine.Debug.Log($"Task {SpeechRecognizer.Id} executing");
        tasks.Add(QuestionMatcher);
        tasks.Add(SpeechRecognizer);

        StartCoroutine("Delay");
    }

    private IEnumerable Delay()
    {
        yield return new WaitForSeconds(5);

        ServicesReady.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void OnApplicationQuit()
    {
        tokenSource.Cancel();
        try
        {
            await Task.WhenAll(tasks.ToArray());
        }
        catch (OperationCanceledException)
        {
            UnityEngine.Debug.Log($"\n{nameof(OperationCanceledException)} thrown\n");
        }
        finally
        {
            tokenSource.Dispose();
        }
    }

    static Task<int> RunProcessAsync(string command, string args, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<int>();

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = command;
        startInfo.Arguments = args;
        //startInfo.RedirectStandardOutput = true;
        //startInfo.RedirectStandardError = true;
        //startInfo.UseShellExecute = false;
        //startInfo.CreateNoWindow = true;

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
}
