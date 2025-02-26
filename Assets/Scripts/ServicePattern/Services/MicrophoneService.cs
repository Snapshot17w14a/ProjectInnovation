using System.Collections;
using UnityEngine;

public class MicrophoneService : Service
{
    private AudioClip recording;
    private bool isRecording = false;

    public ref AudioClip RecordingClip => ref recording;

    public MicrophoneService()
    {
        CheckForAuthorization();
    }

    private IEnumerator CheckForAuthorization()
    {
        FindMicrophones();

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        Debug.Assert(Application.HasUserAuthorization(UserAuthorization.Microphone), "Microphone usage privlage.");
    }

    public AudioClip StartRecording()
    {
        if (isRecording)
        {
            Debug.LogError("Microphone service is already recording");
            return null;
        }

        recording = AudioClip.Create("MicrophoneRecording", 128, 1, 44100, false);
        recording = Microphone.Start(Microphone.devices[0], true, 1, 44100);
        if (recording == null) throw new System.Exception("Microphone recording failed to start.");
        isRecording = true;

        return recording;
    }

    public void StopRecording()
    {
        Microphone.End(Microphone.devices[0]);
        isRecording = false;
    }

    private void FindMicrophones()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
}
