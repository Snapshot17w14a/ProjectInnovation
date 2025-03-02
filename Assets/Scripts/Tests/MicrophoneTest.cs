using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour
{
    Image image;
    [SerializeField] private int multiplier;
    [SerializeField] private float volumeThreshold;
    [SerializeField] private ParticleSystem fireParticle;
    ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private BlowDetector blowDetector;
    

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        emissionModule = fireParticle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (blowDetector == null) return;
        emissionModule.rateOverTime = blowDetector.RMSVolumeValue > volumeThreshold ? blowDetector.RMSVolumeValue * 10 * 80 : 30;
        image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, multiplier * blowDetector.RMSVolumeValue);
        image.color = blowDetector.RMSVolumeValue > volumeThreshold ? Color.red : Color.white; 
    }
}
