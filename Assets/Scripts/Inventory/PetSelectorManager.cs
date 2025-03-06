using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PetSelectorManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Pet[] petPrefabs;

    private readonly Dictionary<string, Pet> namePetPair = new();

    private static BattleManager battleManager;
    [SerializeField] private GameObject canvas;

    public int SlotIndex { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var prefab in petPrefabs)
        {
            namePetPair.Add(prefab.name, prefab);
        }

        foreach(var petName in namePetPair.Keys)
        {
            var tile = Instantiate(tilePrefab, Vector2.zero, Quaternion.identity, transform);
            tile.transform.GetChild(0).GetComponent<Image>().sprite = namePetPair[petName].GetComponent<Image>().sprite;
            tile.GetComponent<Button>().onClick.AddListener(() => SelectPetForIndex(petName));
        }

        battleManager = FindAnyObjectByType<BattleManager>();
    }

    public void SelectPetForIndex(string petName)
    {
        var createdPet = Instantiate<Pet>(namePetPair[petName], Vector2.zero, Quaternion.identity, battleManager.transform);
        battleManager.SetPetAtIndex(SlotIndex, createdPet);
        canvas.SetActive(false);
    }
}
