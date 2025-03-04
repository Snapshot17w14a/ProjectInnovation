using System.Collections;
using UnityEngine;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyWave[] enemyWaves;
    private bool isBattleInProgress = false;
    private int currentWaveIndex = -1;

    private readonly Pet[] petsInBattle = new Pet[3];
    private readonly Enemy[] enemiesInBattle = new Enemy[3];

    public bool IsAttackingAllowed => isBattleInProgress;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemyWave in enemyWaves) enemyWave.Initialize();

        for(int i = 0; i < 3; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Pet>(out petsInBattle[i])) petsInBattle[i].SetManager(this);
            if (transform.GetChild(i + 3).TryGetComponent<Enemy>(out enemiesInBattle[i])) enemiesInBattle[i].SetManager(this);
        }
    }

    private IEnumerator StartWaves()
    {
        while (isBattleInProgress)
        {
            if (currentWaveIndex + 1 == enemyWaves.Length) yield return null;
            yield return new WaitUntil(() =>
            {
                return enemyWaves[currentWaveIndex].RemainingEnemies == 0;
            });

            enemyWaves[++currentWaveIndex].StartWave();
        }
    }

    public void StartBattle()
    {
        isBattleInProgress = true;
    }

    public Character GetTargetCharacter(Character.CharacterType type)
    {
        if (type == Character.CharacterType.Enemy)
        {
            if (petsInBattle[0] != null) return petsInBattle[0];
            int index = Random.Range(1, 3);
            if (petsInBattle[index] != null) return petsInBattle[index];
            else return petsInBattle[index == 1 ? 2 : 1];
        }
        else if (type == Character.CharacterType.Pet)
        {
            if (enemiesInBattle[0] != null) return enemiesInBattle[0];
            int index = Random.Range(1, 3);
            if (enemiesInBattle[index] != null) return enemiesInBattle[index];
            else return enemiesInBattle[index == 1 ? 2 : 1];
        }
        return null;
    }
}
