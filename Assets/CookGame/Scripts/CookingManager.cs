using UnityEngine;
using System.Collections.Generic;

public class CookingManager : MonoBehaviour
{
    [Header("Ingredient Database")]
    public List<IngredientData> allIngredients = new List<IngredientData>();
    
    [Header("References")]
    public MeterController tasteMeter;
    public MeterController stabilityMeter;
    public MeterController magicMeter;
    
    public IngredientSlot slot1;
    public IngredientSlot slot2;
    public IngredientSlot slot3;
    
    private RecipeData currentRecipe;
    private int turnsRemaining;
    private IngredientInstance[] currentIngredients = new IngredientInstance[3];
    
    void Awake()
    {
        Debug.Log("[CookingManager] Awake() called");
        Debug.Log($"[CookingManager] Total ingredients in database: {allIngredients.Count}");
    }
    
    public void StartCooking(RecipeData recipe)
    {
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] StartCooking: {recipe.recipeName}");
        Debug.Log($"[CookingManager] ========================================");
        
        currentRecipe = recipe;
        turnsRemaining = recipe.totalTurns;
        
        Debug.Log($"[CookingManager] Initializing meters...");
        tasteMeter.Initialize("Taste", recipe.tasteMin, recipe.tasteMax);
        stabilityMeter.Initialize("Stability", recipe.stabilityMin, recipe.stabilityMax);
        magicMeter.Initialize("Magic", recipe.magicMin, recipe.magicMax);
        
        Debug.Log($"[CookingManager] Starting first turn...");
        StartNewTurn();
    }
    
    void StartNewTurn()
    {
        Debug.Log($"[CookingManager] ----------------------------------------");
        Debug.Log($"[CookingManager] StartNewTurn - Turns remaining: {turnsRemaining}");
        Debug.Log($"[CookingManager] ----------------------------------------");
        
        if (turnsRemaining <= 0)
        {
            Debug.Log($"[CookingManager] No turns remaining! Ending cooking...");
            EndCooking();
            return;
        }
        
        Debug.Log($"[CookingManager] Generating 3 random ingredients...");
        GenerateIngredients();
    }
    
    void GenerateIngredients()
    {
        if (allIngredients.Count == 0)
        {
            Debug.LogError("[CookingManager] ❌ No ingredients in database!");
            return;
        }
        
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, allIngredients.Count);
            IngredientData ingredient = allIngredients[randomIndex];
            currentIngredients[i] = ingredient.GenerateInstance();
            
            Debug.Log($"[CookingManager] Slot {i + 1}: {ingredient.ingredientName}");
        }
        
        Debug.Log($"[CookingManager] Displaying ingredients in slots...");
        
        slot1.SetIngredient(currentIngredients[0], OnIngredientSelected);
        slot2.SetIngredient(currentIngredients[1], OnIngredientSelected);
        slot3.SetIngredient(currentIngredients[2], OnIngredientSelected);
        
        Debug.Log($"[CookingManager] ✅ Turn setup complete!");
    }
    
    void OnIngredientSelected(IngredientInstance selected)
    {
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] OnIngredientSelected: {selected.data.ingredientName}");
        Debug.Log($"[CookingManager] ========================================");
        
        Debug.Log($"[CookingManager] Applying effects...");
        tasteMeter.AddValue(selected.tasteEffect);
        stabilityMeter.AddValue(selected.stabilityEffect);
        magicMeter.AddValue(selected.magicEffect);
        
        Debug.Log($"[CookingManager] Checking for overflow...");
        if (CheckForOverflow())
        {
            Debug.Log($"[CookingManager] ❌ OVERFLOW DETECTED! Game Over!");
            EndCooking();
            return;
        }
        
        turnsRemaining--;
        Debug.Log($"[CookingManager] Turn consumed. Remaining: {turnsRemaining}");
        
        Debug.Log($"[CookingManager] Hiding ingredient slots...");
        slot1.Hide();
        slot2.Hide();
        slot3.Hide();
        
        Invoke(nameof(StartNewTurn), 1f);
    }
    
    bool CheckForOverflow()
    {
        bool overflow = tasteMeter.IsOverTarget() || 
                       stabilityMeter.IsOverTarget() || 
                       magicMeter.IsOverTarget();
        
        Debug.Log($"[CookingManager] Overflow check: {overflow}");
        return overflow;
    }
    
    void EndCooking()
    {
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] EndCooking called");
        Debug.Log($"[CookingManager] ========================================");
        
        bool tasteOK = tasteMeter.IsInTarget();
        bool stabilityOK = stabilityMeter.IsInTarget();
        bool magicOK = magicMeter.IsInTarget();
        
        Debug.Log($"[CookingManager] Taste in target: {tasteOK}");
        Debug.Log($"[CookingManager] Stability in target: {stabilityOK}");
        Debug.Log($"[CookingManager] Magic in target: {magicOK}");
        
        bool victory = tasteOK && stabilityOK && magicOK;
        
        Debug.Log($"[CookingManager] Result: {(victory ? "VICTORY! ✅" : "DEFEAT! ❌")}");
        Debug.Log($"[CookingManager] ========================================");
        
        GameManager.Instance.uiManager.ShowResultPanel(victory);
    }
}
