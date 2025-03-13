using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PetSelectorManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Pet[] petPrefabs;

    private readonly Dictionary<string, Pet> namePetPair = new();

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject canvas;

    public static int SlotIndex { get; set; }

    public static System.Action<Pet, int> ReturnFunction { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var prefab in petPrefabs)
        {
            namePetPair.Add(prefab.name, prefab);
        }

        foreach(var petName in namePetPair.Keys)
        {
            var tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, transform);
            tile.transform.GetChild(0).GetComponent<Image>().sprite = namePetPair[petName].GetComponent<Image>().sprite;
            tile.GetComponent<Button>().onClick.AddListener(() => ReturnSelectedpPet(petName));
        }
    }

    public void ReturnSelectedpPet(string petName)
    {
        ReturnFunction(namePetPair[petName], SlotIndex);
        ReturnFunction = null;
        uiManager.CloseAllOverlays();
    }
}
