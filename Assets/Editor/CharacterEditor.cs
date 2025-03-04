using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
    private bool isFoldedOut = true;
    private Editor presetObjectEditor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var presetObject = serializedObject.FindProperty("preset");
        PropertyField(presetObject);

        if(presetObject.objectReferenceValue != null)
        {
            isFoldedOut = Foldout(isFoldedOut, "Preset Object Settings");
            if (isFoldedOut)
            {
                if (presetObjectEditor == null || presetObjectEditor.target != presetObject.objectReferenceValue)
                {
                    CreateCachedEditor(presetObject.objectReferenceValue, null, ref presetObjectEditor);
                }

                if (presetObjectEditor != null)
                {
                    EditorGUI.indentLevel++;

                        presetObjectEditor.OnInspectorGUI();

                    EditorGUI.indentLevel--;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
