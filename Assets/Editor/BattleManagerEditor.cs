using UnityEditor;

[CustomEditor(typeof(BattleManager))]
public class BattleManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyWaves"));

        serializedObject.ApplyModifiedProperties();
    }
}
