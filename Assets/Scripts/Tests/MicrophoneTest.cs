using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour
{
    Image image;
    [SerializeField] private int multiplier;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, multiplier * BlowDetector.RMSVolumeValue);
    }
}
