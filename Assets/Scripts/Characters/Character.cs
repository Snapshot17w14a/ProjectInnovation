using System.Collections;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterPresets preset;

    protected CharacterStats stats;
    protected bool isBattleing;

    protected abstract bool IsEnemyInRange { get; }

    protected virtual IEnumerator AttackRoutine()
    {
        while (isBattleing)
        {
            yield return new WaitUntil(() => IsEnemyInRange);
            Attack();
            yield return new WaitForSeconds(stats.AttackCooldown);
        }
    }

    protected abstract void Attack();

    protected virtual void Start()
    {
        stats = new CharacterStats(preset);
    }
}
