using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(BattleManager))]
public class BattleManagerEditor : Editor
{
    private Editor battleCachedEditor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var container = serializedObject.FindProperty("battleContainer");

        PropertyField(container, new GUIContent("Battle Container"));
        
        var refValue = container.objectReferenceValue;

        if (refValue != null && (battleCachedEditor == null || battleCachedEditor.target != refValue))
        {
            CreateCachedEditor(refValue, typeof(BattleEditor), ref battleCachedEditor);
        }

        if (battleCachedEditor != null) battleCachedEditor.OnInspectorGUI();

        LabelField("Other Settings", EditorStyles.boldLabel);

        PropertyField(serializedObject.FindProperty("objectsToHideInBattle"));

        PropertyField(serializedObject.FindProperty("fightButton"));

        PropertyField(serializedObject.FindProperty("usePotionButton"));

        PropertyField(serializedObject.FindProperty("soundPlayer"));

        serializedObject.ApplyModifiedProperties();
    }
}
