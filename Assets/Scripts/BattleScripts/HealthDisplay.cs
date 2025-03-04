using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private RectTransform fillBar;
    private float width;
    private float percentage = 1;
    public float Percentage
    {
        get => percentage;
        set
        {
            percentage = value;
            fillBar.sizeDelta = new Vector2(width * percentage, fillBar.sizeDelta.y);
        }
    }

    private void Start()
    {
        width = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        fillBar = transform.GetChild(1).GetComponent<RectTransform>();
    }
}
