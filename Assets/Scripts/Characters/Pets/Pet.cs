using System.Collections;
using UnityEngine;

public class Pet : Character
{
    [SerializeField] private bool test;
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private HealthDisplay healthDisplay;
    protected override bool IsEnemyInRange => test;

    protected virtual IEnumerator SkillRoutine()
    {
        while (isBattling)
        {
            yield return new WaitUntil(() => IsEnemyInRange);
            stats.skill.UseSkill();
            yield return new WaitForSeconds(stats.SkillCooldown);
        }
    }

    protected void FeedPet(Food food)
    {
        stats.Health += food.Regen;
    }

    protected override void Attack()
    {
        Debug.Log("Attacked emeny, dmg: " + stats.Damage);
        Instantiate(damageNumberPrefab, transform.position, Quaternion.identity, transform).GetComponent<DamageDisplay>().Damage = Random.Range(0, 120);
        healthDisplay.Percentage = Random.Range(0, 100) / 100f;
    }

    protected override void Start()
    {
        base.Start();
        isBattling = true;
        StartCoroutine(AttackRoutine());
    }
}