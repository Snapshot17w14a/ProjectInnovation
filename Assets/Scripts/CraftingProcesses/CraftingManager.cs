using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftingManager : Service
{
    [SerializeField] private string[] craftingScenes;
    [SerializeField] private int maxScorePerProcess = 5;
    private int craftingSceneIndex = -1;
    private Weapon craftingItem;

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

    public void StartCrafting()
    {
        craftingItem = new Weapon();
        StartCoroutine(CraftingProcesses());
    }

    private void CraftingDone()
    {
        var inventory = ServiceLocator.GetService<InventoryManager>();
        inventory.AddItemToInventory(craftingItem);
        inventory.SaveWeapons();
    }

    private IEnumerator CraftingProcesses()
    {
        while(craftingSceneIndex < craftingScenes.Length - 1)
        {
            yield return SceneManager.LoadSceneAsync(craftingScenes[++craftingSceneIndex]);
            var currentCraftingProcess = FindAnyObjectByType<CraftingProcess>().GetComponent<ICraftingProcess>();
            currentCraftingProcess.StartProcess(ref craftingItem);
            yield return new WaitUntil(() => currentCraftingProcess.IsProcessDone);
        }
        CraftingDone();
        yield return SceneManager.LoadSceneAsync("BattleScene");
        Destroy(gameObject);
        yield return null;
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterService(this);
    }
}

public abstract class CraftingProcess : MonoBehaviour { }
