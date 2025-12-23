using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RecipeUnlockManager : MonoBehaviour
{
    public static RecipeUnlockManager Instance { get; private set; }
    
    [Header("Recipe Database")]
    public List<RecipeData> allRecipes = new List<RecipeData>();
    
    void Awake()
    {
        Debug.Log("[RecipeUnlockManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[RecipeUnlockManager] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[RecipeUnlockManager] ⚠️ Duplicate destroyed");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ValidateRecipes();
    }
    
    void ValidateRecipes()
    {
        if (allRecipes == null || allRecipes.Count == 0)
        {
            Debug.LogError("[RecipeUnlockManager] ❌ No recipes in database!");
            return;
        }
        
        Debug.Log($"[RecipeUnlockManager] ✅ {allRecipes.Count} recipes loaded");
        
        foreach (var recipe in allRecipes)
        {
            if (recipe != null)
            {
                Debug.Log($"[RecipeUnlockManager]   - {recipe.recipeName} (Level {recipe.unlockLevel}, {recipe.difficulty})");
            }
        }
    }
    
    public List<RecipeData> GetUnlockedRecipes()
    {
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        var unlocked = allRecipes
            .Where(r => r != null && r.unlockLevel <= playerLevel)
            .OrderBy(r => r.unlockLevel)
            .ThenBy(r => r.difficulty)
            .ToList();
        
        Debug.Log($"[RecipeUnlockManager] Player level {playerLevel}: {unlocked.Count}/{allRecipes.Count} recipes unlocked");
        
        return unlocked;
    }
    
    public List<RecipeData> GetLockedRecipes()
    {
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        return allRecipes
            .Where(r => r != null && r.unlockLevel > playerLevel)
            .OrderBy(r => r.unlockLevel)
            .ToList();
    }
    
    public bool IsRecipeUnlocked(RecipeData recipe)
    {
        if (recipe == null) return false;
        
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        return recipe.unlockLevel <= playerLevel;
    }
    
    public RecipeData GetNextUnlockableRecipe()
    {
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        return allRecipes
            .Where(r => r != null && r.unlockLevel > playerLevel)
            .OrderBy(r => r.unlockLevel)
            .FirstOrDefault();
    }
    
    public int GetLevelsUntilNextUnlock()
    {
        var nextRecipe = GetNextUnlockableRecipe();
        if (nextRecipe == null) return -1;
        
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        return nextRecipe.unlockLevel - playerLevel;
    }
}
