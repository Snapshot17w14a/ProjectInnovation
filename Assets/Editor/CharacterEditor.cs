using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(Character), true)]
public class CharacterEditor : Editor
{
    private bool isCharacterFoldedOut = true;
    private Editor presetObjectEditor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var presetObject = serializedObject.FindProperty("preset");
        PropertyField(presetObject);

        if(presetObject.objectReferenceValue != null)
        {
            isCharacterFoldedOut = Foldout(isCharacterFoldedOut, "Preset Object Settings");
            if (isCharacterFoldedOut)
            {
                if (presetObjectEditor == null || presetObjectEditor.target != presetObject.objectReferenceValue)
                {
                    CreateCachedEditor(presetObject.objectReferenceValue, typeof(CharacterPresetEditor), ref presetObjectEditor);
                }

                if (presetObjectEditor != null)
                {
                    EditorGUI.indentLevel++;

                        presetObjectEditor.OnInspectorGUI();

                    EditorGUI.indentLevel--;
                }
            }
        }

        else if (GUILayout.Button("Create Preset Object"))
        {
            CharacterPreset createdPreset = CreateInstance<CharacterPreset>();
            createdPreset.name = "WaveObject";

            presetObject.objectReferenceValue = createdPreset;

            AssetDatabase.CreateAsset(createdPreset, $"Assets/Scriptables/Presets/preset - ({createdPreset.GetInstanceID()}).asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
