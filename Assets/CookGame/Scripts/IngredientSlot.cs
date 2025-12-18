using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IngredientSlot : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI tasteText;
    public TextMeshProUGUI stabilityText;
    public TextMeshProUGUI magicText;
    public Button selectButton;
    
    private IngredientInstance currentIngredient;
    private System.Action<IngredientInstance> onSelectCallback;
    
    void Awake()
    {
        Debug.Log($"[IngredientSlot] Awake() on {gameObject.name}");
        
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OnButtonClicked);
            Debug.Log($"[IngredientSlot] Button listener added");
        }
    }
    
    public void SetIngredient(IngredientInstance ingredient, System.Action<IngredientInstance> onSelect)
    {
        Debug.Log($"[IngredientSlot] SetIngredient: {ingredient.data.ingredientName}");
        
        currentIngredient = ingredient;
        onSelectCallback = onSelect;
        
        if (nameText != null)
            nameText.text = ingredient.data.ingredientName;
        
        if (tasteText != null)
            tasteText.text = $"Taste: +{ingredient.tasteEffect:F1}";
        
        if (stabilityText != null)
            stabilityText.text = $"Stab: +{ingredient.stabilityEffect:F1}";
        
        if (magicText != null)
            magicText.text = $"Magic: +{ingredient.magicEffect:F1}";
        
        if (iconImage != null && ingredient.data.icon != null)
            iconImage.sprite = ingredient.data.icon;
        
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        
        Debug.Log($"[IngredientSlot] Display updated");
    }
    
    void OnButtonClicked()
    {
        Debug.Log($"[IngredientSlot] ========================================");
        Debug.Log($"[IngredientSlot] Button clicked! Ingredient: {currentIngredient.data.ingredientName}");
        Debug.Log($"[IngredientSlot] ========================================");
        
        if (onSelectCallback != null)
        {
            Debug.Log($"[IngredientSlot] Invoking callback...");
            onSelectCallback.Invoke(currentIngredient);
        }
        else
        {
            Debug.LogError($"[IngredientSlot] ❌ onSelectCallback is NULL!");
        }
    }
    
    public void Hide()
    {
        Debug.Log($"[IngredientSlot] Hiding slot");
        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
    }
}
