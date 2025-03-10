using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(CharacterPreset))]
public class CharacterPresetEditor : Editor
{
    private bool isSkillFoldedOut = true;
    private Editor skillEditor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BeginHorizontal();

            serializedObject.FindProperty("Health").intValue = IntField("Health", serializedObject.FindProperty("Health").intValue);
            serializedObject.FindProperty("Defense").intValue = IntField("Defense", serializedObject.FindProperty("Defense").intValue);

        EndHorizontal();

        BeginHorizontal();

            serializedObject.FindProperty("Damage").intValue = IntField("Damage", serializedObject.FindProperty("Damage").intValue);
            serializedObject.FindProperty("AttackCooldown").floatValue = FloatField("Attack Cooldown", serializedObject.FindProperty("AttackCooldown").floatValue);

        EndHorizontal();

        var skillProperty = serializedObject.FindProperty("Skill");
        PropertyField(skillProperty);

        if (skillProperty.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;

            isSkillFoldedOut = Foldout(isSkillFoldedOut, "Skill parameters");
            if (isSkillFoldedOut)
            {
                if (skillEditor == null || skillEditor.target != skillProperty.objectReferenceValue)
                {
                    CreateCachedEditor(skillProperty.objectReferenceValue, typeof(SkillEditor), ref skillEditor);
                }

                if (skillEditor != null)
                {
                    skillEditor.OnInspectorGUI();
                }
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.FindProperty("StartingLevel").intValue = IntField("Starting Level", serializedObject.FindProperty("StartingLevel").intValue);

        BeginHorizontal();

            serializedObject.FindProperty("HpPerLevel").intValue = IntField("HpPerLevel", serializedObject.FindProperty("HpPerLevel").intValue);
            serializedObject.FindProperty("DmgPerLevel").intValue = IntField("DmgPerLevel", serializedObject.FindProperty("DmgPerLevel").intValue);

        EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
