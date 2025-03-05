using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyWave[] enemyWaves;
    private bool isBattleInProgress = false;
    private int currentWaveIndex = 0;

    private readonly Pet[] petsInBattle = new Pet[3];
    private readonly Enemy[] enemiesInBattle = new Enemy[5];

    private readonly Vector2[] enemyPositions = new Vector2[5];
    private Transform enemyParent;

    public bool IsAttackingAllowed => isBattleInProgress;

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
        Transform petParent = characterParet.Find("Pets");
        enemyParent = characterParet.Find("Enemies");

        for(int i = 0; i < 3; i++) if (petParent.GetChild(i).TryGetComponent<Pet>(out petsInBattle[i])) petsInBattle[i].SetManager(this);
        for(int i = 0; i < 5; i++)
        {
            var enemyTransform = enemyParent.GetChild(i);
            enemyPositions[i] = enemyTransform.position;
        }
    }

    private IEnumerator StartWaves()
    {
        Debug.Log("Coroutine started");

        while (currentWaveIndex < enemyWaves.Length)
        {
            enemyWaves[currentWaveIndex].Initialize();
            Debug.Log($"Wave index: {currentWaveIndex}, number of enemies: {enemyWaves[currentWaveIndex].waveEnemies.Length}");

            for (int i = 0; i < enemyWaves[currentWaveIndex].waveEnemies.Length; i++)
            {
                CreateEnemy(enemyWaves[currentWaveIndex].waveEnemies[i], i);
            }

            isBattleInProgress = true;

            Debug.Log($"Battle started, waiting for enemies to die");
            yield return new WaitUntil(() => EnemiesAlive == 0);
            Debug.Log($"Battle ended, enemies dead");

            isBattleInProgress = false;
            currentWaveIndex++;
        }

        Debug.Log($"All waves beat");
        OnBattleEnd?.Invoke();
        yield return null;
    }

    public void StartBattle()
    {
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
        Debug.Log($"Creating enemy with index: {slotIndex}");
        enemiesInBattle[slotIndex] = Instantiate<Enemy>(enemy, enemyPositions[slotIndex], Quaternion.identity, enemyParent);
        enemiesInBattle[slotIndex].SetManager(this);
    }
}
