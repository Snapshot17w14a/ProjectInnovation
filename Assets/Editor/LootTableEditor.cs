//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using static UnityEditor.EditorGUILayout;

//[CustomEditor(typeof(LootTable))]
//public class LootTableEditor : Editor
//{
//    Dictionary<Object, Editor> objectEditorPair = new();

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        LabelField("Loot table settings", EditorStyles.boldLabel);

//        var entryArray = serializedObject.FindProperty("entries");
//        entryArray.arraySize = IntField("Arr size", entryArray.arraySize);

//        for (int i = 0; i < entryArray.arraySize; i++)
//        {
//            var currentProperty = entryArray.GetArrayElementAtIndex(i);

//            BeginVertical("Box");

//            PropertyField(currentProperty, new GUIContent("Scriptable"));

//            Object refValue = currentProperty.objectReferenceValue;

//            if (refValue == null)
//            {
//                EndVertical();
//                continue;
//            }

//            if (!objectEditorPair.ContainsKey(refValue))
//            {
//                Editor createdEditor = null;
//                CreateCachedEditor(refValue, typeof(LootTableEntryEditor), ref createdEditor);
//                objectEditorPair.Add(refValue, createdEditor);
//            }

//            objectEditorPair[refValue].OnInspectorGUI();

//            EndVertical();
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
