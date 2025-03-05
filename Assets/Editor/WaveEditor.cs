using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(EnemyWave))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var enemyWaveArray = serializedObject.FindProperty("waveEnemies");

        BeginHorizontal();
        LabelField("Array size");
        enemyWaveArray.arraySize = IntSlider(enemyWaveArray.arraySize, 0, 5);
        EndHorizontal();

        EditorGUI.indentLevel++;

        for (int i = 0; i < enemyWaveArray.arraySize; i++) 
        {
            PropertyField(enemyWaveArray.GetArrayElementAtIndex(i));
        }

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
