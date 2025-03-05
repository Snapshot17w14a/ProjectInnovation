using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringZones : MonoBehaviour
{

    [SerializeField] private PouringMetal pouringMetal;
    [SerializeField] private Transform meter;
    [SerializeField] private float scaleMultiplier = 0.01f;
    private float accumulatedMetal;

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

        if(meter != null)
        {
            Vector3 newScale = meter.localScale;
            newScale.y += amount * scaleMultiplier;
            meter.localScale = newScale;
        }
    }

    private void CalculateGrade()
    {
        Debug.Log($"IndividualMetal: {accumulatedMetal}");

        float totalLiquid = pouringMetal.GetTotalLiquid();
        float perfectAmount = totalLiquid / pouringMetal.ZoneCount();

        float difference = Mathf.Abs(perfectAmount - accumulatedMetal);

        string grade;

        if ((int)difference == 0)
        {
            grade = "Perfect";
        }
        else if ((int)difference >= 2 && (int)difference <= 2)
        {
            grade = "Good";
        }
        else if ((int)difference >= 4 && (int)difference <= 4)
        {
            grade = "Average";
        }
        else
        {
            grade = "Bad";
        }

        Debug.Log($"Grade: {grade}");
    }
}
