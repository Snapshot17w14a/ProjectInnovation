using UnityEditor;
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

        var skillField = serializedObject.FindProperty("Skill");
        PropertyField(skillField);

        if (skillField.objectReferenceValue != null)
        {
            EditorGUI.indentLevel++;

            isSkillFoldedOut = Foldout(isSkillFoldedOut, "Skill parameters");
            if (isSkillFoldedOut)
            {
                if (skillEditor == null || skillEditor.target != skillField.objectReferenceValue)
                {
                    CreateCachedEditor(skillField.objectReferenceValue, typeof(SkillEditor), ref skillEditor);
                }

                if (skillEditor != null)
                {
                    skillEditor.OnInspectorGUI();
                }
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
