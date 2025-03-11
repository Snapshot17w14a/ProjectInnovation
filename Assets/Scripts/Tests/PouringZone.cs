using UnityEngine;

public class PouringZone : MonoBehaviour
{
    public float PourFraction { get; private set; }

    [SerializeField] private PouringMetal pouringMetal;
    [SerializeField] private GradingManager gradingManager;

    [SerializeField] private float scaleMultiplier = 0.01f;
    private float accumulatedMetal;

    [Header("Grading Ranges")]
    [SerializeField] private float goodMin = 1f;
    [SerializeField] private float goodMax = 2f;
    [SerializeField] private float averageMin = 3f;
    [SerializeField] private float averageMax = 4f;

    private void Awake()
    {
        pouringMetal.OnPouringFinished += CalculateGrade;
    }

    private void OnDestroy()
    {
        pouringMetal.OnPouringFinished -= CalculateGrade;
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectMetal(other.gameObject.GetComponent<LiquidDropContainer>().Amount);
        Destroy(other.gameObject);
    }

    public void CollectMetal(float amount)
    {
        accumulatedMetal += amount;

        float totalLiquid = pouringMetal.GetTotalLiquid();
        float perfectAmount = totalLiquid / pouringMetal.ZoneCount();
        PourFraction = accumulatedMetal / perfectAmount;
    }

    private void CalculateGrade()
    {
        float totalLiquid = pouringMetal.GetTotalLiquid();
        float perfectAmount = totalLiquid / pouringMetal.ZoneCount();

        float difference = Mathf.Abs(perfectAmount - accumulatedMetal);
        int roundedDifference = Mathf.RoundToInt(difference);
        int gradeValue;

        if (roundedDifference == 0)
        {
            gradeValue = 3;
        }
        else if (roundedDifference >= goodMin && roundedDifference <= goodMax)
        {
            gradeValue = 2;
        }
        else if (roundedDifference >= averageMin && roundedDifference <= averageMax)
        {
            gradeValue = 1;
        }
        else
        {
            gradeValue= 0;
        }

        gradingManager.RegisterGrade(gradeValue);
    }
}
