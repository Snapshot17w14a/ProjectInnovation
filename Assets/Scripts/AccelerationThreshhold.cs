using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationThreshhold : MonoBehaviour
{
    private float hitThreshold = 4f;
    private Vector3 lastAcceleration;
    void Start()
    {
        lastAcceleration = Input.acceleration;
    }

    void Update()
    {
        Vector3 currentAcceleration = Input.acceleration;
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > hitThreshold)
        {
            Debug.Log("Hit!");
        }

        lastAcceleration = currentAcceleration;
    }
}
