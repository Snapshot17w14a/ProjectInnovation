using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PouringMetal : MonoBehaviour
{
    [SerializeField] private float maxPourSpeed = 5f; // Maximum pouring rate
    [SerializeField] private float pourAcceleration = 10f;
    [SerializeField] private float pourDeceleration = 15f;
    [SerializeField] private float totalLiquid = 30f; // Total amount of liquid available
    [SerializeField] private float pourGoal = 30f; // Goal amount for pouring
    [SerializeField] private float accelerometerSpeed = 5f;

    private float pourAmount = 0f;
    private float currentPourSpeed = 0f;
    private float yOffest = 0.02f;

    private bool isPouring = false;

    // Left and Right constraints
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private List<PouringZones> pouringZones;

    [SerializeField] private TMP_Text amountText;

    [SerializeField] private GameObject liquidCubes;

    void Start()
    {
        amountText.text = $" {totalLiquid.ToString("F1")}";
    }

    private void Update()
    {
        MoveAccelerometer();
        PourMetal();
    }

    private void PourMetal()
    {
        if (isPouring)
        {
            currentPourSpeed = Mathf.Min(currentPourSpeed + pourAcceleration * Time.deltaTime, maxPourSpeed);
        }
        else
        {
            currentPourSpeed = Mathf.Max(currentPourSpeed - pourDeceleration * Time.deltaTime, 0);
        }

        if(totalLiquid > 0 && currentPourSpeed > 0)
        {
            float pouringAdjusted = currentPourSpeed * Time.deltaTime;
            totalLiquid = Mathf.Max(totalLiquid - pouringAdjusted, 0);
            pourAmount += pouringAdjusted;
            amountText.text = $"{totalLiquid.ToString("F1")}";
            Instantiate(liquidCubes, new Vector3(transform.position.x, transform.position.y - yOffest, transform.position.z), Quaternion.identity) ;
        }
    }

    private void MoveAccelerometer()
    {
        float move = (Input.acceleration.x / accelerometerSpeed) * Time.deltaTime;
        float newX = transform.position.x + move;

        newX = Mathf.Clamp(newX, pointA.position.x, pointB.position.x);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void IsPouring() => isPouring = !isPouring;
}


