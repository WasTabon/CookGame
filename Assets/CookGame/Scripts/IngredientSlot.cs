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
    
    [Header("Selection Effect")]
    public Color selectedGlowColor = new Color(1f, 1f, 0f, 0.5f);
    
    [Header("Animation Settings")]
    public float spinDuration = 0.5f;
    public int spinRotations = 2;
    
    private IngredientInstance currentIngredient;
    private Action onClickCallback;
    private Sequence displaySequence;
    private bool isSelected = false;
    
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
        isSelected = false;
        
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
        transform.localRotation = Quaternion.identity;
        
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
        
        displaySequence.OnComplete(() => {
            UpdateUI();
            StartIdleAnimation();
        });
        
        Debug.Log($"[IngredientSlot] {gameObject.name} ✅ Display animation started");
    }
    
    void StartIdleAnimation()
    {
        if (isSelected) return;
        
        transform.DOScale(1.02f, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetId($"{gameObject.name}_idle");
    }
    
    void StopIdleAnimation()
    {
        DOTween.Kill($"{gameObject.name}_idle");
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
        
        isSelected = true;
        StopIdleAnimation();
        
        PlaySelectAnimation();
        
        onClickCallback?.Invoke();
    }
    
    void PlaySelectAnimation()
    {
        displaySequence?.Kill();
        
        Sequence selectSequence = DOTween.Sequence();
        
        selectSequence.Append(transform.DOScale(1.3f, 0.15f).SetEase(Ease.OutQuad));
        selectSequence.Append(transform.DOScale(1.1f, 0.1f).SetEase(Ease.InOutSine));
        
        if (backgroundImage != null)
        {
            Color originalColor = backgroundImage.color;
            backgroundImage.DOColor(selectedGlowColor, 0.1f)
                .OnComplete(() => backgroundImage.DOColor(originalColor, 0.2f));
        }
    }
    
    public void Hide()
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} Hide()");
        
        displaySequence?.Kill();
        StopIdleAnimation();
        
        Sequence hideSequence = DOTween.Sequence();
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        if (isSelected)
        {
            hideSequence.Append(transform.DOScale(1.5f, 0.15f).SetEase(Ease.OutQuad));
            hideSequence.Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        }
        else
        {
            hideSequence.Append(transform.DOScale(0f, 0.25f).SetEase(Ease.InBack));
        }
        
        hideSequence.Join(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.OnComplete(() => {
            gameObject.SetActive(false);
            isSelected = false;
        });
        
        Debug.Log($"[IngredientSlot] {gameObject.name} ✅ Hide animation started");
    }
    
    public void Clear()
    {
        Debug.Log($"[IngredientSlot] {gameObject.name} Clear()");
        
        StopIdleAnimation();
        currentIngredient = null;
        onClickCallback = null;
        isSelected = false;
        gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        displaySequence?.Kill();
        StopIdleAnimation();
    }
}
