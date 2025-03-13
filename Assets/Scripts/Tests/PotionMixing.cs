using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PotionMixing : MonoBehaviour
{
    private float hitThreshold = 1f;
    [SerializeField] private int averageThreshold = 50;
    [SerializeField] private int successThreshold = 900;
    [SerializeField] private int failThreshold = 300;
    [SerializeField] private float maxWaterAmount = 5f;
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
    void Start()
    {
        lastAcceleration = Input.acceleration;

        foreach (Button button in ingredientButtons)
        {
            string ingredientName = button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            button.onClick.AddListener(() => AddIngredient(ingredientName));
        }

        potionEffects.Add(new HashSet<string> { "Herbs", "Mushroom", "Berries" }, "Health");
        potionEffects.Add(new HashSet<string> { "Shards", "Goo", "Wings" }, "Damage");
        potionEffects.Add(new HashSet<string> { "Scales", "Shards", "Silk" }, "Armour");
        potionEffects.Add(new HashSet<string> { "Berries", "Wings", "Feather" }, "Attack Speed");
        potionEffects.Add(new HashSet<string> { "Feather", "Herbs", "Silk" }, "Crit Chance");
        potionEffects.Add(new HashSet<string> { "Mushroom", "Goo", "Scales" }, "Armour Pen");
    }

    void Update()
    {
        if (isAddingWater && waterAmount <= maxWaterAmount)
        {
            waterAmount += waterIncreaseRate * Time.deltaTime;
            waterText.text = $" " + waterAmount.ToString("F1");
        }

        Vector3 currentAcceleration = Input.acceleration;
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > hitThreshold && canShake)
        {
            shakeCount++;
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


        switch (buff) {

            case "Health":
                
                // inventoryManager.AddPotion(new HealthPotion(100));

                break;

            default:
                throw new NotImplementedException(nameof(buff));
        }

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
        return potionEffects.TryGetValue(key, out string buff) ? $"Gives a +{effectiveness.ToString("F1")}% {buff} buff for {duration.ToString("F0")} seconds " : "Potion Fail: Ingredients incompatible";
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

    private void ShowRecipe()
    {
        if(isShowingRecipes)
        {
            recipeImage.gameObject.SetActive(true);
        } else
        {
            recipeImage.gameObject.SetActive(false);
        }
    }

    public void ToggleWater() => isAddingWater = !isAddingWater;
    public void ToggleRecipes() => isShowingRecipes = !isShowingRecipes;
}
