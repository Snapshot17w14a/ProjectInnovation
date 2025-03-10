using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyWave[] enemyWaves;
    private bool isBattleInProgress = false;
    private int currentWaveIndex = 0;

    private readonly Enemy[] enemiesInBattle = new Enemy[5];
    private readonly Pet[] petsInBattle = new Pet[3];

    private readonly Vector2[] enemyPositions = new Vector2[5];
    private Transform enemyParent;

    private readonly Vector2[] petPositions = new Vector2[3];
    private Transform petParent;

    [SerializeField] private GameObject[] objectsToHideInBattle;

    public bool IsAttackingAllowed => isBattleInProgress;
    public Enemy[] AllEnemies => enemiesInBattle;

    private int EnemiesAlive
    {
        get
        {
            int count = 0;
            foreach (var enemy in enemiesInBattle) if (enemy != null) count++;
            return count;
        }
    }

    public Action OnBattleEnd;

    // Start is called before the first frame update
    void Start()
    {
        Transform characterParet = transform.Find("Characters");
        petParent = characterParet.Find("Pets");
        enemyParent = characterParet.Find("Enemies");

        //for(int i = 0; i < 3; i++) if (petParent.GetChild(i).TryGetComponent<Pet>(out petsInBattle[i])) petsInBattle[i].SetManager(this);
        for (int i = 0; i < 3; i++)
        {
            var petTransform = petParent.GetChild(i);
            petPositions[i] = petTransform.position;
            Destroy(petTransform.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            var enemyTransform = enemyParent.GetChild(i);
            enemyPositions[i] = enemyTransform.position;
            Destroy(enemyTransform.gameObject);
        }

        Skill.battleManager = this;
    }

    private IEnumerator StartWaves()
    {
        //Debug.Log("Coroutine started");

        while (currentWaveIndex < enemyWaves.Length)
        {
            enemyWaves[currentWaveIndex].Initialize();
            //Debug.Log($"Wave index: {currentWaveIndex}, number of enemies: {enemyWaves[currentWaveIndex].waveEnemies.Length}");

            for (int i = 0; i < enemyWaves[currentWaveIndex].waveEnemies.Length; i++)
            {
                CreateEnemy(enemyWaves[currentWaveIndex].waveEnemies[i], i);
            }

            isBattleInProgress = true;

            //Debug.Log($"Battle started, waiting for enemies to die");
            yield return new WaitUntil(() => EnemiesAlive == 0);
            //Debug.Log($"Battle ended, enemies dead");

            isBattleInProgress = false;
            currentWaveIndex++;
        }

        //Debug.Log($"All waves beat");
        OnBattleEnd?.Invoke();
        yield return null;
    }

    public void StartBattle()
    {
        foreach (var gameObject in objectsToHideInBattle) gameObject.SetActive(false);
        StartCoroutine(StartWaves());
    }

    public Character GetTargetCharacter(Character.CharacterType type)
    {
        int i = -1;
        if (type == Character.CharacterType.Enemy)
        {
            while (i + 1 < petsInBattle.Length && petsInBattle[++i] == null) { }
            return petsInBattle[i];
        }
        else if (type == Character.CharacterType.Pet)
        {
            while (i + 1 < enemiesInBattle.Length && enemiesInBattle[++i] == null) { }
            return enemiesInBattle[i];
        }
        return null;
    }

    private void CreateEnemy(Enemy enemy, int slotIndex)
    {
        if (enemy == null) return;
        //Debug.Log($"Creating enemy with index: {slotIndex}");
        enemiesInBattle[slotIndex] = Instantiate<Enemy>(enemy, enemyPositions[slotIndex], Quaternion.identity, enemyParent);
        enemiesInBattle[slotIndex].SetManager(this);
    }

    public void SetPetAtIndex(int index, Pet pet)
    {
        if (petsInBattle[index] != null) DestroyImmediate(petsInBattle[index].gameObject);
        petsInBattle[index] = pet;
        pet.transform.SetParent(petParent);
        pet.transform.SetSiblingIndex(index);
        pet.transform.position = petPositions[index];
        pet.SetManager(this);
    }
}
