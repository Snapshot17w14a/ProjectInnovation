using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingScript : MonoBehaviour
{
    Gyroscope gyroscope;
    Quaternion gyroscopeRotation;
    Quaternion ninetydegx = Quaternion.Euler(-90, 0, 0);

    [SerializeField]
    private float pouringAngle = 30f;
    [SerializeField]
    private float maxPourSpeed = 50f;
    [SerializeField]
    private float pourRate = 1f;
    private float lastTiltAngle = 0f;
    private float pourAmount = 0f;

    private bool isPouring = false;

    void Start()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }

    private void Update()
    {
        gyroscopeRotation = GyroToUnity(Input.gyro.attitude);
        float zRotation = gyroscopeRotation.eulerAngles.z;
        float yRotation = gyroscopeRotation.eulerAngles.y;
        float xRotation = gyroscopeRotation.eulerAngles.x;
        transform.rotation = ninetydegx * Quaternion.Euler(xRotation, yRotation, zRotation);

        CheckPouring(transform.rotation);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, -q.y, q.z, -q.w);
    }

    private void CheckPouring(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        float tiltAngle = NormalizeAngle(euler.z); // Normalize to -180 to 180

        // Check if tilt is beyond the pouring threshold
        if (tiltAngle > pouringAngle)
        {
            if (!isPouring)
            {
                isPouring = true;
                Debug.Log("Started Pouring!");
            }

            // Calculate pour speed (change in angle per second)
            float pourSpeed = Mathf.Abs(tiltAngle - lastTiltAngle) / Time.deltaTime;

            if (pourSpeed < maxPourSpeed)
            {
                pourAmount += pourRate * Time.deltaTime;
                Debug.Log("Pouring Metal: " + pourAmount);

            }
            else
            {
                Debug.Log("Pouring too fast! Quality decreased.");
            }
        }
        else if (isPouring)
        {
            isPouring = false;
            Debug.Log("Stopped Pouring.");
        }

        lastTiltAngle = tiltAngle;
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }
}
