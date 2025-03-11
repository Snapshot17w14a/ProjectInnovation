using UnityEngine;

public class Enemy : Character
{
    private static GameObject dropItemPrefab;
    private static Transform dropItemParent;

    protected override void Attack()
    {
        var target = battleManager.GetTargetCharacter(CharacterType.Enemy);
        target.TakeDamage(stats.Damage);
    }

    protected override void Start()
    {
        base.Start();
        if (dropItemPrefab == null) dropItemPrefab = Resources.Load<GameObject>("DropItem");
        if (dropItemParent == null) dropItemParent = battleManager.transform.Find("DropParent");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (isMarkedForDestruction) 
        {
            var dropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity, dropItemParent);
            dropItem.GetComponent<Rigidbody>().AddForce(new(Random.Range(-2f, 2f), 3f, 0), ForceMode.Impulse);
            Destroy(dropItem, 3f);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}