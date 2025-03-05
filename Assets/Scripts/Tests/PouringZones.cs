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
        CollectMetal(pouringMetal.pouringAdjusted);
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

    private void CalculateGrade(float accumulatedMetal)
    {
        accumulatedMetal = this.accumulatedMetal;
        Debug.Log($"IndividualMetal: {accumulatedMetal}");
    }

    public float GetCollectedMetal()
    {
        return accumulatedMetal;
    }
}
