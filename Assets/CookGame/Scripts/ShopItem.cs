using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Probability Kitchen/Shop Item")]
public class ShopItem : ScriptableObject
{
    public string itemId;
    public string itemName;
    [TextArea(2, 4)]
    public string description;
    
    public enum BoosterType
    {
        ExtraTurn,
        Shield,
        DoubleCoins,
        DoubleXP
    }
    public BoosterType boosterType;
    
    [Header("Price (Coins)")]
    public int coinPrice;
    
    [Header("Quantity")]
    public int quantity = 1;
    
    [Header("Visual")]
    public Sprite icon;
    public Color backgroundColor = Color.white;
    
    [Header("Availability")]
    public bool isAvailable = true;
    public int unlockLevel = 1;
    
    void OnEnable()
    {
        Debug.Log($"[ShopItem] Loaded: {itemName} - {coinPrice} 💰");
    }
    
    public bool CanAfford()
    {
        if (CurrencyManager.Instance == null) return false;
        return CurrencyManager.Instance.Coins >= coinPrice;
    }
    
    public string GetPriceText()
    {
        return $"{coinPrice} 💰";
    }
}
