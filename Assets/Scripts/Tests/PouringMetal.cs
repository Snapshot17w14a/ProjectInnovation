using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static UnityEditor.Progress;

public class LiquidDropContainer : MonoBehaviour
{
    public float Amount { get; set; }
}

public class PouringMetal : CraftingProcess, ICraftingProcess
{
    public event Action OnPouringFinished;

    [SerializeField] private float maxPourSpeed = 5f; // Maximum pouring rate
    [SerializeField] private float pourAcceleration = 10f;
    [SerializeField] private float pourDeceleration = 15f;
    [SerializeField] private float totalLiquidAmount = 100f;
    [SerializeField] private float pourGoal = 30f; // Goal amount for pouring
    [SerializeField] private float accelerometerSpeed = 5f;

    private float pourAmount = 0f;
    private float currentPourSpeed = 0f;
    private float yOffest = 0.02f;
    private float gradeAmount = 0f;
    private float currentLiquidAmount;


    private float pouringAdjusted;

    private bool isPouring = false;
    private bool isFiniashed = false;

    // Left and Right constraints
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private GradingManager gradingManager;

    [SerializeField]
    private List<PouringZones> pouringZones;

    [SerializeField] private TMP_Text amountText;

    [SerializeField] private GameObject liquidCubes;

    [SerializeField] private Transform metalParent;

    private Weapon weapon;

    private bool isProcessDone = false;

    public bool IsProcessDone => isProcessDone;

    void Start()
    {
        amountText.text = $" {currentLiquidAmount.ToString("F1")}";
        currentLiquidAmount = totalLiquidAmount;
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

        if(currentLiquidAmount > 0 && currentPourSpeed > 0)
        {
            pouringAdjusted = (currentLiquidAmount < currentPourSpeed / 100f ? currentLiquidAmount : currentPourSpeed / 100f);
            currentLiquidAmount = Mathf.Max(currentLiquidAmount - pouringAdjusted, 0);
            amountText.text = $"{currentLiquidAmount.ToString("F1")}";
            Instantiate(liquidCubes, new Vector3(transform.position.x, transform.position.y - yOffest, transform.position.z), Quaternion.identity, metalParent).AddComponent<LiquidDropContainer>().Amount = pouringAdjusted;
            
        }
        else if(currentLiquidAmount <= 0) IsPouringFinished(currentLiquidAmount);
    }

    private void IsPouringFinished(float totalLiquid)
    {
        //Can also add the timer as a condition
        if (totalLiquid <= 0 && !isFiniashed && metalParent.childCount == 0)
        {
            isFiniashed = true;
            gradingManager.ResetGrades();
            OnPouringFinished?.Invoke();
            gradingManager.DisplayGrade();
            weapon.SetCastResult(gradingManager.GetOverallGrade());
            isProcessDone = true;
        }
    }

    private void MoveAccelerometer()
    {
        float move = (Input.acceleration.x / accelerometerSpeed) * Time.deltaTime;
        float newX = transform.position.x + move;

        newX = Mathf.Clamp(newX, pointA.position.x, pointB.position.x);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public float GetTotalLiquid() => totalLiquidAmount;
    public int ZoneCount() => pouringZones.Count;

    public void isCasting() => isPouring = !isPouring;

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }
}


