using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(LootTable))]
public class LootTableEditor : Editor
{
    Dictionary<Object, Editor> objectEditorPair = new();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        LabelField("Loot table settings", EditorStyles.boldLabel);

        var entryArray = serializedObject.FindProperty("entries");

        BeginHorizontal();

        entryArray.arraySize = Mathf.Max(0, IntField("Arr size", entryArray.arraySize));

        if (GUILayout.Button(new GUIContent("+"))) entryArray.arraySize++;
        if (GUILayout.Button(new GUIContent("-"))) entryArray.arraySize -= entryArray.arraySize != 0 ? 1 : 0;

        EndHorizontal();

        for (int i = 0; i < entryArray.arraySize; i++)
        {
            BeginVertical("Box");

            PropertyField(entryArray.GetArrayElementAtIndex(i), new GUIContent($"Table Entry {i + 1}"));

            //Object refValue = currentProperty.objectReferenceValue;

            //if (refValue == null)
            //{
            //    EndVertical();
            //    continue;
            //}

            //if (!objectEditorPair.ContainsKey(refValue))
            //{
            //    Editor createdEditor = null;
            //    CreateCachedEditor(refValue, typeof(LootTableEntryEditor), ref createdEditor);
            //    objectEditorPair.Add(refValue, createdEditor);
            //}

            //objectEditorPair[refValue].OnInspectorGUI();

            EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
