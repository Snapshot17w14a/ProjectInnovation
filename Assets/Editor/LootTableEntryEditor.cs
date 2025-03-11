using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;

[CustomPropertyDrawer(typeof(LootTableEntry))]
public class LootTableEntryEditor : PropertyDrawer
{
    private readonly Dictionary<string, bool[]> foldoutStates = new();
    private readonly Dictionary<string, int> nextLineCount = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var materialArray = property.FindPropertyRelative("materials");
        var materialDropRate = property.FindPropertyRelative("materialDropRate");
        var materialRatePerLevel = property.FindPropertyRelative("materialRatePerLevel");

        var handleArray = property.FindPropertyRelative("handles");
        var handleDropRate = property.FindPropertyRelative("handleDropRate");
        var handleRatePerLevel = property.FindPropertyRelative("handleRatePerLevel");

        BeginProperty(position, label, property);

        var propertyPath = property.propertyPath;
        if (!foldoutStates.ContainsKey(propertyPath))
        {
            foldoutStates[propertyPath] = new bool[] { true, true, true };
        }

        if (!nextLineCount.ContainsKey(propertyPath))
        {
            nextLineCount[propertyPath] = 0;
        }
        else nextLineCount[propertyPath] = 0;

        var rectPosition = new Rect(position.x, position.y, position.width, singleLineHeight);

        var level = property.FindPropertyRelative("fromLevel");
        level.intValue = IntField(rectPosition, "From level", level.intValue);

        NextLine(ref rectPosition, propertyPath);

        #region MaterialDrops

        rectPosition.width *= 0.5f;
        foldoutStates[propertyPath][0] = Foldout(rectPosition, foldoutStates[propertyPath][0], "Material drops");
        rectPosition.x += rectPosition.width;
        materialArray.arraySize = Mathf.Max(0, IntField(rectPosition, "Drops count", materialArray.arraySize));
        materialDropRate.arraySize = materialArray.arraySize;
        materialRatePerLevel.arraySize = materialArray.arraySize;

        rectPosition.x -= rectPosition.width;
        rectPosition.width *= 2f;
        NextLine(ref rectPosition, propertyPath);

        if (foldoutStates[propertyPath][0])
        {
            for (int i = 0; i < materialArray.arraySize; i++)
            {
                PropertyField(rectPosition, materialArray.GetArrayElementAtIndex(i), new GUIContent("Material to drop"));
                NextLine(ref rectPosition, propertyPath);
                rectPosition.width *= 0.5f;
                materialDropRate.GetArrayElementAtIndex(i).intValue = IntField(rectPosition, "Drop%", materialDropRate.GetArrayElementAtIndex(i).intValue);
                rectPosition.x += rectPosition.width;
                materialRatePerLevel.GetArrayElementAtIndex(i).intValue = IntField(rectPosition, "+% per level", materialRatePerLevel.GetArrayElementAtIndex(i).intValue);
                NextLine(ref rectPosition, propertyPath);
                rectPosition.x -= rectPosition.width;
                rectPosition.width *= 2f;
            }
        }

        #endregion

        #region HandleDrops

        rectPosition.width *= 0.5f;
        foldoutStates[propertyPath][2] = Foldout(rectPosition, foldoutStates[propertyPath][2], "Handle drops");
        rectPosition.x += rectPosition.width;
        handleArray.arraySize = Mathf.Max(0, IntField(rectPosition, "Drops count", handleArray.arraySize));
        handleDropRate.arraySize = handleArray.arraySize;
        handleRatePerLevel.arraySize = handleArray.arraySize;

        rectPosition.x -= rectPosition.width;
        rectPosition.width *= 2f;
        NextLine(ref rectPosition, propertyPath);

        if (foldoutStates[propertyPath][2])
        {
            for (int i = 0; i < handleArray.arraySize; i++)
            {
                ObjectField(rectPosition, handleArray.GetArrayElementAtIndex(i), new GUIContent("Material to drop"));
                NextLine(ref rectPosition, propertyPath);
                rectPosition.width *= 0.5f;
                handleDropRate.GetArrayElementAtIndex(i).intValue = IntField(rectPosition, "Drop%", handleDropRate.GetArrayElementAtIndex(i).intValue);
                rectPosition.x += rectPosition.width;
                handleRatePerLevel.GetArrayElementAtIndex(i).floatValue = FloatField(rectPosition, "+% per level", handleRatePerLevel.GetArrayElementAtIndex(i).floatValue);
                NextLine(ref rectPosition, propertyPath);
                rectPosition.x -= rectPosition.width;
                rectPosition.width *= 2f;
            }
        }

        #endregion
    }

    private void NextLine(ref Rect rect, string path)
    {
        rect.y += singleLineHeight + standardVerticalSpacing;
        nextLineCount[path]++;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int multiplier = nextLineCount.ContainsKey(property.propertyPath) ? nextLineCount[property.propertyPath] : 1;
        return singleLineHeight * multiplier + standardVerticalSpacing * (multiplier - 1);
    }
}
