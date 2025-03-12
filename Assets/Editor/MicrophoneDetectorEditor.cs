using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(ForgeHandler))]
public class MicrophoneDetectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var minScoreThresholdVal = System.MathF.Round(serializedObject.FindProperty("minScoreThreshold").floatValue, 2);
        var maxScoreThresholdVal = System.MathF.Round(serializedObject.FindProperty("maxScoreThreshold").floatValue, 2);

        LabelField("Interaction settings");

        PropertyField(serializedObject.FindProperty("microphoneThreshold"));
        PropertyField(serializedObject.FindProperty("increaseAmount"));
        PropertyField(serializedObject.FindProperty("decreaseAmount"));
        PropertyField(serializedObject.FindProperty("activeIncrease"));
        PropertyField(serializedObject.FindProperty("activeDecrease"));
        PropertyField(serializedObject.FindProperty("allowedTime"));

        LabelField("Score increase threshold");

        BeginHorizontal();

        LabelField("Min: " + minScoreThresholdVal);
        LabelField("Max: " + maxScoreThresholdVal);

        EndHorizontal();

        MinMaxSlider(ref minScoreThresholdVal, ref maxScoreThresholdVal, 0, 1);

        serializedObject.FindProperty("minScoreThreshold").floatValue = minScoreThresholdVal;
        serializedObject.FindProperty("maxScoreThreshold").floatValue = maxScoreThresholdVal;

        serializedObject.ApplyModifiedProperties();
    }
}
