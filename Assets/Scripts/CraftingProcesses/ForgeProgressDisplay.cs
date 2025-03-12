using UnityEngine;
using UnityEngine.UI;

public class ForgeProgressDisplay : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private Image needle;
    [SerializeField] private Image correctSpot;
    [SerializeField] private Image volumeImage;

    [SerializeField] private float maxIntensity;
    [SerializeField] private Material bigCrystalMaterial;
    [SerializeField] private Material lineartCrystalMaterial;
    [SerializeField] private Gradient crystalColorGradient;
    [SerializeField] private Gradient lineartColorGradient;

    private float width;
    private float correctSpotWidth;
    private float correctSpotX;

    // Start is called before the first frame update
    void Start()
    {
        width = bar.rectTransform.sizeDelta.x;
        correctSpotWidth = correctSpot.rectTransform.sizeDelta.x;
        correctSpotX = correctSpot.rectTransform.anchoredPosition.x;

        bigCrystalMaterial.SetVector("_Color_gradient_high", crystalColorGradient.Evaluate(0));
        bigCrystalMaterial.SetVector("_Color_gradient_low", crystalColorGradient.Evaluate(0));

        lineartCrystalMaterial.SetVector("_Color", lineartColorGradient.Evaluate(0));
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

    public void UpdateFireParticles(float t)
    {
        float intensity = Mathf.Lerp(1, maxIntensity, t);

        bigCrystalMaterial.SetVector("_Color_gradient_high", Vector4.Lerp(bigCrystalMaterial.GetVector("_Color_gradient_high"), ColorToVec4(crystalColorGradient.Evaluate(t)) * intensity, Time.deltaTime));
        bigCrystalMaterial.SetVector("_Color_gradient_low", Vector4.Lerp(bigCrystalMaterial.GetVector("_Color_gradient_low"), ColorToVec4(crystalColorGradient.Evaluate(t)) * intensity, Time.deltaTime));

        lineartCrystalMaterial.SetVector("_Color", Vector4.Lerp(lineartCrystalMaterial.GetVector("_Color"), ColorToVec4(lineartColorGradient.Evaluate(t)) * intensity, Time.deltaTime));
    }

    private Vector4 ColorToVec4(Color color) => new(color.r, color.g, color.b, color.a);
}
