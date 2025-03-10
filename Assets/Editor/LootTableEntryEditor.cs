//using UnityEditor;
//using static UnityEditor.EditorGUILayout;

//[CustomEditor(typeof(LootTableEntry))]
//public class LootTableEntryEditor : Editor
//{
//    private bool[] foldoutBools = { true, true, true };

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        serializedObject.FindProperty("fromLevel").intValue = IntField("From x Level", serializedObject.FindProperty("fromLevel").intValue);

//        var materialArray = serializedObject.FindProperty("materials");
//        var materialDropRate = serializedObject.FindProperty("materialDropRate");
//        var materialRatePerLevel = serializedObject.FindProperty("materialRatePerLevel");

//        BeginHorizontal();

//            foldoutBools[0] = Foldout(foldoutBools[0], "Material drop settings");
//            materialArray.arraySize = IntField("Drops count", materialArray.arraySize);
//            materialDropRate.arraySize = materialArray.arraySize;
//            materialRatePerLevel.arraySize = materialArray.arraySize;

//        EndHorizontal();

//        for(int i = 0; i < materialArray.arraySize; i++)
//        {
//            PropertyField(materialArray.GetArrayElementAtIndex(i));
//            PropertyField(materialDropRate.GetArrayElementAtIndex(i));
//            PropertyField(materialRatePerLevel.GetArrayElementAtIndex(i));
//        }

//        serializedObject.ApplyModifiedProperties();
//    }
//}
