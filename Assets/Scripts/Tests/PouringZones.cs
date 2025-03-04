using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringZones : MonoBehaviour
{
    [SerializeField] private Transform meter;
    [SerializeField] private float scaleMultiplier = 0.01f;
    private float accumulatedMetal;

    private void OnTriggerEnter(Collider other)
    {
        CollectMetal(1f);
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
}
