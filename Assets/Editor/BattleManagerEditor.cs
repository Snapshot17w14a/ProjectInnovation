using UnityEditor;

[CustomEditor(typeof(BattleManager))]
public class BattleManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var wavesArray = serializedObject.FindProperty("enemyWaves");

        for (int i = 0; i < wavesArray.arraySize; i++) 
        {
            EditorGUILayout.PropertyField(wavesArray.GetArrayElementAtIndex(i));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
