using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    public float SetVolume
    {
        set
        {
            audioSource.volume = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if(audioClips.Length == 0)
        {
            Debug.LogError("No clips in array.");
            return;
        }
        if (audioClips.Length == 1)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }
    }

    public void PlayAudioAtIndex(int index)
    {
        if (index >= audioClips.Length) return;
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlayAudioWithRange(int minInclusive, int maxExclusive)
    {
        if(minInclusive < 0 || maxExclusive >  audioClips.Length) return;
        audioSource.clip = audioClips[Random.Range(minInclusive, maxExclusive)];
        audioSource.Play();
    }
}
