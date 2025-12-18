using UnityEngine;
using TMPro;

public class CookingPanel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI turnsRemainingText;
    
    private CookingManager cookingManager;
    
    void Awake()
    {
        Debug.Log("[CookingPanel] Awake() called");
    }
    
    public void Initialize(RecipeData recipe, CookingManager manager)
    {
        Debug.Log($"[CookingPanel] Initialize with recipe: {recipe.recipeName}");
        
        cookingManager = manager;
        
        if (recipeNameText != null)
            recipeNameText.text = recipe.recipeName;
        
        if (turnsRemainingText != null)
            turnsRemainingText.text = $"Turns: {recipe.totalTurns}";
    }
    
    public void UpdateTurnsDisplay(int remaining)
    {
        Debug.Log($"[CookingPanel] UpdateTurnsDisplay: {remaining}");
        
        if (turnsRemainingText != null)
            turnsRemainingText.text = $"Turns: {remaining}";
    }
}
