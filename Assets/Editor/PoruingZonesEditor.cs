using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PouringZone))]
public class PouringZonesEditor : Editor
{
    private SerializedProperty goodMinProp;
    private SerializedProperty goodMaxProp;
    private SerializedProperty averageMinProp;
    private SerializedProperty averageMaxProp;

    private void OnEnable()
    {
        goodMinProp = serializedObject.FindProperty("goodMin");
        goodMaxProp = serializedObject.FindProperty("goodMax");
        averageMinProp = serializedObject.FindProperty("averageMin");
        averageMaxProp = serializedObject.FindProperty("averageMax");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grading Ranges", EditorStyles.boldLabel);

        // MinMax Slider for Good Range
        float goodMin = goodMinProp.floatValue;
        float goodMax = goodMaxProp.floatValue;
        EditorGUILayout.MinMaxSlider("Good Range", ref goodMin, ref goodMax, 0, 10);
        goodMinProp.floatValue = goodMin;
        goodMaxProp.floatValue = goodMax;

        // MinMax Slider for Average Range
        float avgMin = averageMinProp.floatValue;
        float avgMax = averageMaxProp.floatValue;
        EditorGUILayout.MinMaxSlider("Average Range", ref avgMin, ref avgMax, 0, 10);
        averageMinProp.floatValue = avgMin;
        averageMaxProp.floatValue = avgMax;

        serializedObject.ApplyModifiedProperties();
    }
}
