using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour, IWeaponEffect
{
    [SerializeField] private float reductionPerStack = 0.1f;
    [SerializeField] private int maxStacks = 3;
    private int currentStacks = 0;

    private Enemy targetEnemy;

    public void ApplyEffect(Enemy enemy)
    {
        AddStack(enemy);
    }

    private void AddStack(Enemy enemy)
    {
        if (currentStacks < maxStacks)
        {
            currentStacks++;
            float reductionMultiplier = 1f - (reductionPerStack * currentStacks);
            Debug.Log("Ice Stacks: " + currentStacks);
        }
        else
        {
            Debug.Log("IceEffect already at max stacks!");
        }
    }

    public void RemoveEffect(Enemy enemy)
    {

    }
}
