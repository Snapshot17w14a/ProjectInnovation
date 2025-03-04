using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

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
    [SerializeField] private float totalLiquid = 30;
    [SerializeField]
    private float pourGoal = 30f;

    private float lastAbsoluteTiltAngle;

    // Temporary 
    [SerializeField] private Transform meterBall;
    [SerializeField] private Transform startingPoint;

    //---------

    [SerializeField] private float speed = 5f;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    public float moveSpeed = 5f;
    private bool moveLeft = false;
    private bool moveRight = false;

    private bool usingButtonsToMove = false;

    [SerializeField] private List<PouringZones> pouringZones;

    [SerializeField] private GameObject liquid;

    [SerializeField] private TMP_Text amountText;

    //Make the pouring a bit crazy when its tilted too much suddenly
    // make it pour quicker based on the angle that it is being poured

    void Start()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;

        amountText.text = $" {totalLiquid.ToString("F1")}";
    }

    private void Update()
    {
        gyroscopeRotation = GyroToUnity(Input.gyro.attitude);
        float zRotation = gyroscopeRotation.eulerAngles.z;
        float yRotation = gyroscopeRotation.eulerAngles.y;
        float xRotation = gyroscopeRotation.eulerAngles.x;
        transform.rotation = ninetydegx * Quaternion.Euler(xRotation, yRotation, zRotation);

        if (usingButtonsToMove)
        {
            MoveButton();
        }
        else
        {
            MoveAccelerometer();
        }
        CheckPouring(transform.rotation);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, -q.y, q.z, -q.w);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PouringZones>(out PouringZones zone))
        {
            pouringZones.Add(zone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PouringZones>(out PouringZones zone))
        {
            pouringZones.Remove(zone);
        }
    }

    private void CheckPouring(Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        float tiltAngle = NormalizeAngle(euler.z); // Normalize to 180

        // Check if tilt is beyond the pouring threshold
        if (tiltAngle <= pouringAngle && !IsCompleted())
        {

            float pourSpeed = tiltAngle - lastTiltAngle;
            float absoluteTiltAngle = Mathf.Abs(tiltAngle);
            //Debug.Log(" " + pourSpeed);

            if (pourSpeed > -maxPourSpeed && absoluteTiltAngle <= pouringAngle)
            {
                pourRate = 1;
                if (absoluteTiltAngle <= lastAbsoluteTiltAngle)
                {
                    meterBall.transform.position += new Vector3(pourSpeed / 0.25f, 0, 0) * Time.deltaTime;
                }

                totalLiquid -= pourRate * Time.deltaTime;
                amountText.text = $" {totalLiquid.ToString("F1")}";

                if (isLiquidEmpty())
                {
                    pourAmount += pourRate * Time.deltaTime;
                    //temporary
                    Instantiate(liquid, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
                }

                if (pouringZones.Count > 0)
                {
                    float distributedAmount = pourRate * Time.deltaTime / pouringZones.Count;

                    foreach (PouringZones zone in pouringZones)
                    {
                        zone.PourMetal(distributedAmount);
                    }
                }
            }
            else
            {
                Debug.LogError("Pouring too fast! Quality decreased.");
                pourRate = 2;

                if (isLiquidEmpty())
                {
                    pourAmount += pourRate * Time.deltaTime;

                }
            }
            lastAbsoluteTiltAngle = absoluteTiltAngle;
        }
        else if (IsCompleted())
        {
            //Debug.Log("Casting Completed!");
        }

        lastTiltAngle = tiltAngle;
        
        // Temporary
        float ballToStartingPoint = startingPoint.position.x - meterBall.position.x;
        meterBall.transform.position += new Vector3(ballToStartingPoint, 0, 0) * Time.deltaTime;

    }

    private void MoveAccelerometer()
    {
        float move = (Input.acceleration.x / speed) * Time.deltaTime;
        float newX = transform.position.x + move;

        newX = Mathf.Clamp(newX, pointA.position.x, pointB.position.x);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void MoveButton()
    {
        Vector3 deviceRight = Camera.main.transform.right;

        if (moveLeft)
            transform.Translate(-deviceRight * moveSpeed * Time.deltaTime, Space.World);
        else if (moveRight)
            transform.Translate(deviceRight * moveSpeed * Time.deltaTime, Space.World);
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? Mathf.Abs(angle - 360) : angle;
    }

    private bool IsCompleted()
    {
        return pourAmount >= pourGoal;
    }

    public bool isLiquidEmpty()
    {
        return totalLiquid >= 0;
    }

    public void StartMoveLeft() => moveLeft = true;
    public void StopMoveLeft() => moveLeft = false;
    public void StartMoveRight() => moveRight = true;
    public void StopMoveRight() => moveRight = false;
    public void Toggle() => usingButtonsToMove = !usingButtonsToMove;
}
