using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PouringMetal : MonoBehaviour
{
    Gyroscope gyroscope;
    Quaternion gyroscopeRotation;
    Quaternion ninetydegx = Quaternion.Euler(-90, 0, 0);


    private float pouringAngle = 140f;
    [SerializeField]
    private float maxPourSpeed = 50f;
    [SerializeField]
    private float pourRate = 1f;
    private float lastTiltAngle = 0f;
    private float pourAmount = 0f;
    [SerializeField]
    private float pourGoal = 30f;

    private float lastAbsoluteTiltAngle;

    [SerializeField] private Transform meterBall;
    [SerializeField] private Transform startingPoint;

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
        float tiltAngle = NormalizeAngle(euler.z); // Normalize to 180

        // Check if tilt is beyond the pouring threshold
        if (tiltAngle <= pouringAngle && !IsCompleted())
        {

            float pourSpeed = tiltAngle - lastTiltAngle;
            float AbsoluteTiltAngle = Mathf.Abs(tiltAngle);
            //Debug.Log(" " + pourSpeed);

            if (pourSpeed > -maxPourSpeed && AbsoluteTiltAngle <= pouringAngle)
            {
                if (AbsoluteTiltAngle <= lastAbsoluteTiltAngle)
                {
                    meterBall.transform.position += new Vector3(pourSpeed, 0, 0) * Time.deltaTime;
                }
                pourAmount += pourRate * Time.deltaTime;
                //Debug.Log("Pouring Metal: " + pourAmount);

            }
            else
            {
                Debug.Log("Pouring too fast! Quality decreased.");
            }
            lastAbsoluteTiltAngle = AbsoluteTiltAngle;
        }
        else if (IsCompleted())
        {
            //Debug.Log("Casting Completed!");
        }

        lastTiltAngle = tiltAngle;
        float ballToStartingPoint = startingPoint.position.x - meterBall.position.x;
        meterBall.transform.position += new Vector3(ballToStartingPoint, 0, 0) * Time.deltaTime;

    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? Mathf.Abs(angle - 360) : angle;
    }

    private bool IsCompleted()
    {
        return pourAmount >= pourGoal;
    }
}
