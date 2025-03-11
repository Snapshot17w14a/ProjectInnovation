using UnityEngine;

[CreateAssetMenu(fileName = "BattleContainer", menuName = "Scriptables/Battle Container")]
[System.Serializable]
public class Battle : ScriptableObject
{
    public EnemyWave[] enemyWaves;
}
