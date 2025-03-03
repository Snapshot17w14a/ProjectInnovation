using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    TextMeshProUGUI damageNumber;
    [SerializeField] private float timeoutTime = 1;
    [SerializeField] private float minTextSize = 5;
    [SerializeField] private float maxTextSize = 50;
    [SerializeField] private float movementDistance = 100;
    [SerializeField] private float sizeDelta = 30;
    [SerializeField] private Gradient damageGradient;
    private float aliveTime = 0;

    public int Damage { get; set; }

    private void Start()
    {
        damageNumber = GetComponent<TextMeshProUGUI>();
        CalculateTextSizeAndColor(Damage);
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > timeoutTime) Destroy(gameObject);
        damageNumber.alpha = timeoutTime - aliveTime;
        damageNumber.fontSize += sizeDelta * Time.deltaTime;
        transform.position += new Vector3(0, movementDistance * Time.deltaTime, 0);
    }

    private void CalculateTextSizeAndColor(int damage)
    {
        float damagePercentage = damage / 120f;
        damageNumber.fontSize = Mathf.Lerp(minTextSize, maxTextSize, damagePercentage);
        damageNumber.text = damage.ToString();
        damageNumber.color = damageGradient.Evaluate(damagePercentage);
    }
}
