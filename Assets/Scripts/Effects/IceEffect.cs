using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : IWeaponEffect
{
    [SerializeField] private float reductionPerStack = 0.1f;
    [SerializeField] private int maxStacks = 3;
    private int currentStacks = 0;

    private Character targetEnemy;

    public void ApplyEffect(Character target, params object[] parameters)
    {
        if (targetEnemy == null)
        {
            targetEnemy = target;
        }

        AddStack();
    }

    private void AddStack()
    {
        if (currentStacks >= maxStacks)
        {
            Debug.Log("IceEffect already at max stacks!");
            return;
        }

        currentStacks++;
        float reductionMultiplier = 1f - (reductionPerStack * currentStacks);

        if(targetEnemy != null)
        {
            targetEnemy.AddIceEffectStack(maxStacks);
        }
    }
}