using UnityEngine;
using UnityEngine.UI;

public class ForgeProgressDisplay : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private Image needle;
    [SerializeField] private Image correctSpot;
    [SerializeField] private Image volumeImage;

    [SerializeField] private float particleMultiplier = 10;
    [SerializeField] private ParticleSystem fireParticles;
    private ParticleSystem.EmissionModule emissionModule;

    private float width;
    private float correctSpotWidth;
    private float correctSpotX;

    // Start is called before the first frame update
    void Start()
    {
        width = bar.rectTransform.sizeDelta.x;
        correctSpotWidth = correctSpot.rectTransform.sizeDelta.x;
        correctSpotX = correctSpot.rectTransform.anchoredPosition.x;

        emissionModule = fireParticles.emission;
    }

    public void SetNeedleProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        needle.rectTransform.anchoredPosition = new(progress * width, 0);
    }

    public bool IsNeedlePositionCorrect()
    {
        float needleX = needle.rectTransform.anchoredPosition.x;
        return needleX >= correctSpotX && needleX <= correctSpotX + correctSpotWidth;   
    }

    public void DisplayMicrophoneVolume(float microphoneVolume, float volumeThreshold)
    {
        volumeImage.rectTransform.sizeDelta = new Vector2(volumeImage.rectTransform.sizeDelta.x, microphoneVolume * 100);
        volumeImage.color = microphoneVolume > volumeThreshold ? Color.red : Color.white;
    }

    public void UpdateFireParticles(float microphoneVolume, float volumeThreshold)
    {
        emissionModule.rateOverTime = microphoneVolume > volumeThreshold ? microphoneVolume * 10 * particleMultiplier : 30;
    }
}
