using UnityEngine;
using TMPro;
using DG.Tweening;

public class OrderPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI tasteTargetText;
    public TextMeshProUGUI stabilityTargetText;
    public TextMeshProUGUI magicTargetText;
    public TextMeshProUGUI turnsText;
    
    void Awake()
    {
        Debug.Log("[OrderPanel] Awake() called");
        
        if (recipeNameText == null) Debug.LogError("[OrderPanel] ❌ recipeNameText is NULL!");
        if (tasteTargetText == null) Debug.LogError("[OrderPanel] ❌ tasteTargetText is NULL!");
        if (stabilityTargetText == null) Debug.LogError("[OrderPanel] ❌ stabilityTargetText is NULL!");
        if (magicTargetText == null) Debug.LogError("[OrderPanel] ❌ magicTargetText is NULL!");
        if (turnsText == null) Debug.LogError("[OrderPanel] ❌ turnsText is NULL!");
    }
    
    public void DisplayRecipe(RecipeData recipe)
    {
        Debug.Log($"[OrderPanel] DisplayRecipe() called with: {recipe.recipeName}");
        
        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.recipeName;
            Debug.Log($"[OrderPanel] Recipe name set: {recipe.recipeName}");
        }
        
        if (tasteTargetText != null)
        {
            tasteTargetText.text = $"Taste: {recipe.tasteMin:F0} - {recipe.tasteMax:F0}";
            Debug.Log($"[OrderPanel] Taste text set: {tasteTargetText.text}");
        }
        
        if (stabilityTargetText != null)
        {
            stabilityTargetText.text = $"Stability: {recipe.stabilityMin:F0} - {recipe.stabilityMax:F0}";
            Debug.Log($"[OrderPanel] Stability text set: {stabilityTargetText.text}");
        }
        
        if (magicTargetText != null)
        {
            magicTargetText.text = $"Magic: {recipe.magicMin:F0} - {recipe.magicMax:F0}";
            Debug.Log($"[OrderPanel] Magic text set: {magicTargetText.text}");
        }
        
        if (turnsText != null)
        {
            turnsText.text = $"Turns: {recipe.totalTurns}";
            Debug.Log($"[OrderPanel] Turns text set: {turnsText.text}");
        }
        
        Debug.Log("[OrderPanel] Starting text animations...");
        AnimateTexts();
    }
    
    void AnimateTexts()
    {
        Debug.Log("[OrderPanel] AnimateTexts() called");
        
        if (recipeNameText != null) recipeNameText.alpha = 0;
        if (tasteTargetText != null) tasteTargetText.alpha = 0;
        if (stabilityTargetText != null) stabilityTargetText.alpha = 0;
        if (magicTargetText != null) magicTargetText.alpha = 0;
        if (turnsText != null) turnsText.alpha = 0;
        
        Sequence seq = DOTween.Sequence();
        
        if (recipeNameText != null)
            seq.Append(recipeNameText.DOFade(1f, 0.3f).OnComplete(() => Debug.Log("[OrderPanel] Recipe name fade complete")));
            
        if (tasteTargetText != null)
            seq.Append(tasteTargetText.DOFade(1f, 0.3f).OnComplete(() => Debug.Log("[OrderPanel] Taste fade complete")));
            
        if (stabilityTargetText != null)
            seq.Append(stabilityTargetText.DOFade(1f, 0.3f).OnComplete(() => Debug.Log("[OrderPanel] Stability fade complete")));
            
        if (magicTargetText != null)
            seq.Append(magicTargetText.DOFade(1f, 0.3f).OnComplete(() => Debug.Log("[OrderPanel] Magic fade complete")));
            
        if (turnsText != null)
            seq.Append(turnsText.DOFade(1f, 0.3f).OnComplete(() => Debug.Log("[OrderPanel] Turns fade complete")));
        
        seq.OnComplete(() => Debug.Log("[OrderPanel] ✅ All text animations complete!"));
    }
    
    public void OnAcceptOrderButtonPressed()
    {
        Debug.Log("[OrderPanel] ========================================");
        Debug.Log("[OrderPanel] ACCEPT ORDER button pressed!");
        Debug.Log("[OrderPanel] ========================================");
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("[OrderPanel] ❌ GameManager.Instance is NULL!");
            return;
        }
        
        if (GameManager.Instance.uiManager == null)
        {
            Debug.LogError("[OrderPanel] ❌ UIManager is NULL!");
            return;
        }
        
        if (GameManager.Instance.orderManager == null || GameManager.Instance.orderManager.currentOrder == null)
        {
            Debug.LogError("[OrderPanel] ❌ No current order!");
            return;
        }
        
        Debug.Log("[OrderPanel] Starting cooking...");
        GameManager.Instance.uiManager.ShowCookingPanel(GameManager.Instance.orderManager.currentOrder);
        Debug.Log("[OrderPanel] ✅ ShowCookingPanel() called");
        Debug.Log("[OrderPanel] ========================================");
    }
}
