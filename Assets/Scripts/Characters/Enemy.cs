using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    private static GameObject dropItemPrefab;
    private static Transform dropItemParent;

    [SerializeField] private LootTable lootTable;

    private static Sprite[] materialSprites;

    protected override void Attack()
    {
        var target = battleManager.GetTargetCharacter(CharacterType.Enemy);
        target.TakeDamage(stats.Damage);
    }

    protected override void Start()
    {
        base.Start();
        materialSprites ??= Resources.LoadAll<Sprite>("MaterialSprites");
        if (dropItemPrefab == null) dropItemPrefab = Resources.Load<GameObject>("DropItem");
        if (dropItemParent == null) dropItemParent = battleManager.transform.Find("DropParent");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (isMarkedForDestruction) 
        {
            CalculateDrops();
            battleManager.RemoveEnemyFromBattle(this);
        }
    }

    private void CalculateDrops()
    {
        //Debug.Log("Calculating drops");
        int level = stats.Level;
        int index = -1;

        //Debug.Log("Level: " + level);

        //Get the highest level loot entry
        //for(int i = 0; i < lootTable.entries.Length; i++)
        //{
        //    Debug.Log($"Comparing levels, enemy level: {level}, entry level: {lootTable.entries[i].fromLevel - 1}");
        //    if (lootTable.entries[i].fromLevel <= level) index = i;
        //}

        do
        {
            index++;
            //Debug.Log($"Comparing levels, enemy level: {level}, entry level: {lootTable.entries[index].fromLevel}, index: {index}");
        }
        while (index + 1 != lootTable.entries.Length && lootTable.entries[index + 1].fromLevel <= level);

        LootTableEntry values = lootTable.entries[index];
        int levelDifference = level - values.fromLevel;
        var inventoryManager = ServiceLocator.GetService<InventoryManager>();

        //Debug.Log($"lvlDiff: {levelDifference}");

        //Material drops
        if (values.materials.Length != 0)
        {
            for (int i = 0; i < values.materials.Length; i++)
            {
                int rng = Random.Range(0, 100);
                //Debug.Log($"rng: {rng}, droprate: {values.materialDropRate[i] + values.materialRatePerLevel[i] * levelDifference}");
                if (rng < (values.materialDropRate[i] + values.materialRatePerLevel[i] * levelDifference))
                {
                    //Debug.Log($"Dropped material: {values.materials[i]}");
                    inventoryManager.AddMaterial(values.materials[i], 1);
                    var dropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity, dropItemParent);
                    dropItem.GetComponent<Rigidbody>().AddForce(new(Random.Range(-2f, 2f), 3f, 0), ForceMode.Impulse);
                    dropItem.GetComponent<Image>().sprite = materialSprites[i];
                    Destroy(dropItem, 3f);
                }
            }
        }

        //Handle drops
        if (values.handles.Length != 0)
        {
            for(int i = 0; i < values.handles.Length; i++)
            {
                int rng = Random.Range(0, 100);
                //Debug.Log($"rng: {rng}, droprate: {values.handleDropRate[i] + values.materialRatePerLevel[i] * levelDifference}");
                if (rng < (values.handleDropRate[i] + values.materialRatePerLevel[i] * levelDifference))
                {
                    inventoryManager.AddAssemblyItem(values.handles[i], 1);
                    var dropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity, dropItemParent);
                    dropItem.GetComponent<Rigidbody>().AddForce(new(Random.Range(-2f, 2f), 3f, 0), ForceMode.Impulse);
                    dropItem.GetComponent<Image>().sprite = values.handles[i].sprite;
                    Destroy(dropItem, 3f);
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}