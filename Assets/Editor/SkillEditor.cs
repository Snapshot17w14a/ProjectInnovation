using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Skill))]
class SkillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BeginHorizontal();

        serializedObject.FindProperty("SkillDamage").intValue = IntField("Skill Damage", serializedObject.FindProperty("SkillDamage").intValue);
        serializedObject.FindProperty("SkillCooldown").floatValue = FloatField("SkillCooldown", serializedObject.FindProperty("SkillCooldown").floatValue);

        EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
