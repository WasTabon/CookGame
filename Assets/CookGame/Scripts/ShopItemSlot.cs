using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI References")]
    public Image backgroundImage;
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public TMP_Text quantityText;
    public Button buyButton;
    public TMP_Text buyButtonText;
    public GameObject lockedOverlay;
    public TMP_Text lockedText;
    
    [Header("Colors")]
    public Color affordableColor = new Color(0.2f, 0.7f, 0.2f, 1f);
    public Color unaffordableColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    public Color coinColor = new Color(1f, 0.8f, 0.2f, 1f);
    
    private ShopItem currentItem;
    private Action<ShopItem> onPurchaseCallback;
    
    void Awake()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyClicked);
        }
    }
    
    public void Setup(ShopItem item, Action<ShopItem> onPurchase = null)
    {
        currentItem = item;
        onPurchaseCallback = onPurchase;
        
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (currentItem == null) return;
        
        if (nameText != null)
        {
            nameText.text = currentItem.itemName;
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = currentItem.description;
        }
        
        if (iconImage != null && currentItem.icon != null)
        {
            iconImage.sprite = currentItem.icon;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = currentItem.backgroundColor;
        }
        
        if (quantityText != null)
        {
            if (currentItem.quantity > 1)
            {
                quantityText.text = $"x{currentItem.quantity}";
                quantityText.gameObject.SetActive(true);
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }
        }
        
        UpdatePriceDisplay();
        UpdateAvailability();
    }
    
    void UpdatePriceDisplay()
    {
        if (priceText == null) return;
        
        priceText.text = currentItem.GetPriceText();
        priceText.color = coinColor;
    }
    
    void UpdateAvailability()
    {
        bool canAfford = currentItem.CanAfford();
        bool isUnlocked = true;
        
        if (PlayerProgressManager.Instance != null)
        {
            isUnlocked = PlayerProgressManager.Instance.Data.level >= currentItem.unlockLevel;
        }
        
        if (lockedOverlay != null)
        {
            lockedOverlay.SetActive(!isUnlocked);
            
            if (!isUnlocked && lockedText != null)
            {
                lockedText.text = $"Level {currentItem.unlockLevel}";
            }
        }
        
        if (buyButton != null)
        {
            buyButton.interactable = canAfford && isUnlocked && currentItem.isAvailable;
            
            Image buttonImage = buyButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = canAfford ? affordableColor : unaffordableColor;
            }
        }
        
        if (buyButtonText != null)
        {
            if (!isUnlocked)
            {
                buyButtonText.text = "LOCKED";
            }
            else if (!canAfford)
            {
                buyButtonText.text = "CAN'T AFFORD";
            }
            else
            {
                buyButtonText.text = "BUY";
            }
        }
    }
    
    void OnBuyClicked()
    {
        if (currentItem == null) return;
        
        Debug.Log($"[ShopItemSlot] Buy clicked: {currentItem.itemName}");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        buyButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        
        if (onPurchaseCallback != null)
        {
            onPurchaseCallback.Invoke(currentItem);
        }
        else if (ShopManager.Instance != null)
        {
            if (ShopManager.Instance.TryPurchase(currentItem))
            {
                PlayPurchaseAnimation();
                UpdateAvailability();
            }
            else
            {
                PlayFailAnimation();
            }
        }
    }
    
    void PlayPurchaseAnimation()
    {
        if (iconImage != null)
        {
            iconImage.transform.DOPunchScale(Vector3.one * 0.3f, 0.4f, 5);
        }
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.FlashSuccess();
        }
    }
    
    void PlayFailAnimation()
    {
        transform.DOShakePosition(0.3f, 10f, 20);
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.FlashOverflow();
        }
    }
    
    public void RefreshAvailability()
    {
        UpdateAvailability();
    }
}
