[System.Serializable]
public class EnemyWave
{
    public Enemy[] waveEnemies;
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
