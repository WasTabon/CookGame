using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    private const string COINS_KEY = "PlayerCoins";
    private const string GEMS_KEY = "PlayerGems";
    
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;
    
    void Awake()
    {
        Debug.Log("[CurrencyManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCurrency();
            Debug.Log("[CurrencyManager] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[CurrencyManager] ⚠️ Duplicate detected, destroying...");
            Destroy(gameObject);
        }
    }
    
    void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
        
        Debug.Log($"[CurrencyManager] ✅ Loaded - Coins: {Coins}, Gems: {Gems}");
    }
    
    void SaveCurrency()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
        
        Debug.Log($"[CurrencyManager] ✅ Saved - Coins: {Coins}, Gems: {Gems}");
    }
    
    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CurrencyManager] ⚠️ Tried to add {amount} coins (must be positive)");
            return;
        }
        
        int oldValue = Coins;
        Coins += amount;
        SaveCurrency();
        
        Debug.Log($"[CurrencyManager] 💰 Coins: {oldValue} + {amount} = {Coins}");
        OnCoinsChanged?.Invoke(Coins);
    }
    
    public bool SpendCoins(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CurrencyManager] ⚠️ Tried to spend {amount} coins (must be positive)");
            return false;
        }
        
        if (Coins < amount)
        {
            Debug.Log($"[CurrencyManager] ❌ Not enough coins: {Coins} < {amount}");
            return false;
        }
        
        int oldValue = Coins;
        Coins -= amount;
        SaveCurrency();
        
        Debug.Log($"[CurrencyManager] 💰 Coins: {oldValue} - {amount} = {Coins}");
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }
    
    public void AddGems(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CurrencyManager] ⚠️ Tried to add {amount} gems (must be positive)");
            return;
        }
        
        int oldValue = Gems;
        Gems += amount;
        SaveCurrency();
        
        Debug.Log($"[CurrencyManager] 💎 Gems: {oldValue} + {amount} = {Gems}");
        OnGemsChanged?.Invoke(Gems);
    }
    
    public bool SpendGems(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[CurrencyManager] ⚠️ Tried to spend {amount} gems (must be positive)");
            return false;
        }
        
        if (Gems < amount)
        {
            Debug.Log($"[CurrencyManager] ❌ Not enough gems: {Gems} < {amount}");
            return false;
        }
        
        int oldValue = Gems;
        Gems -= amount;
        SaveCurrency();
        
        Debug.Log($"[CurrencyManager] 💎 Gems: {oldValue} - {amount} = {Gems}");
        OnGemsChanged?.Invoke(Gems);
        return true;
    }
    
    public void ResetCurrency()
    {
        Debug.Log("[CurrencyManager] ⚠️ Resetting all currency!");
        Coins = 0;
        Gems = 0;
        SaveCurrency();
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
    }
}
