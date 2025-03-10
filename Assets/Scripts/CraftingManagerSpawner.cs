using UnityEngine;
using UnityEngine.UI;

public class CraftingManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject managerPrefab;

    [SerializeField] private Button startButton;

    // Start is called before the first frame update
    void Awake()
    {
        var manager = FindObjectOfType<CraftingManager>();
        if (manager == null)
        {
            manager = Instantiate(managerPrefab).GetComponent<CraftingManager>();
            startButton.onClick.AddListener(manager.StartCrafting);
        }
    }
}
