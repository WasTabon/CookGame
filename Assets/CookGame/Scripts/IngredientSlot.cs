using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class IngredientSlot : MonoBehaviour
{
    [Header("UI References")]
    public Button slotButton;
    public Image backgroundImage;
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text tasteText;
    public TMP_Text stabilityText;
    public TMP_Text magicText;
    
    [Header("Rarity Colors")]
    public Color commonColor = new Color(0.7f, 0.7f, 0.7f);
    public Color rareColor = new Color(0.3f, 0.5f, 1f);
    public Color epicColor = new Color(0.8f, 0.3f, 1f);
    
    [Header("Animation Settings")]
    public float spinDuration = 0.5f;
    public int spinRotations = 2;
    
    private IngredientInstance currentIngredient;
    private Action onClickCallback;
    private Sequence displaySequence;
    
    void Awake()
    {
        Debug.Log($"[IngredientSlot] Awake() - {gameObject.name}");
    }
    
    void Start()
    {
        ValidateReferences();
        SetupButton();
    }
    
    void ValidateReferences()
    {
        if (slotButton == null)
            Debug.LogError($"[IngredientSlot] ❌ {gameObject.name} - Slot Button is NULL!");
        if (nameText == null)
            Debug.LogWarning($"[IngredientSlot] ⚠️ {gameObject.name} - Name Text is NULL");
    }
    
    void SetupButton()
    {
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
            Debug.Log($"[IngredientSlot] {gameObject.name} ✅ Button listener added");
        }
    }
    
    public void DisplayIngredient(IngredientInstance ingredient, Action onClick)
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} DisplayIngredient: {ingredient.data.ingredientName}");
        
        currentIngredient = ingredient;
        onClickCallback = onClick;
        
        gameObject.SetActive(true);
        
        if (slotButton != null)
        {
            slotButton.interactable = true;
        }
        
        PlayDisplayAnimation();
    }
    
    void PlayDisplayAnimation()
    {
        displaySequence?.Kill();
        
        transform.localScale = Vector3.zero;
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        
        displaySequence = DOTween.Sequence();
        
        displaySequence.Append(transform.DOScale(1.2f, spinDuration * 0.5f).SetEase(Ease.OutBack));
        displaySequence.Join(canvasGroup.DOFade(1f, spinDuration * 0.3f));
        displaySequence.Join(transform.DORotate(new Vector3(0, 360 * spinRotations, 0), spinDuration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));
        
        displaySequence.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
        
        displaySequence.OnComplete(() => UpdateUI());
        
        Debug.Log($"[IngredientSlot] {gameObject.name} ✅ Display animation started");
    }
    
    void UpdateUI()
    {
        if (currentIngredient == null)
        {
            Debug.LogError($"[IngredientSlot] {gameObject.name} ❌ Current ingredient is NULL!");
            return;
        }
        
        Debug.Log($"[IngredientSlot] {gameObject.name} Updating UI...");
        
        if (nameText != null)
        {
            nameText.text = currentIngredient.data.ingredientName;
        }
        
        if (tasteText != null)
        {
            tasteText.text = $"+{currentIngredient.tasteEffect:F0}";
        }
        
        if (stabilityText != null)
        {
            stabilityText.text = $"+{currentIngredient.stabilityEffect:F0}";
        }
        
        if (magicText != null)
        {
            magicText.text = $"+{currentIngredient.magicEffect:F0}";
        }
        
        if (iconImage != null && currentIngredient.data.icon != null)
        {
            iconImage.sprite = currentIngredient.data.icon;
        }
        
        UpdateRarityColor();
        
        Debug.Log($"[IngredientSlot] {gameObject.name} ✅ UI updated");
    }
    
    void UpdateRarityColor()
    {
        if (backgroundImage == null) return;
        
        Color color;
        switch (currentIngredient.data.rarity)
        {
            case IngredientData.Rarity.Epic:
                color = epicColor;
                break;
            case IngredientData.Rarity.Rare:
                color = rareColor;
                break;
            default:
                color = commonColor;
                break;
        }
        
        backgroundImage.DOColor(color, 0.3f);
        Debug.Log($"[IngredientSlot] {gameObject.name} Rarity color: {currentIngredient.data.rarity}");
    }
    
    void OnSlotClicked()
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} CLICKED - {currentIngredient?.data.ingredientName ?? "NULL"}");
        
        if (slotButton != null)
        {
            slotButton.interactable = false;
        }
        
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5);
        
        onClickCallback?.Invoke();
    }
    
    public void Hide()
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} Hide()");
        
        displaySequence?.Kill();
        
        Sequence hideSequence = DOTween.Sequence();
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        hideSequence.Append(transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
        hideSequence.Join(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log($"[IngredientSlot] {gameObject.name} ✅ Hide animation started");
    }
    
    public void Clear()
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} Clear()");
        
        currentIngredient = null;
        onClickCallback = null;
        gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        displaySequence?.Kill();
    }
}
