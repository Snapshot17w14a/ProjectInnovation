using System.Collections;
using UnityEngine;

public class Pet : Character
{
    protected override void Start()
    {
        base.Start();
        stats = ServiceLocator.GetService<InventoryManager>().PetNameToStats(name.Replace("(Clone)", "")).Clone();
        if (stats.skill != null)
        {
            stats.skill = (Skill)ScriptableObject.CreateInstance(stats.skill.GetType());
            stats.skill.FromStaticSkill(preset.Skill);
            stats.skill.parent = gameObject;
        }
    }

    protected virtual IEnumerator SkillRoutine()
    {
        yield return new WaitUntil(() => battleManager != null && battleManager.IsAttackingAllowed);
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        while (isBattling)
        {
            stats.skill.UseSkill();
            yield return new WaitForSeconds(stats.skill.SkillCooldown);
        }
    }

    protected void FeedPet(Food food)
    {
        stats.Health += food.Regen;
    }

    protected override void Attack()
    {
        var target = battleManager.GetTargetCharacter(CharacterType.Pet);
        target.TakeDamage(stats.Damage * (stats.Weapon != null ? (stats.Weapon.CritChance > Random.Range(0, 100) ? Mathf.RoundToInt(stats.Weapon.CriticalDamage / 100f) : 1) : 1));
        stats.Weapon.weaponEffect.ApplyEffect(target);
        characterAnimator.SetTrigger("Attack");
    }

    public override void StartBattle()
    {
        base.StartBattle();
        StartCoroutine(SkillRoutine());
    }
}