using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class ShopPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text coinsText;
    
    [Header("Content")]
    public Transform itemsContainer;
    public GameObject itemSlotPrefab;
    
    [Header("Buttons")]
    public Button closeButton;
    
    private List<ShopItemSlot> spawnedSlots = new List<ShopItemSlot>();
    
    void Awake()
    {
        Debug.Log("[ShopPanel] Awake() called");
    }
    
    void Start()
    {
        SetupButtons();
        SetupCurrencyDisplay();
    }
    
    void SetupButtons()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
    }
    
    void SetupCurrencyDisplay()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;
        }
    }
    
    public void Show()
    {
        Debug.Log("[ShopPanel] Show()");
        
        gameObject.SetActive(true);
        
        UpdateCurrencyDisplay();
        PopulateItems();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelOpen();
        }
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.8f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.3f));
        showSequence.Join(transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
    }
    
    public void Hide()
    {
        Debug.Log("[ShopPanel] Hide()");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelClose();
        }
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
    }
    
    void PopulateItems()
    {
        ClearItems();
        
        if (ShopManager.Instance == null)
        {
            Debug.LogWarning("[ShopPanel] ⚠️ ShopManager not found");
            return;
        }
        
        List<ShopItem> items = ShopManager.Instance.GetAvailableItems();
        
        Debug.Log($"[ShopPanel] Populating {items.Count} items");
        
        foreach (var item in items)
        {
            CreateItemSlot(item);
        }
    }
    
    void CreateItemSlot(ShopItem item)
    {
        if (itemsContainer == null || itemSlotPrefab == null)
        {
            Debug.LogWarning("[ShopPanel] ⚠️ Missing container or prefab");
            return;
        }
        
        GameObject slotObj = Instantiate(itemSlotPrefab, itemsContainer);
        ShopItemSlot slot = slotObj.GetComponent<ShopItemSlot>();
        
        if (slot != null)
        {
            slot.Setup(item, OnItemPurchaseAttempt);
            spawnedSlots.Add(slot);
        }
        
        slotObj.transform.localScale = Vector3.zero;
        slotObj.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(spawnedSlots.Count * 0.05f);
    }
    
    void ClearItems()
    {
        foreach (var slot in spawnedSlots)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        spawnedSlots.Clear();
    }
    
    void OnItemPurchaseAttempt(ShopItem item)
    {
        Debug.Log($"[ShopPanel] Purchase attempt: {item.itemName}");
        
        if (ShopManager.Instance != null)
        {
            if (ShopManager.Instance.TryPurchase(item))
            {
                RefreshAllSlots();
            }
        }
    }
    
    void RefreshAllSlots()
    {
        foreach (var slot in spawnedSlots)
        {
            if (slot != null)
            {
                slot.RefreshAvailability();
            }
        }
    }
    
    void UpdateCurrencyDisplay()
    {
        if (CurrencyManager.Instance == null) return;
        
        UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
    }
    
    void UpdateCoinsDisplay(int amount)
    {
        if (coinsText != null)
        {
            coinsText.text = FormatNumber(amount);
            coinsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
        
        RefreshAllSlots();
    }
    
    string FormatNumber(int number)
    {
        if (number >= 1000000)
            return (number / 1000000f).ToString("F1") + "M";
        if (number >= 1000)
            return (number / 1000f).ToString("F1") + "K";
        return number.ToString();
    }
    
    void OnCloseClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        Hide();
    }
    
    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinsDisplay;
        }
    }
}
