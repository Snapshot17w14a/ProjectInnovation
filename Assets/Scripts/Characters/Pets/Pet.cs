using System.Collections;
using UnityEngine;

public class Pet : Character
{
    protected virtual IEnumerator SkillRoutine()
    {
        yield return new WaitUntil(() => battleManager != null && battleManager.IsAttackingAllowed);
        while (isBattling)
        {
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
        var target = battleManager.GetTargetCharacter(CharacterType.Pet);
        target.TakeDamage(stats.Damage);
        characterAnimator.SetTrigger("Attack");
    }

    protected override void Start()
    {
        base.Start();
    }
}