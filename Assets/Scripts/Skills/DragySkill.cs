using UnityEngine;

[CreateAssetMenu(fileName = "Dragy Skill", menuName = "Scriptables/Skills/DragySkill")]
public class DragySkill : Skill
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float unitsPerSecond = 200;
    [SerializeField] private float aliveTime = 2;

    public override void UseSkill()
    {
        var enemies = battleManager.AllEnemies;
        foreach (var enemy in enemies) if (enemy != null) enemy.TakeDamage(SkillDamage);
        var projectile = Instantiate(effectPrefab, parent.transform.position, effectPrefab.transform.rotation, parent.transform).AddComponent<DragyFireProjectile>();
        projectile.UnitsPerSecond = unitsPerSecond;
        projectile.AliveTime = aliveTime;
    }

    public override void FromStaticSkill(Skill skill)
    {
        base.FromStaticSkill(skill);
        var dragySkill = (DragySkill)skill;
        effectPrefab = dragySkill.effectPrefab;
        unitsPerSecond = dragySkill.unitsPerSecond;
        aliveTime = dragySkill.aliveTime;
    }
}

public class DragyFireProjectile : MonoBehaviour
{
    public float UnitsPerSecond { get; set; }
    public float AliveTime { get; set; }

    private float timer = 0;

    private void Update()
    {
        transform.position += new Vector3(UnitsPerSecond * Time.deltaTime, 0f, 0f);
        timer += Time.deltaTime;
        if (timer >= AliveTime) Destroy(gameObject);
    }
}