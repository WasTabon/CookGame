using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    [Header("Recipe Database")]
    public List<RecipeData> allRecipes = new List<RecipeData>();
    
    public RecipeData currentOrder { get; private set; }
    
    void Awake()
    {
        Debug.Log("[OrderManager] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[OrderManager] Start() called");
        ValidateReferences();
    }
    
    void ValidateReferences()
    {
        if (allRecipes == null || allRecipes.Count == 0)
        {
            Debug.LogError("[OrderManager] ❌ No recipes in database!");
        }
        else
        {
            Debug.Log($"[OrderManager] ✅ {allRecipes.Count} recipes loaded");
            foreach (var recipe in allRecipes)
            {
                if (recipe != null)
                {
                    Debug.Log($"[OrderManager]   - {recipe.recipeName} (Level {recipe.unlockLevel}, {recipe.difficulty})");
                }
            }
        }
    }
    
    public List<RecipeData> GetAvailableRecipes()
    {
        if (RecipeUnlockManager.Instance != null)
        {
            return RecipeUnlockManager.Instance.GetUnlockedRecipes();
        }
        
        return allRecipes;
    }
    
    public RecipeData GenerateRandomOrder()
    {
        Debug.Log("[OrderManager] GenerateRandomOrder() called");
        
        List<RecipeData> availableRecipes = GetAvailableRecipes();
        
        if (availableRecipes == null || availableRecipes.Count == 0)
        {
            Debug.LogError("[OrderManager] ❌ No recipes available!");
            return null;
        }
        
        int randomIndex = Random.Range(0, availableRecipes.Count);
        currentOrder = availableRecipes[randomIndex];
        
        Debug.Log($"[OrderManager] ✅ Generated order: {currentOrder.recipeName}");
        Debug.Log($"[OrderManager]   Unlock Level: {currentOrder.unlockLevel}");
        Debug.Log($"[OrderManager]   Taste: {currentOrder.tasteMin}-{currentOrder.tasteMax}");
        Debug.Log($"[OrderManager]   Stability: {currentOrder.stabilityMin}-{currentOrder.stabilityMax}");
        Debug.Log($"[OrderManager]   Magic: {currentOrder.magicMin}-{currentOrder.magicMax}");
        Debug.Log($"[OrderManager]   Turns: {currentOrder.totalTurns}");
        
        return currentOrder;
    }
    
    public RecipeData GenerateOrderByDifficulty(RecipeData.Difficulty difficulty)
    {
        Debug.Log($"[OrderManager] GenerateOrderByDifficulty({difficulty}) called");
        
        List<RecipeData> availableRecipes = GetAvailableRecipes();
        List<RecipeData> matchingRecipes = availableRecipes.FindAll(r => r.difficulty == difficulty);
        
        if (matchingRecipes.Count == 0)
        {
            Debug.LogWarning($"[OrderManager] ⚠️ No {difficulty} recipes available, using random");
            return GenerateRandomOrder();
        }
        
        int randomIndex = Random.Range(0, matchingRecipes.Count);
        currentOrder = matchingRecipes[randomIndex];
        
        Debug.Log($"[OrderManager] ✅ Generated {difficulty} order: {currentOrder.recipeName}");
        
        return currentOrder;
    }
    
    public void SetOrder(RecipeData recipe)
    {
        if (recipe == null)
        {
            Debug.LogError("[OrderManager] ❌ Cannot set null recipe!");
            return;
        }
        
        if (RecipeUnlockManager.Instance != null && !RecipeUnlockManager.Instance.IsRecipeUnlocked(recipe))
        {
            Debug.LogWarning($"[OrderManager] ⚠️ Recipe {recipe.recipeName} is locked!");
            return;
        }
        
        currentOrder = recipe;
        Debug.Log($"[OrderManager] ✅ Order set: {currentOrder.recipeName}");
    }
    
    public void ClearOrder()
    {
        Debug.Log("[OrderManager] ClearOrder() called");
        currentOrder = null;
    }
    
    public int GetUnlockedRecipeCount()
    {
        if (RecipeUnlockManager.Instance != null)
        {
            return RecipeUnlockManager.Instance.GetUnlockedRecipes().Count;
        }
        return allRecipes.Count;
    }
    
    public int GetTotalRecipeCount()
    {
        return allRecipes.Count;
    }
}
