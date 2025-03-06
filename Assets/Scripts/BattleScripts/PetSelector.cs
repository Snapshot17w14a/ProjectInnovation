using UnityEngine;

public class PetSelector : MonoBehaviour
{
    [SerializeField] private GameObject petSelectorCanvas;
    [SerializeField] private PetSelectorManager selectorManager;

    public void SelectSlot(int index)
    {
        petSelectorCanvas.SetActive(true);
        selectorManager.SlotIndex = index;
    }
}
