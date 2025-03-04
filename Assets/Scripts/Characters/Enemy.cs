public class Enemy : Character
{
    protected override void Attack()
    {
        var target = battleManager.GetTargetCharacter(CharacterType.Enemy);
        target.TakeDamage(stats.Damage);
    }
}