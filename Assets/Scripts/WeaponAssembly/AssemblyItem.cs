using UnityEngine;

[CreateAssetMenu(fileName = "Assembly Item", menuName = "Scriptables/Assembly Item", order = 1)]
public class AssemblyItem : ScriptableObject
{
    public Sprite sprite;
    public string itemName;
}
