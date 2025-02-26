using UnityEditor;
[CustomEditor(typeof(EnemyWave))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var enemyWaveArray = serializedObject.FindProperty("waveEnemies");

        for (int i = 0; i < enemyWaveArray.arraySize; i++) 
        {
            EditorGUILayout.PropertyField(enemyWaveArray.GetArrayElementAtIndex(i));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
