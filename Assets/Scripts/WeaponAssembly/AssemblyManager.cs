using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class AssemblyManager : MonoBehaviour
{
    private GameObject[] assemblySnapPoints;
    private GameObject weapon;
    int currentSnapIndex = 0;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField][Range(1,5)] private int gridDimension;

    private Dictionary<string, List<AssemblyItem>> typeInstancePair = new();

    private string[] tempAssemblyOrder = { "Grips" };

    private void Awake()
    {
        CalculateMaxCellSize();

        string[] itemTypes = Directory.GetDirectories("Assets/Resources/");
        foreach(var type in itemTypes)
        {
            string strippedType = type.Replace("Assets/Resources/", "");
            var allItems = Resources.LoadAll<AssemblyItem>(strippedType);
            typeInstancePair.Add(strippedType, new List<AssemblyItem>(allItems));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon = transform.Find("Weapon").gameObject;
        var snapObject = weapon.transform.Find("Snappoints");
        assemblySnapPoints = new GameObject[snapObject.childCount];
        for (int i = 0; i < assemblySnapPoints.Length; i++) 
        {
            assemblySnapPoints[i] = snapObject.GetChild(i).gameObject;
        }

        PopulateContent(currentSnapIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void AddItem(AssemblyItem item)
    {
        var itemObject = new GameObject(item.itemName, typeof(CanvasRenderer), typeof(Image));
        itemObject.GetComponent<Image>().sprite = item.sprite;
        itemObject.transform.position = assemblySnapPoints[currentSnapIndex].transform.position;
        itemObject.transform.SetParent(weapon.transform, true);
        itemObject.GetComponent<RectTransform>().sizeDelta = assemblySnapPoints[currentSnapIndex].GetComponent<RectTransform>().sizeDelta;
        PopulateContent(++currentSnapIndex);
    }

    private void PopulateContent(int typeIndex)
    {
        if (typeIndex == tempAssemblyOrder.Length) return;
        int contentChildCount = scrollView.content.childCount;
        for(int i = contentChildCount - 1; i >= 0 ; i--)
        {
            DestroyImmediate(scrollView.content.GetChild(i).gameObject);
        }

        foreach (var item in typeInstancePair[tempAssemblyOrder[typeIndex]])
        {
            CreateButtonForItem(item);
        }
    }
}
