using UnityEngine.UI;
using UnityEngine;

public class AssemblyManager : CraftingProcess, ICraftingProcess
{
    [SerializeField] private Image handleVisual;
    [SerializeField] private Image bladeVisual;

    [SerializeField] private GameObject handleTilePrefab;
    [SerializeField] private Transform tileParent;

    [SerializeField] private SoundEffectPlayer soundPlayer;

    [SerializeField] private Sprite[] blades;

    private Weapon craftingWeapon;

    private bool isAssemblyDone;
    public bool IsProcessDone => isAssemblyDone;

    // Start is called before the first frame update
    void Start()
    {
        //CalculateMaxCellSize();
        //weapon = transform.Find("Weapon").gameObject;
        //var snapObject = weapon.transform.Find("Snappoints");
        //assemblySnapPoint = snapObject.GetChild(0).gameObject;
        PopulateContent();
    }

    public void StartProcess(ref Weapon item)
    {
        craftingWeapon = item;
        bladeVisual.sprite = blades[(int)item.ItemMaterial];
        PopulateContent();
    }

    //private void CalculateMaxCellSize()
    //{
    //    var width = scrollView.GetComponent<RectTransform>().rect.width - scrollView.verticalScrollbar.GetComponent<RectTransform>().rect.width;
    //    var gridLayout = scrollView.content.GetComponent<GridLayoutGroup>();
    //    var spacing = gridLayout.spacing.x;
    //    width -= spacing * 2 + spacing * (gridDimension - 1);
    //    width /= gridDimension;
    //    gridLayout.cellSize = new(width, width);
    //}

    //private void CreateButtonForItem(AssemblyItem item)
    //{
    //    var button = new GameObject(item.itemName, typeof(CanvasRenderer), typeof(Image), typeof(Button));
    //    button.transform.SetParent(scrollView.content, false);
    //    var icon = new GameObject(item.itemName + "'s icon", typeof(CanvasRenderer), typeof(Image));
    //    icon.transform.SetParent(button.transform, false);
    //    icon.GetComponent<Image>().sprite = item.sprite;
    //    button.GetComponent<Button>().onClick.AddListener(() => AddItem(item));
    //    button.GetComponent<Button>().onClick.AddListener(() => soundPlayer.PlayAudio());
    //}

    //public void AddItem(AssemblyItem item)
    //{
    //    //ServiceLocator.GetService<InventoryManager>().AddAssemblyItem(item, -1);
    //    var itemObject = new GameObject(item.itemName, typeof(CanvasRenderer), typeof(Image));
    //    itemObject.GetComponent<Image>().sprite = item.sprite;
    //    itemObject.transform.position = assemblySnapPoint.transform.position;
    //    itemObject.transform.SetParent(weapon.transform, true);
    //    itemObject.GetComponent<RectTransform>().sizeDelta = assemblySnapPoint.GetComponent<RectTransform>().sizeDelta;
    //    itemObject.GetComponent<RectTransform>().localRotation = assemblySnapPoint.GetComponent<RectTransform>().localRotation;
    //    assemblyItem.SetGrip(item);
    //    isAssemblyDone = true;
    //}

    private void AddItem(AssemblyItem item)
    {
        handleVisual.sprite = item.sprite;
        handleVisual.color = Color.white;
        craftingWeapon.SetGrip(item);
        isAssemblyDone = true;
    }

    private void PopulateContent()
    {
        var inventoryManager = ServiceLocator.GetService<InventoryManager>();
        foreach (var item in inventoryManager.AssemblyItems)
        {
            if(inventoryManager.GetAssemblyItemCount(item) > 0)
            {
                var tile = Instantiate(handleTilePrefab, tileParent);
                tile.transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
                tile.GetComponent<Button>().onClick.AddListener(() => AddItem(item));
                tile.GetComponent<Button>().onClick.AddListener(() => soundPlayer.PlayAudio());
            }
        }
    }
}
