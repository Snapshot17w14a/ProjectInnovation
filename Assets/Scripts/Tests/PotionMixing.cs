using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class PotionMixing : MonoBehaviour
{
    private float hitThreshold = 1f;
    [SerializeField] private int averageThreshold = 50;
    [SerializeField] private int successThreshold = 900;
    [SerializeField] private int failThreshold = 300;
    [SerializeField] private float maxWaterAmount = 5f;
    [SerializeField] private float intencity = 0.1f;
    private float waterIncreaseRate = 0.5f;
    private Vector3 lastAcceleration;
    private int shakeCount;

    private float waterAmount = 1f;

    private bool isAddingWater = false;
    private bool canShake = false;
    private bool isShowingRecipes = false;

    private string buff;
    private string quality;

    private List<string> selectedIngredients = new List<string>();

    private Dictionary<HashSet<string>, string> potionEffects = new Dictionary<HashSet<string>, string>(HashSetComparer.Instance);

    [SerializeField] private Button[] ingredientButtons;
    [SerializeField] private Button waterButton;
    [SerializeField] private Button shakeButton;
    [SerializeField] private Button completeButton;

    [SerializeField] private TMP_Text effectText;
    [SerializeField] private TMP_Text waterText;

    [SerializeField] private RawImage shakeIndicator;
    [SerializeField] private RawImage bookImage;
    [SerializeField] private RawImage recipeImage;

    [SerializeField] private Material liquidMaterial;

    [SerializeField] private Transform effectPosition;
    [SerializeField] private VisualEffect waterVFX;
    void Start()
    {
        DisableVFX();
        lastAcceleration = Input.acceleration;

        foreach (Button button in ingredientButtons)
        {
            string ingredientName = button.GetComponentInChildren<TextMeshProUGUI>().text;
            button.onClick.AddListener(() => AddIngredient(ingredientName));
        }

        potionEffects.Add(new HashSet<string> { "Herbs", "Mushroom", "Berries" }, "Health");
        potionEffects.Add(new HashSet<string> { "Shards", "Goo", "Wings" }, "Damage");
        potionEffects.Add(new HashSet<string> { "Scales", "Shards", "Silk" }, "Armour");
        potionEffects.Add(new HashSet<string> { "Berries", "Wings", "Feather" }, "Attack Speed");
        potionEffects.Add(new HashSet<string> { "Feather", "Herbs", "Silk" }, "Critical Chance");
        potionEffects.Add(new HashSet<string> { "Mushroom", "Goo", "Scales" }, "Armour Penetration");
    }

    void Update()
    {
        if (isAddingWater && waterAmount <= maxWaterAmount)
        {
            waterAmount += waterIncreaseRate * Time.deltaTime;
            
            waterText.text = $" " + waterAmount.ToString("F1");
        } 
        else
        {
            EnableVFX();
        }

        Vector3 currentAcceleration = Input.acceleration;
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > hitThreshold && canShake)
        {
            shakeCount++;
            Debug.Log(" " + shakeCount);
            ChangeMaterial(shakeCount);
            lastAcceleration = currentAcceleration;
        }

        ShowRecipe();
    }

    public void FinishPotion()
    {
        buff = GetPotionBuff();
        var (duration, effectMultiplier) = CalculatePotionEffect();
        Debug.Log($"Potion Created! - Buff: {buff}, Quality: {quality}, Duration: {duration}s, Effect Strength: {effectMultiplier * 100}%");

        effectText.text = buff;
        InventoryManager inventoryManager = ServiceLocator.GetService<InventoryManager>();

        potionEffects.TryGetValue(new HashSet<string>(selectedIngredients), out string switchString);

        switch (switchString)
        {

            case "Health":
                inventoryManager.AddPotion(new HealthPotion((int)effectMultiplier));
                break;
            case "Damage":
                inventoryManager.AddPotion(new DamagePotion((int)effectMultiplier, duration));
                break;
            case "Armour":
                inventoryManager.AddPotion(new ArmourPotion((int)effectMultiplier, duration));
                break;
            case "Attack Speed":
                inventoryManager.AddPotion(new AttackSpeedPotion((int)effectMultiplier, duration));
                break;
            case "Critical Chance":
                inventoryManager.AddPotion(new CricicalChancePotion((int)effectMultiplier, duration));
                break;
            case "Armour Penetration":
                inventoryManager.AddPotion(new ArmourPenetrationPotion((int)effectMultiplier, duration));
                break;
            default:
                throw new NotImplementedException(nameof(buff));
        }

        inventoryManager.SavePotions();
    }

    private (string quality, float multiplier) DeterminePotionQuality()
    {
        if (shakeCount < averageThreshold)
        {
            return ("Weak", 0.5f);
        }
        if (shakeCount >= averageThreshold && shakeCount < successThreshold)
        {
            return ("Average", 1f);
        }
        if (shakeCount >= successThreshold && shakeCount < failThreshold)
        {
            return ("Good", 2f);
        }

        return ("Failed", 0f);
    }

    private string GetPotionBuff()
    {
        var key = new HashSet<string>(selectedIngredients);
        var (duration, effectMultiplier) = CalculatePotionEffect();
        var (quality, multiplier) = DeterminePotionQuality();

        var effectiveness = effectMultiplier * multiplier;
        return potionEffects.TryGetValue(key, out string buff) ? $"Gives a +{effectiveness:F1}% {buff} buff for {duration:F0} seconds " : "Potion Fail: Ingredients incompatible";
    }

    // shaking = effectiveness

    public void AddIngredient(string ingredient)
    {
        if (selectedIngredients.Count != 3)
        {
            selectedIngredients.Add(ingredient);
            Debug.Log("Added: " + ingredient);

            if (selectedIngredients.Count == 1)
            {

            }
            else if (selectedIngredients.Count == 2)
            {

            }
            else if (selectedIngredients.Count == 3)
            {
                bookImage.gameObject.SetActive(false);
                waterButton.gameObject.SetActive(true);
                waterText.gameObject.SetActive(true);
                waterText.text = $" " + waterAmount.ToString("F1");
                shakeButton.gameObject.SetActive(true);
            }
        }
    }

    public void ResetPotion()
    {
        selectedIngredients.Clear();
        waterAmount = 1f;
        shakeCount = 0;


        effectText.text = $"";
        waterText.text = $"{waterAmount.ToString("F1")}";

        bookImage.gameObject.SetActive(true);
        foreach (Button button in ingredientButtons)
        {
            button.gameObject.SetActive(true);
        }

        waterButton.gameObject.SetActive(false);
        waterText.gameObject.SetActive(false);
        shakeIndicator.gameObject.SetActive(false);
        shakeButton.gameObject.SetActive(false);
        completeButton.gameObject.SetActive(false);
        canShake = false;
    }

    private (float duration, float effectMultiplier) CalculatePotionEffect()
    {
        float baseDuration = 15f;

        float duration = baseDuration * waterAmount;
        float effectMultiplier = Mathf.Clamp(1f - ((waterAmount - 2f) / (maxWaterAmount - 1f)), 0.3f, 1f);

        return (duration, effectMultiplier * 100);
    }

    public void ProceedToShake()
    {
        canShake = true;
        shakeButton.gameObject.SetActive(false);
        waterText.gameObject.SetActive(false);
        waterButton.gameObject.SetActive(false);
        shakeIndicator.gameObject.SetActive(true);
        completeButton.gameObject.SetActive(true);
    }

    private void ChangeMaterial(int shakeCount)
    {
        Color targetColor = Color.white;

        if (shakeCount >= averageThreshold)
        {
            targetColor = Color.green;
        }
        if (shakeCount >= successThreshold)
        {
            targetColor = Color.blue;
        }
        if(shakeCount >= failThreshold)
        {
            targetColor = Color.red;
        }

        liquidMaterial.SetVector("_Color_Gradient_Top", new Vector4(targetColor.r * intencity, targetColor.g * intencity, targetColor.b * intencity, 1f));
    }

    private void ShowRecipe()
    {
        if (isShowingRecipes)
        {
            recipeImage.gameObject.SetActive(true);
        }
        else
        {
            recipeImage.gameObject.SetActive(false);
        }
    }

    private void EnableVFX()
    {
        if(waterVFX != null)
        {
            waterVFX.Play();
        }
    }

    private void DisableVFX()
    {
        if(waterVFX != null)
        {
            waterVFX.Stop();
        }
    }

    public void ToggleWater() => isAddingWater = !isAddingWater;
    public void ToggleRecipes() => isShowingRecipes = !isShowingRecipes;

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
