using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringZones : MonoBehaviour
{
    private bool isPouring = false;

    private float accumulatedMetal = 0f;

    [SerializeField] private Transform meter;

    [SerializeField] private PouringMetal pouringMeter;
    public void PourMetal(float amount)
    {
        accumulatedMetal += amount;

        if (pouringMeter.isLiquidEmpty())
        {
            var transformLocalScale = meter.transform.localScale;
            transformLocalScale.y += 0.001f;
            meter.transform.localScale = transformLocalScale;
        }
    }
}
