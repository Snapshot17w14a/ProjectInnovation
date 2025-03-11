using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour, IWeaponEffect
{
    [SerializeField] private float reductionPerStack = 0.1f;
    [SerializeField] private int maxStacks = 3;
    private int currentStacks = 0;

    private Character targetEnemy;

    public void ApplyEffect(Character enemy)
    {
        if (targetEnemy == null)
        {
            targetEnemy = enemy;
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
            // reduce the attackSpeed of the enemy here but ask Kevin
        }
    }
}
