using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> canvases = new();

    public void OpenInventoryPanel()
    {
        foreach(var gameobject in canvases)
        {
            if (gameobject.name != "Inventory") continue;
            gameobject.SetActive(true);
            //gameobject.GetComponent<InventoryDisplayer>().UpdateContent();
        }
    }

    public void CloseAllOverlays()
    {
        foreach(var gameobject in canvases)
        {
            gameobject.SetActive(false);
        }
    }

    public void OpenCanvasWithName(string name)
    {
        foreach (var gameobject in canvases)
        {
            if (!gameobject.name.Contains(name)) gameobject.SetActive(false);
            else
            {
                gameobject.SetActive(true);
                gameobject.TryGetComponent(out InventoryDisplayer inventoryDisplayer);
                if(inventoryDisplayer != null) inventoryDisplayer.UpdateContent();
            }
        }
    }

    public void StartCraftingProcess()
    {
        SceneManager.LoadScene("CraftingScene");
    }
}
