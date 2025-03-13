using UnityEngine.UI;
using UnityEngine;

public class AssemblyManager : CraftingProcess, ICraftingProcess
{
    [SerializeField] private GameObject assemblySnapPoint;
    [SerializeField] private GameObject weapon;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField][Range(1,5)] private int gridDimension;

    [SerializeField] private SoundEffectPlayer soundPlayer;

    private Weapon assemblyItem;

    private bool isAssemblyDone;
    public bool IsProcessDone => isAssemblyDone;

    // Start is called before the first frame update
    void Start()
    {
        CalculateMaxCellSize();
        //weapon = transform.Find("Weapon").gameObject;
        var snapObject = weapon.transform.Find("Snappoints");
        //assemblySnapPoint = snapObject.GetChild(0).gameObject;
    }

    public void StartProcess(ref Weapon item)
    {
        assemblyItem = item;
        PopulateContent();
    }

    private void CalculateMaxCellSize()
    {
        var width = scrollView.GetComponent<RectTransform>().rect.width - scrollView.verticalScrollbar.GetComponent<RectTransform>().rect.width;
        var gridLayout = scrollView.content.GetComponent<GridLayoutGroup>();
        var spacing = gridLayout.spacing.x;
        width -= spacing * 2 + spacing * (gridDimension - 1);
        width /= gridDimension;
        gridLayout.cellSize = new(width, width);
    }

    private void CreateButtonForItem(AssemblyItem item)
    {
        var button = new GameObject(item.itemName, typeof(CanvasRenderer), typeof(Image), typeof(Button));
        button.transform.SetParent(scrollView.content, false);
        var icon = new GameObject(item.itemName + "'s icon", typeof(CanvasRenderer), typeof(Image));
        icon.transform.SetParent(button.transform, false);
        icon.GetComponent<Image>().sprite = item.sprite;
        button.GetComponent<Button>().onClick.AddListener(() => AddItem(item));
        button.GetComponent<Button>().onClick.AddListener(() => soundPlayer.PlayAudio());
    }

    public void AddItem(AssemblyItem item)
    {
        //ServiceLocator.GetService<InventoryManager>().AddAssemblyItem(item, -1);
        var itemObject = new GameObject(item.itemName, typeof(CanvasRenderer), typeof(Image));
        itemObject.GetComponent<Image>().sprite = item.sprite;
        itemObject.transform.position = assemblySnapPoint.transform.position;
        itemObject.transform.SetParent(weapon.transform, true);
        itemObject.GetComponent<RectTransform>().sizeDelta = assemblySnapPoint.GetComponent<RectTransform>().sizeDelta;
        itemObject.GetComponent<RectTransform>().localRotation = assemblySnapPoint.GetComponent<RectTransform>().localRotation;
        assemblyItem.SetGrip(item);
        isAssemblyDone = true;
    }

    private void PopulateContent()
    {
        int contentChildCount = scrollView.content.childCount;
        for(int i = contentChildCount - 1; i >= 0 ; i--)
        {
            DestroyImmediate(scrollView.content.GetChild(i).gameObject);
        }

        var inventoryManager = ServiceLocator.GetService<InventoryManager>();
        foreach (var item in inventoryManager.AssemblyItems)
        {
            if(inventoryManager.GetAssemblyItemCount(item) > 0) CreateButtonForItem(item);
        }
    }
}
