using UnityEngine;
using System;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    
    [Header("Shop Items (Boosters)")]
    public List<ShopItem> allItems = new List<ShopItem>();
    
    public event Action<ShopItem> OnItemPurchased;
    public event Action<ShopItem, string> OnPurchaseFailed;
    
    void Awake()
    {
        Debug.Log("[ShopManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[ShopManager] ✅ Singleton created");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ValidateItems();
    }
    
    void ValidateItems()
    {
        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogWarning("[ShopManager] ⚠️ No shop items configured!");
            return;
        }
        
        Debug.Log($"[ShopManager] ✅ {allItems.Count} shop items loaded");
        foreach (var item in allItems)
        {
            if (item != null)
            {
                Debug.Log($"[ShopManager]   - {item.itemName}: {item.GetPriceText()}");
            }
        }
    }
    
    public List<ShopItem> GetAvailableItems()
    {
        List<ShopItem> available = new List<ShopItem>();
        
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        
        foreach (var item in allItems)
        {
            if (item != null && item.isAvailable && item.unlockLevel <= playerLevel)
            {
                available.Add(item);
            }
        }
        
        return available;
    }
    
    public bool CanPurchase(ShopItem item)
    {
        if (item == null) return false;
        if (!item.isAvailable) return false;
        if (!item.CanAfford()) return false;
        
        int playerLevel = 1;
        if (PlayerProgressManager.Instance != null)
        {
            playerLevel = PlayerProgressManager.Instance.Data.level;
        }
        if (item.unlockLevel > playerLevel) return false;
        
        return true;
    }
    
    public bool TryPurchase(ShopItem item)
    {
        Debug.Log($"[ShopManager] Attempting to purchase: {item?.itemName ?? "NULL"}");
        
        if (item == null)
        {
            OnPurchaseFailed?.Invoke(item, "Invalid item");
            return false;
        }
        
        if (!item.isAvailable)
        {
            Debug.Log("[ShopManager] ❌ Item not available");
            OnPurchaseFailed?.Invoke(item, "Item not available");
            return false;
        }
        
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("[ShopManager] ❌ CurrencyManager not found!");
            OnPurchaseFailed?.Invoke(item, "Currency system error");
            return false;
        }
        
        if (!CurrencyManager.Instance.SpendCoins(item.coinPrice))
        {
            Debug.Log($"[ShopManager] ❌ Not enough coins: need {item.coinPrice}");
            OnPurchaseFailed?.Invoke(item, "Not enough coins");
            return false;
        }
        
        ApplyPurchase(item);
        
        Debug.Log($"[ShopManager] ✅ Purchase successful: {item.itemName}");
        OnItemPurchased?.Invoke(item);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinCollect();
        }
        
        HapticController.Success();
        
        return true;
    }
    
    void ApplyPurchase(ShopItem item)
    {
        if (BoosterManager.Instance == null)
        {
            Debug.LogError("[ShopManager] ❌ BoosterManager not found!");
            return;
        }
        
        BoosterManager.Instance.AddBooster(item.boosterType, item.quantity);
        Debug.Log($"[ShopManager] Added {item.quantity}x {item.boosterType}");
    }
}
