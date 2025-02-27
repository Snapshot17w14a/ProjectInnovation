using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BlowDetector : MonoBehaviour
{
    [SerializeField] private AudioSource microponeSource;
    [SerializeField] private int sampleCount = 128;
    private float[] samples;

    public float RMSVolumeValue { get; set; }

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

    // Print the RMSVolume of the clip
    void Update()
    {
        float val = RMSVolume();
        RMSVolumeValue = val;
    }

    private float RMSVolume()
    {
        microponeSource.clip.GetData(samples, 0);
        float sum = 0;

        foreach (float value in samples) 
        {
            sum += value * value;
        }

        return Mathf.Sqrt(sum / sampleCount);
    }
}
