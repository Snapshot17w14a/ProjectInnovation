using System.Collections;
using UnityEngine;

public class Pet : Character
{
    [SerializeField] private bool test;
    protected override bool IsEnemyInRange => test;

    protected virtual IEnumerator SkillRouting()
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
    }

    protected override void Start()
    {
        base.Start();
        isBattling = true;
        StartCoroutine(AttackRoutine());
    }
}