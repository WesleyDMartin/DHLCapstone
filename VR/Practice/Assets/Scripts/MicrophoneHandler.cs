using System;
using System.Collections.Generic;
using System.IO;
using System.Speech;
using System.Speech.Recognition;
using System.Linq;
using UnityEngine;
using UnityScript.Lang;

public class MicrophoneHandler : MonoBehaviour
{
    public static float MicLoudness;
    public bool IsRecording;
    private readonly int _sampleWindow = 128;
    private AudioClip _clipRecord;
    private List<AudioClip> _audioClips = new List<AudioClip>();
    private string _device;
    private bool _isInitialized;
    private static string _filePath;
    private int count = 0;

    public float testSound;
    public bool WriteDone = false;

    private void InitMic()
    {
        int maxFreq;
        int minFreq;

        //Check if there is at least one microphone connected  
        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present  
        {

            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0)
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
        }

        _filePath = Path.Combine(Application.persistentDataPath, "test.wav");
        if (_device == null)
        {
            _device = Microphone.devices[0];
            //_clipRecord = Microphone.Start(_device, true, 3, 44100);
            Debug.Log(_clipRecord);
        }
    }

    public void StopMicrophone()
    {
        Microphone.End(_device);
    }

    public void StartRecording()
    {
        IsRecording = true;
        //Microphone.End(_device);
        _clipRecord = Microphone.Start(_device, true, 20, 44100);
    }

    public string StopRecording()
    {
        Microphone.End(_device);
        IsRecording = false;
        var message = "Testing";
        //var clip = SavWav.TrimSilence(_clipRecord, 1);
        var clip = _clipRecord;
        SavWav.Save(_filePath, clip);
        //_clipRecord = Microphone.Start(_device, true, 3, 44100);
        WriteDone = true;
        return _filePath;
    }

    private void Update()
    {
        //MicLoudness = LevelMax();
        //testSound = MicLoudness;
    }

    private void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    private void OnDisable()
    {
        StopMicrophone();
    }

    private void OnDestory()
    {
        StopMicrophone();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }

        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }


    /// <summary>
    ///     Combines the specified clips.
    ///     Thanks to https://answers.unity.com/questions/513408/combine-2-audio-clip-in-one.html
    /// </summary>
    /// <param name="clips">The clips.</param>
    /// <returns></returns>
    public static AudioClip Combine(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0)
            return null;

        var length = 0;
        for (var i = 0; i < clips.Count; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        var data = new float[length];
        length = 0;
        for (var i = 0; i < clips.Count; i++)
        {
            if (clips[i] == null)
                continue;

            var buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        var result = AudioClip.Create("Combine", length, 1, 44100, false, false);
        result.SetData(data, 0);

        return result;
    }
}