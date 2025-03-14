using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionInventoryDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private UIManager uiManager;

    private readonly List<GameObject> createdTiles = new();

    public void UpdateContent()
    {
        foreach (var tile in createdTiles) Destroy(tile);
        createdTiles.Clear();

        var inventoryManager = ServiceLocator.GetService<InventoryManager>();

        foreach(var potion in inventoryManager.GetAllPotions)
        {
            var tile = Instantiate(tilePrefab, tileParent).transform;

            tile.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{potion.Type} Potion";
            tile.GetChild(2).GetComponent<TextMeshProUGUI>().text = potion.Description;
            tile.GetChild(3).GetComponent<Button>().onClick.AddListener(() => battleManager.SetPotion(potion));
            tile.GetChild(3).GetComponent<Button>().onClick.AddListener(() => uiManager.CloseAllOverlays());

            createdTiles.Add(tile.gameObject);
        }
    }
}
