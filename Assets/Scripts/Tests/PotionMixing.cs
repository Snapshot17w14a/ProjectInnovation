using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PotionMixing : MonoBehaviour
{
    [SerializeField] private float hitThreshold = 1f;
    [SerializeField] private int successThreshold = 900;
    [SerializeField] private int failThreshold = 300;
    [SerializeField] private int averageThreshold = 50;
    [SerializeField] private float maxWaterAmount = 5f;
    [SerializeField] private float waterIncreaseRate = 0.5f;
    private Vector3 lastAcceleration;
    private int shakeCount;

    private float waterAmount = 1f;

    private bool isAddingWater = false;
    private bool canShake = false;

    private MeshRenderer meshRenderer;

    private List<string> selectedIngredients = new List<string>();

    private Dictionary<HashSet<string>, string> potionEffects = new Dictionary<HashSet<string>, string>(HashSetComparer.Instance);

    [SerializeField] private Button[] ingredientButtons;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        lastAcceleration = Input.acceleration;

        foreach (var button in ingredientButtons)
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
            Debug.Log("WaterAmount: " + waterAmount);
        }

        if(selectedIngredients.Count >= 3)
        {
            canShake = true;
        } else
        {
            canShake = false;
        }

        Vector3 currentAcceleration = Input.acceleration;
        float accelerationChange = (currentAcceleration - lastAcceleration).magnitude;

        if (accelerationChange > hitThreshold && canShake)
        {
            shakeCount++;
            Debug.Log("ShakeCount: " + shakeCount);
            lastAcceleration = currentAcceleration;
        }

        UpdatePotionColor();
    }

    public void FinishPotion()
    {
        string buff = GetPotionBuff();
        string quality = DeterminePotionQuality();
        var (duration, effectMultiplier) = CalculatePotionEffect();

        Debug.Log($"Potion Created! - Buff: {buff}, Quality: {quality}, Duration: {duration}s, Effect Strength: {effectMultiplier * 100}%");
    }

    private string DeterminePotionQuality()
    {
        if (shakeCount < averageThreshold)
        {
            return "Weak";
        }
        if (shakeCount >= averageThreshold && shakeCount < successThreshold)
        {
            return "Average";
        }
        if (shakeCount >= successThreshold && shakeCount < failThreshold)
        {
            return "Good";
        }

        return "Failed";
    }

    private string GetPotionBuff()
    {
        var key = new HashSet<string>(selectedIngredients);
        return potionEffects.TryGetValue(key, out string buff) ? buff : "Unknown Potion";
    }

    public void AddIngredient(string ingredient)
    {
        if (selectedIngredients.Count != 3)
        {
            selectedIngredients.Add(ingredient);
            Debug.Log("Added: " + ingredient);
        }
        else
        {
            Debug.LogError("You already added 3 ingredients!");
        }
    }

    public void ResetPotion()
    {
        selectedIngredients.Clear();
        waterAmount = 0f;
        shakeCount = 0;
    }

    private (float duration, float effectMultiplier) CalculatePotionEffect()
    {
        float baseDuration = 15f;
        float baseEffectiveness = 1f;

        float duration = baseDuration * waterAmount;
        float effectMultiplier = Mathf.Clamp(1f - ((waterAmount - 1f) / (maxWaterAmount - 1f)), 0.3f, 1f);

        return (duration, effectMultiplier);
    }

    private void UpdatePotionColor()
    {
        if (meshRenderer == null) return;

        if (shakeCount < averageThreshold)
        {
            meshRenderer.material.color = Color.gray; // Weak potion
        }
        else if (shakeCount < successThreshold)
        {
            meshRenderer.material.color = Color.green; // Average potion
        }
        else if (shakeCount < failThreshold)
        {
            meshRenderer.material.color = Color.blue; // Good potion
        }
        else
        {
            meshRenderer.material.color = Color.red; // Failed potion
        }
    }

    public void ToggleWater() => isAddingWater = !isAddingWater;
}
