using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(BattleManager))]
public class BattleManagerEditor : Editor
{
    Dictionary<Object, Editor> objectEditorPair = new();
    private bool isWaveListFoldedOut = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var waveList = serializedObject.FindProperty("enemyWaves");

        BeginHorizontal();
        isWaveListFoldedOut = Foldout(isWaveListFoldedOut, "Enemy Waves");
        waveList.arraySize = IntField(waveList.arraySize);
        if (GUILayout.Button("+")) waveList.arraySize++;
        if (GUILayout.Button("-") && waveList.arraySize != 0) waveList.arraySize--; 
        EndHorizontal();

        if (isWaveListFoldedOut)
        {
            for (int i = 0; i < waveList.arraySize; i++)
            {
                DrawListElement(waveList.GetArrayElementAtIndex(i));
            }
        }

        if (GUILayout.Button("Add Wave"))
        {
            EnemyWave createdWaveObject = CreateInstance<EnemyWave>();
            createdWaveObject.name = "WaveObject";

            AssetDatabase.CreateAsset(createdWaveObject, $"Assets/Scriptables/Waves/Wave - ({createdWaveObject.GetInstanceID()}).asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            waveList.arraySize++;
            waveList.GetArrayElementAtIndex(waveList.arraySize - 1).objectReferenceValue = createdWaveObject;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawListElement(SerializedProperty element)
    {
        BeginVertical("Box");

        PropertyField(element, new GUIContent("Scriptable"));

        Object refValue = element.objectReferenceValue;

        if (refValue == null)
        {
            EndVertical();
            return;
        }

        if (!objectEditorPair.ContainsKey(refValue))
        {
            Editor createdEditor = null;
            CreateCachedEditor(refValue, typeof(WaveEditor), ref createdEditor);
            objectEditorPair.Add(refValue, createdEditor);
        }

        objectEditorPair[refValue].OnInspectorGUI();

        EndVertical();
    }
}
