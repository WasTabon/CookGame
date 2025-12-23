using UnityEngine;
using System;

[System.Serializable]
public class BoosterInventory
{
    public int extraTurns = 0;
    public int shields = 0;
    public int doubleCoins = 0;
    public int doubleXP = 0;
}

[System.Serializable]
public class ActiveBoosters
{
    public bool doubleCoinsActive = false;
    public bool doubleXPActive = false;
    public int extraTurnsThisGame = 0;
    public bool shieldActive = false;
}

public class BoosterManager : MonoBehaviour
{
    public static BoosterManager Instance { get; private set; }
    
    private const string SAVE_KEY = "BoosterInventory";
    
    public BoosterInventory Inventory { get; private set; }
    public ActiveBoosters ActiveBoosters { get; private set; }
    
    public event Action<ShopItem.BoosterType, int> OnBoosterCountChanged;
    public event Action<ShopItem.BoosterType> OnBoosterUsed;
    public event Action<ShopItem.BoosterType> OnBoosterActivated;
    
    void Awake()
    {
        Debug.Log("[BoosterManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
            ActiveBoosters = new ActiveBoosters();
            Debug.Log("[BoosterManager] ✅ Singleton created");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetBoosterCount(ShopItem.BoosterType type)
    {
        return type switch
        {
            ShopItem.BoosterType.ExtraTurn => Inventory.extraTurns,
            ShopItem.BoosterType.Shield => Inventory.shields,
            ShopItem.BoosterType.DoubleCoins => Inventory.doubleCoins,
            ShopItem.BoosterType.DoubleXP => Inventory.doubleXP,
            _ => 0
        };
    }
    
    public void AddBooster(ShopItem.BoosterType type, int amount = 1)
    {
        Debug.Log($"[BoosterManager] Adding {amount}x {type}");
        
        switch (type)
        {
            case ShopItem.BoosterType.ExtraTurn:
                Inventory.extraTurns += amount;
                break;
            case ShopItem.BoosterType.Shield:
                Inventory.shields += amount;
                break;
            case ShopItem.BoosterType.DoubleCoins:
                Inventory.doubleCoins += amount;
                break;
            case ShopItem.BoosterType.DoubleXP:
                Inventory.doubleXP += amount;
                break;
        }
        
        SaveInventory();
        OnBoosterCountChanged?.Invoke(type, GetBoosterCount(type));
    }
    
    public bool UseBooster(ShopItem.BoosterType type)
    {
        int count = GetBoosterCount(type);
        if (count <= 0)
        {
            Debug.Log($"[BoosterManager] ❌ No {type} boosters available");
            return false;
        }
        
        Debug.Log($"[BoosterManager] Using {type} booster");
        
        switch (type)
        {
            case ShopItem.BoosterType.ExtraTurn:
                Inventory.extraTurns--;
                break;
            case ShopItem.BoosterType.Shield:
                Inventory.shields--;
                break;
            case ShopItem.BoosterType.DoubleCoins:
                Inventory.doubleCoins--;
                ActiveBoosters.doubleCoinsActive = true;
                break;
            case ShopItem.BoosterType.DoubleXP:
                Inventory.doubleXP--;
                ActiveBoosters.doubleXPActive = true;
                break;
        }
        
        SaveInventory();
        OnBoosterUsed?.Invoke(type);
        OnBoosterCountChanged?.Invoke(type, GetBoosterCount(type));
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        HapticController.MediumImpact();
        
        return true;
    }
    
    public void ActivateBoosterForGame(ShopItem.BoosterType type)
    {
        if (!UseBooster(type)) return;
        
        Debug.Log($"[BoosterManager] ⚡ {type} activated for this game");
        OnBoosterActivated?.Invoke(type);
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.FlashSuccess();
        }
    }
    
    public void ResetActiveBoostersForNewGame()
    {
        Debug.Log("[BoosterManager] Resetting active boosters for new game");
        ActiveBoosters.doubleCoinsActive = false;
        ActiveBoosters.doubleXPActive = false;
        ActiveBoosters.extraTurnsThisGame = 0;
        ActiveBoosters.shieldActive = false;
    }
    
    public float GetCoinMultiplier()
    {
        return ActiveBoosters.doubleCoinsActive ? 2f : 1f;
    }
    
    public float GetXPMultiplier()
    {
        return ActiveBoosters.doubleXPActive ? 2f : 1f;
    }
    
    public bool HasBooster(ShopItem.BoosterType type)
    {
        return GetBoosterCount(type) > 0;
    }
    
    void SaveInventory()
    {
        string json = JsonUtility.ToJson(Inventory);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("[BoosterManager] 💾 Inventory saved");
    }
    
    void LoadInventory()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            Inventory = JsonUtility.FromJson<BoosterInventory>(json);
            Debug.Log("[BoosterManager] 📂 Inventory loaded");
        }
        else
        {
            Inventory = new BoosterInventory();
            Debug.Log("[BoosterManager] 📂 New inventory created");
        }
        
        LogInventory();
    }
    
    void LogInventory()
    {
        Debug.Log($"[BoosterManager] Inventory: ExtraTurn={Inventory.extraTurns}, Shield={Inventory.shields}, " +
                  $"DoubleCoins={Inventory.doubleCoins}, DoubleXP={Inventory.doubleXP}");
    }
    
    public void ResetInventory()
    {
        Debug.Log("[BoosterManager] ⚠️ Resetting inventory!");
        Inventory = new BoosterInventory();
        ActiveBoosters = new ActiveBoosters();
        SaveInventory();
    }
}
