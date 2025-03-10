using UnityEngine;

public class PetSelector : MonoBehaviour
{
    [SerializeField] private GameObject petSelectorCanvas;
    [SerializeField] private PetSelectorManager selectorManager;

    private static BattleManager battleManager;

    private void Start()
    {
        if (battleManager == null) battleManager = FindObjectOfType<BattleManager>();
    }

    public void SelectSlot(int index)
    {
        petSelectorCanvas.SetActive(true);
        PetSelectorManager.SlotIndex = index;
        PetSelectorManager.ReturnFunction = battleManager.SetPetAtIndex;
    }
}
