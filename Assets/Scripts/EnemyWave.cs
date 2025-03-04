using UnityEngine;

[System.Serializable]
public class EnemyWave : ScriptableObject
{
    [SerializeField] private Enemy[] waveEnemies;
    private int maxEnemyCount;
    public int RemainingEnemies => maxEnemyCount - waveEnemies.Length;

    public void Initialize()
    {
        maxEnemyCount = waveEnemies.Length;
    }

    public void StartWave()
    {

    }
}