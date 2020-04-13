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

    public static event EventHandler ServicesReady;

    // Store references to the tasks so that we can wait on them and
    // observe their status after cancellation.
    private Task QuestionMatcher, SpeechRecognizer;
    private ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
    private static bool _servicesRunning = false;

    public static bool ServicesRunning => _servicesRunning;

    // Start is called before the first frame update
    void Awake()
    {
        var path = Application.dataPath;
        tokenSource = new CancellationTokenSource();
        token = tokenSource.Token;
        UnityEngine.Debug.Log(path);
        UnityEngine.Debug.Log(Environment.CurrentDirectory);
        QuestionMatcher = Task.Run(() => RunProcessAsync("python", "service.py", token));
        SpeechRecognizer = Task.Run(() => RunProcessAsync(Environment.CurrentDirectory + @"\SpeechRecognizer\", "SpeechRecognizer.exe", "google", token));
        UnityEngine.Debug.Log($"Task {QuestionMatcher.Id} executing");
        UnityEngine.Debug.Log($"Task {SpeechRecognizer.Id} executing");
        tasks.Add(QuestionMatcher);
        tasks.Add(SpeechRecognizer);

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(10);

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

    public static async Task<int> RunProcessAsync(string command, string args, CancellationToken token)
    {
        UnityEngine.Debug.Log("PATH TO " + args + ": " +command);
        UnityEngine.Debug.Log(args);
        using (var process = new Process
        {
            StartInfo =
        {
            WorkingDirectory = Environment.CurrentDirectory,
            FileName = command,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        },
            EnableRaisingEvents = true
        })
        {
            return await RunProcessAsync(process, token).ConfigureAwait(false);
        }
    }

    public static async Task<int> RunProcessAsync(string path, string command, string args, CancellationToken token)
    {
        UnityEngine.Debug.Log(path);
        UnityEngine.Debug.Log("PATH TO " + args + ": " + command);
        UnityEngine.Debug.Log(args);
        using (var process = new Process
        {
            StartInfo =
        {
            WorkingDirectory = path,
            FileName = path + command,
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        },
            EnableRaisingEvents = true
        })
        {
            return await RunProcessAsync(process, token).ConfigureAwait(false);
        }
    }
    private static Task<int> RunProcessAsync(Process process, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<int>();

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

        process.OutputDataReceived += (s, ea) => UnityEngine.Debug.Log(ea.Data);
        process.ErrorDataReceived += (s, ea) => UnityEngine.Debug.Log("ERR: " + ea.Data);

        UnityEngine.Debug.Log(process.StartInfo.WorkingDirectory);
        try
        {
            bool started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false) 
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
            return tcs.Task;
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return tcs.Task;
    }
}
