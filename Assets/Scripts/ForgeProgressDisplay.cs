using UnityEngine;
using UnityEngine.UI;

public class ForgeProgressDisplay : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private Image needle;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        width = bar.rectTransform.sizeDelta.x;
    }

    public void SetNeedleProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        var lastPos = needle.rectTransform.anchoredPosition;
        needle.rectTransform.anchoredPosition = new(progress * width, lastPos.y);
    }
}
