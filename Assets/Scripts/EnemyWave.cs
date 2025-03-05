using UnityEngine;

[System.Serializable]
public class EnemyWave : ScriptableObject
{
    [SerializeField] public Enemy[] waveEnemies;
    private int maxEnemyCount;
    public int RemainingEnemies => maxEnemyCount;

    public void Initialize()
    {
        maxEnemyCount = waveEnemies.Length;
    }
}