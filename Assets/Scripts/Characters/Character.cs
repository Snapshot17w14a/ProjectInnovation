using System.Collections;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharacterPreset preset;
    private static GameObject damageNumberPrefab;

    private HealthDisplay healthDisplay;

    protected Animator characterAnimator;
    protected CharacterStats stats;
    protected BattleManager battleManager;

    protected bool isBattling;
    protected bool isMarkedForDestruction = false;

    protected Animator characterAnimator;
    protected BattleManager battleManager;

    public Vector3 HitPosition => hitPosition;
    private Vector3 hitPosition;

    public enum CharacterType
    {
        Pet,
        Enemy
    }

    protected virtual IEnumerator AttackRoutine()
    {
        yield return new WaitUntil(() => battleManager != null && battleManager.IsAttackingAllowed);
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        while (isBattling)
        {
            Attack();
            yield return new WaitForSeconds(stats.AttackCooldown);
        }
    }

    protected abstract void Attack();

    protected virtual void Start()
    {
        stats = new CharacterStats(preset);
        characterAnimator = GetComponent<Animator>();
        healthDisplay = GetComponentInChildren<HealthDisplay>();

        hitPosition = transform.Find("HitPosition").position;

        if (damageNumberPrefab == null) damageNumberPrefab = Resources.Load<GameObject>("DamageNumber");

        isBattling = true;
        StartCoroutine(AttackRoutine());
    }

    public virtual void TakeDamage(int damage)
    {
        stats.Health -= Mathf.Max(0, damage - stats.Defense);
        Instantiate(damageNumberPrefab, transform.position, Quaternion.identity, transform).GetComponent<DamageDisplay>().Damage = damage;
        healthDisplay.Percentage = stats.Health / (float)stats.MaxHealth;
        characterAnimator.SetTrigger("Hit");
        if (stats.Health <= 0)
        {
            Destroy(gameObject);
            isMarkedForDestruction = true;
        }
    }

    public void SetManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        battleManager.OnBattleEnd += EndBattle;
    }

    private void EndBattle()
    {
        isBattling = false;
        StopAllCoroutines();
    }

    protected virtual void OnDestroy()
    {
        battleManager.OnBattleEnd -= EndBattle;
    }
}
