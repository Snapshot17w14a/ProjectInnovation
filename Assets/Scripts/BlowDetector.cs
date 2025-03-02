using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(MicrophoneService))]
public class BlowDetector : MonoBehaviour
{
    [SerializeField] private AudioSource microponeSource;
    [SerializeField] private int sampleCount = 128;
    private float[] samples;

    public float RMSVolumeValue => RMSVolume();

    // Initialize the MicrophoneService
    private void Awake()
    {
        samples = new float[sampleCount];
    }

    // Start recording on the default microphone of the device
    private void Start()
    {
        microponeSource = GetComponent<AudioSource>();

        var micService = ServiceLocator.GetService<MicrophoneService>();

        microponeSource.clip = micService.StartRecording(sampleCount);
        while (!(Microphone.GetPosition(null) > 0)) //Wait until the microphone recording catches up to real-time
        microponeSource.Play();
    }

    private void OnDestroy()
    {
        var microphoneService = ServiceLocator.GetService<MicrophoneService>();
        microphoneService.StopRecording();
        ServiceLocator.UnregisterService<MicrophoneService>(microphoneService);
    }

    // Print the RMSVolume of the clip
    void Update()
    {
         
    }

    private float RMSVolume()
    {
        int startPos = ServiceLocator.GetService<MicrophoneService>().MicrophoneOffset - sampleCount; 
        if (startPos < 0) startPos += sampleCount;

        microponeSource.clip.GetData(samples, startPos);
        float sum = 0;

        foreach (float value in samples) 
        {
            sum += value * value;
        }

        return Mathf.Sqrt(sum / sampleCount);
    }
}
