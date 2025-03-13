using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.VFX;

public class LiquidDropContainer : MonoBehaviour
{
    public float Amount { get; set; }
}

public class PouringMetal : CraftingProcess, ICraftingProcess
{
    public event Action OnPouringFinished;
    public float MoveFraction { get; private set; }

    [SerializeField] private float maxPourSpeed = 5f; // Maximum pouring rate
    [SerializeField] private float pourAcceleration = 10f;
    [SerializeField] private float pourDeceleration = 15f;
    [SerializeField] private float totalLiquidAmount = 100f;
    [SerializeField] private float accelerometerSpeed = 5f;

    private float angleOffset = 180;
    private float currentPourSpeed = 0f;
    private float yOffest = 0.02f;
    private float currentLiquidAmount;


    private float pouringAdjusted;

    private bool isPouring = false;
    private bool isFiniashed = false;

    // Left and Right constraints
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private GradingManager gradingManager;

    [SerializeField]
    private List<PouringZone> pouringZones;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private GameObject liquidCubes;
    [SerializeField] private Transform metalParent;
    [SerializeField] private VisualEffect moltenMetalVFX;
    [SerializeField] private RectTransform vial;

    private Weapon weapon;

    private bool isProcessDone = false;

    public bool IsProcessDone => isProcessDone;

    [SerializeField] private SoundEffectPlayer soundPlayer;

    void Start()
    {
        amountText.text = $" {totalLiquidAmount.ToString("F1")}";
        currentLiquidAmount = totalLiquidAmount;

        Quaternion adjustedRotation = vial.rotation;
        adjustedRotation *= Quaternion.Euler(0, 0, angleOffset); // Rotate by angleOffset on the Z axis
        moltenMetalVFX = Instantiate(moltenMetalVFX, vial.position, adjustedRotation, vial);
    }

    void Update()
    {
        MoveAccelerometer();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                isPouring = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPouring = false;
            }
        }
        else
        {
            isPouring = false;
        }

        PourMetal();
    }

    private void PourMetal()
    {
        if (isPouring && currentLiquidAmount > 0)
        {
            currentPourSpeed = Mathf.Min(currentPourSpeed + pourAcceleration * Time.deltaTime, maxPourSpeed);

        }
        else
        {
            currentPourSpeed = Mathf.Max(currentPourSpeed - pourDeceleration * Time.deltaTime, 0);
            EnableVFX();
        }

        if (currentLiquidAmount > 0 && currentPourSpeed > 0)
        {
            pouringAdjusted = Mathf.Min(currentLiquidAmount, currentPourSpeed / 100f);
            currentLiquidAmount = Mathf.Max(currentLiquidAmount - pouringAdjusted, 0);
            amountText.text = $"{currentLiquidAmount:F1}";

            Instantiate(liquidCubes, new Vector3(transform.position.x, transform.position.y - yOffest, transform.position.z),
                        Quaternion.identity, metalParent)
                .AddComponent<LiquidDropContainer>().Amount = pouringAdjusted;
        }

        if (currentLiquidAmount <= 0)
        {
            DisableVFX();
            IsPouringFinished(currentLiquidAmount);
        }
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
        MoveFraction = (newX - pointA.position.x) / (pointB.position.x - pointA.position.x);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public float GetTotalLiquid() => totalLiquidAmount;
    public int ZoneCount() => pouringZones.Count;

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }

    private void EnableVFX()
    {
        if (moltenMetalVFX != null)
        {
            moltenMetalVFX.Play();
        }
    }

    private void DisableVFX()
    {
        if (moltenMetalVFX != null)
        {
            moltenMetalVFX.Stop();
        }
    }
}


