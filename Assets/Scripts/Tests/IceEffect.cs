using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour, IWeaponEffect
{
    [SerializeField] private float reductionPerStack = 0.1f;
    [SerializeField] private int maxStacks = 3;
    private int currentStacks = 0;

    public void ApplyEffect()
    {
        AddStack();
    }

    private void AddStack()
    {
        if (currentStacks < maxStacks)
        {
            currentStacks++;
            UpdateAttackSpeed();
            Debug.Log("Ice Stacks " + currentStacks);
        }
        else
        {
            Debug.Log("IceEffect already at max stacks!");
        }
    }

    private void UpdateAttackSpeed()
    {
        float reductionMultiplier = 1f - (reductionPerStack * currentStacks);
        //Maybe we modify attackSpeed of the enemy here?
    }

    public void RemoveEffect()
    {

    }
}
