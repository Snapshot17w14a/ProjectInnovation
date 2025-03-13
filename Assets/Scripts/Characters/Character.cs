using System.Collections;
using System.Collections.Generic;
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

    public Vector3 HitPosition => hitPosition;
    private Vector3 hitPosition;

    private static Transform damageNumberParent;

    private List<Buff> buffs = new List<Buff>();

    protected SoundEffectPlayer soundEffectPlayer;

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
            soundEffectPlayer.SetVolume = 0.5f;
            soundEffectPlayer.PlayAudioWithRange(0, 3);
            yield return new WaitForSeconds(stats.AttackCooldown);
        }
    }

    protected abstract void Attack();

    protected virtual void Start()
    {
        if (preset != null) stats = new CharacterStats(preset);
        characterAnimator = GetComponent<Animator>();
        healthDisplay = GetComponentInChildren<HealthDisplay>();

        hitPosition = transform.Find("HitPosition").position;

        if (damageNumberParent == null) damageNumberParent =  GameObject.Find("DamageNumbers").transform;
        if (damageNumberPrefab == null) damageNumberPrefab = Resources.Load<GameObject>("DamageNumber");

        soundEffectPlayer = transform.GetComponentInChildren<SoundEffectPlayer>();
    }

    public virtual void TakeDamage(int damage)
    {
        if (isMarkedForDestruction) return;
        stats.Health -= Mathf.Max(0, damage - stats.Defense);
        Instantiate(damageNumberPrefab, transform.position, Quaternion.identity, damageNumberParent).GetComponent<DamageDisplay>().Damage = damage;
        healthDisplay.Percentage = stats.Health / (float)stats.MaxHealth;
        characterAnimator.SetTrigger("Hit");
        if (stats.Health <= 0)
        {
            Destroy(gameObject, 1.5f);
            StopAllCoroutines();
            characterAnimator.SetTrigger("Death");
            isMarkedForDestruction = true;
        }
    }

    private void Update()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            Buff buff = buffs[i];
            if (buff.IsExpired)
            {
                buff.OnRemoved(this);
                buffs.Remove(buff);
            }
        }
    }
    public void AddBuff(Buff buff)
    {
        buff.OnApplied(this);
        buffs.Add(buff);
    }

    public virtual void Heal(int healAmount)
    {
        stats.Health = Mathf.Min(stats.MaxHealth, stats.Health + Mathf.Min(0, healAmount));
        // TODO: add heal number prefab
        healthDisplay.Percentage = stats.Health / (float)stats.MaxHealth;
        // TODO: Animation for healing
    }

    public void AddDamageStat(int damage)
    {
        stats.Damage += Mathf.Min(0, damage);
    }

    public void RemoveDamageStat(int damage)
    {
        stats.Damage -= Mathf.Max(0, damage);
    }

    public void AddIceEffectStack(int maxStacks)
    {
        stats.IceEffectStacks = Mathf.Min(maxStacks, stats.IceEffectStacks + 1);
    }

    public void RemoveIceEffectStack()
    {
        stats.IceEffectStacks = Mathf.Max(0, stats.IceEffectStacks - 1);
    }

    public void SetManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        battleManager.OnBattleEnd += EndBattle;
    }

    public virtual void StartBattle()
    {
        isBattling = true;
        StartCoroutine(AttackRoutine());
    }

    public virtual void EndBattle()
    {
        isBattling = false;
        battleManager.OnBattleEnd -= EndBattle;
        StopAllCoroutines();
    }

    protected virtual void OnDestroy()
    {
        EndBattle();
    }
}
