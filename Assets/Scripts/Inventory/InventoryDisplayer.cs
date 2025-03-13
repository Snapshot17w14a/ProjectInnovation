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
            var tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, scrollContent.transform).GetComponent<WeaponTile>();
            tile.damageText.text = $"<b>Damage:</b> {weapon.Damage}";
            tile.critChanceText.text = $"<b>Crit Chance:</b> {weapon.CritChance}%";
            tile.critDamageText.text = $"<b>Crit Damage:</b> {weapon.CriticalDamage}%";
            tile.attackSpeedText.text = $"<b>Attack Speed:</b> +{weapon.AttackSpeed}/s";
            tile.materialText.text = $"<b>Material:</b> {weapon.ItemMaterial}" ;
            tile.armorPenText.text = $"<b>Armor Penetration:</b> {weapon.ArmorPenetration}";
            tile.weapon = weapon;
            tile.equipButton.onClick.AddListener(() =>
            {
                PetSelectorManager.ReturnFunction = SetWeaponForPet;
                petSelectorCanvas.SetActive(true);
                selectedWeapon = tile.weapon;
            });
            var icon = weapon.GetWeaponSpritePrefab();
            icon.transform.SetParent(tile.iconTransform);
            icon.transform.position = tile.iconTransform.position;
            icon.transform.localScale = new Vector3(2, 2, 2);
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
