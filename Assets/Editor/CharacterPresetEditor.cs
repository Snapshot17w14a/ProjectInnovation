using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(CharacterPreset))]
public class CharacterPresetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BeginHorizontal();

            serializedObject.FindProperty("Health").intValue = IntField("Health", serializedObject.FindProperty("Health").intValue);
            serializedObject.FindProperty("Health").intValue = IntField("Defense", serializedObject.FindProperty("Health").intValue);

        EndHorizontal();

        BeginHorizontal();

            serializedObject.FindProperty("Damage").intValue = IntField("Damage", serializedObject.FindProperty("Damage").intValue);
            serializedObject.FindProperty("AttackCooldown").floatValue = FloatField("Attack Cooldown", serializedObject.FindProperty("AttackCooldown").floatValue);

        EndHorizontal();

        PropertyField(serializedObject.FindProperty("Skill"));

        if (serializedObject.FindProperty("Skill").objectReferenceValue != null)
        {
            BeginHorizontal();

                serializedObject.FindProperty("SkillDamage").intValue = IntField("Skill Damage", serializedObject.FindProperty("SkillDamage").intValue);
                serializedObject.FindProperty("SkillCooldown").floatValue = FloatField("SkillCooldown", serializedObject.FindProperty("SkillCooldown").floatValue);

            EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
