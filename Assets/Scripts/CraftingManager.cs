using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InventoryManager))]
public class CraftingManager : Service
{
    [SerializeField] private string[] craftingScenes;
    [SerializeField] private int maxScorePerProcess = 5;
    private int craftingSceneIndex = -1;
    private Item craftingItem;

    public int MaxScorePerProcess => maxScorePerProcess;

    // Start is called before the first frame update
    protected override void Awake()
    {
        if (ServiceLocator.DoesServiceExist<CraftingManager>() && !ServiceLocator.CompareService(this))
        {
            DestroyImmediate(gameObject);
            return;
        }

        ServiceLocator.RegisterService(this);
        DontDestroyOnLoad(this);

        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCrafting()
    {
        craftingItem = new Item();
        StartCoroutine(CraftingProcesses());
    }

    private void CraftingDone()
    {
        var inventory = ServiceLocator.GetService<InventoryManager>();
        if (inventory != null) inventory.AddItemToInventory(craftingItem);
        inventory.SaveItems();
    }

    private IEnumerator CraftingProcesses()
    {
        while(craftingSceneIndex < craftingScenes.Length - 1)
        {
            yield return SceneManager.LoadSceneAsync(craftingScenes[++craftingSceneIndex]);
            var currentCraftingProcess = FindAnyObjectByType<ForgeHandler>();
            currentCraftingProcess.StartProcess(ref craftingItem);
            yield return new WaitUntil(() => currentCraftingProcess.IsProcessDone);
        }
        CraftingDone();
        yield return SceneManager.LoadSceneAsync("CraftingScene");
        yield return null;
    }
}
