using System.Collections;
using UnityEngine;

[System.Serializable]
public class BattleManager : MonoBehaviour
{
    [SerializeField] EnemyWave[] enemyWaves;
    private bool isBattleInProgress = false;
    private int currentWaveIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemyWave in enemyWaves) enemyWave.Initialize();
        isBattleInProgress = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
