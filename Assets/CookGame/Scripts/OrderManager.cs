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
        Debug.Log($"[OrderManager] Total recipes in database: {allRecipes.Count}");
        
        for (int i = 0; i < allRecipes.Count; i++)
        {
            if (allRecipes[i] == null)
                Debug.LogError($"[OrderManager] ❌ Recipe at index {i} is NULL!");
            else
                Debug.Log($"[OrderManager] Recipe {i}: {allRecipes[i].recipeName}");
        }
    }
    
    public RecipeData GenerateRandomOrder()
    {
        Debug.Log("[OrderManager] GenerateRandomOrder() called");
        
        if (allRecipes.Count == 0)
        {
            Debug.LogError("[OrderManager] ❌ No recipes available! allRecipes list is empty!");
            return null;
        }
        
        int randomIndex = Random.Range(0, allRecipes.Count);
        currentOrder = allRecipes[randomIndex];
        
        Debug.Log($"[OrderManager] ✅ Random recipe selected: {currentOrder.recipeName} (index {randomIndex})");
        Debug.Log($"[OrderManager]   Taste: {currentOrder.tasteMin}-{currentOrder.tasteMax}");
        Debug.Log($"[OrderManager]   Stability: {currentOrder.stabilityMin}-{currentOrder.stabilityMax}");
        Debug.Log($"[OrderManager]   Magic: {currentOrder.magicMin}-{currentOrder.magicMax}");
        Debug.Log($"[OrderManager]   Turns: {currentOrder.totalTurns}");
        
        return currentOrder;
    }
}
