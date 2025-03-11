using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject petSelectorCanvas;

    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private List<WeaponTile> createdTiles;

    private Weapon selectedWeapon;

    public void UpdateContent()
    {
        foreach (var tile in createdTiles) if (tile != null) DestroyImmediate(tile.gameObject);
        createdTiles.Clear();
        var weapons = ServiceLocator.GetService<InventoryManager>().GetAllWeapons;
        foreach (var weapon in weapons)
        {
            var tile = Instantiate(tilePrefab, Vector2.zero, Quaternion.identity, scrollContent.transform).GetComponent<WeaponTile>();
            tile.damageText.text = $"Damage: {weapon.Damage}";
            tile.critChanceText.text = $"Crit Chance: {weapon.CritChance}%";
            tile.critDamageText.text = $"Crit Damage: {weapon.CriticalDamage}%";
            tile.attackSpeedText.text = $"Attack Speed: {weapon.AttackSpeed}/s" ;
            tile.materialText.text = $"Material: {weapon.ItemMaterial}" ;
            tile.armorPenText.text = $"Armor Penetration: {weapon.ArmorPenetration}";
            tile.weapon = weapon;
            tile.equipButton.onClick.AddListener(() =>
            {
                PetSelectorManager.ReturnFunction = SetWeaponForPet;
                petSelectorCanvas.SetActive(true);
                selectedWeapon = tile.weapon;
            });
            createdTiles.Add(tile);
        }
    }

    public void SetWeaponForPet(Pet pet, int index)
    {
        GlobalEvent<WeaponAssigmentEvent>.RaiseEvent(new WeaponAssigmentEvent(selectedWeapon, pet.name));
        //var inventoryManger = ServiceLocator.GetService<InventoryManager>();
        //var petStat = inventoryManger.PetNameToStats(pet.name);
        //inventoryManger.SetPetStat(pet.name, petStat.AssignWeapon(selectedWeapon));
    }
}
