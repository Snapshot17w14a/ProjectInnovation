using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponTile : MonoBehaviour
{
    public Weapon displayedWeapon;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI materialText;
    public TextMeshProUGUI armorPenText;

    public Button equipButton;

    public Weapon weapon;

    private void Awake()
    {
        transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Removing weapon: " + weapon);
            var manager = ServiceLocator.GetService<InventoryManager>();
            manager.RemoveItemFromInventory(weapon);
            manager.SaveWeapons();
            Destroy(gameObject);
        });
        equipButton = transform.Find("Equip").GetComponent<Button>();
    }
}
