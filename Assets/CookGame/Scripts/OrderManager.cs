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
                    Debug.Log($"[OrderManager]   - {recipe.recipeName} ({recipe.difficulty})");
                }
            }
        }
    }
    
    public RecipeData GenerateRandomOrder()
    {
        Debug.Log("[OrderManager] GenerateRandomOrder() called");
        
        if (allRecipes == null || allRecipes.Count == 0)
        {
            Debug.LogError("[OrderManager] ❌ No recipes available!");
            return null;
        }
        
        int randomIndex = Random.Range(0, allRecipes.Count);
        currentOrder = allRecipes[randomIndex];
        
        Debug.Log($"[OrderManager] ✅ Generated order: {currentOrder.recipeName}");
        Debug.Log($"[OrderManager]   Taste: {currentOrder.tasteMin}-{currentOrder.tasteMax}");
        Debug.Log($"[OrderManager]   Stability: {currentOrder.stabilityMin}-{currentOrder.stabilityMax}");
        Debug.Log($"[OrderManager]   Magic: {currentOrder.magicMin}-{currentOrder.magicMax}");
        Debug.Log($"[OrderManager]   Turns: {currentOrder.totalTurns}");
        
        return currentOrder;
    }
    
    public void ClearOrder()
    {
        Debug.Log("[OrderManager] ClearOrder() called");
        currentOrder = null;
    }
}
