using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringZones : MonoBehaviour
{
    private bool isPouring = false;

    private float accumulatedMetal = 0f;

    public void PourMetal(float amount)
    {
        accumulatedMetal += amount;
        Debug.Log($"Accumulated metal: {accumulatedMetal}");
    }
}
