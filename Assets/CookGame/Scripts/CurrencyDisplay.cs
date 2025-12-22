using UnityEngine;
using TMPro;
using DG.Tweening;

public class CurrencyDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text coinsText;
    public TMP_Text gemsText;
    
    [Header("Animation")]
    public float punchScale = 0.2f;
    public float punchDuration = 0.3f;
    
    void Start()
    {
        Debug.Log("[CurrencyDisplay] Start() called");
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += OnCoinsChanged;
            CurrencyManager.Instance.OnGemsChanged += OnGemsChanged;
            
            UpdateDisplay();
        }
        else
        {
            Debug.LogError("[CurrencyDisplay] ❌ CurrencyManager.Instance is NULL!");
        }
    }
    
    void UpdateDisplay()
    {
        if (CurrencyManager.Instance == null) return;
        
        if (coinsText != null)
        {
            coinsText.text = FormatNumber(CurrencyManager.Instance.Coins);
        }
        
        if (gemsText != null)
        {
            gemsText.text = FormatNumber(CurrencyManager.Instance.Gems);
        }
    }
    
    void OnCoinsChanged(int newValue)
    {
        Debug.Log($"[CurrencyDisplay] Coins changed to: {newValue}");
        
        if (coinsText != null)
        {
            coinsText.text = FormatNumber(newValue);
            coinsText.transform.DOPunchScale(Vector3.one * punchScale, punchDuration, 5);
        }
    }
    
    void OnGemsChanged(int newValue)
    {
        Debug.Log($"[CurrencyDisplay] Gems changed to: {newValue}");
        
        if (gemsText != null)
        {
            gemsText.text = FormatNumber(newValue);
            gemsText.transform.DOPunchScale(Vector3.one * punchScale, punchDuration, 5);
        }
    }
    
    string FormatNumber(int number)
    {
        if (number >= 1000000)
            return (number / 1000000f).ToString("F1") + "M";
        if (number >= 1000)
            return (number / 1000f).ToString("F1") + "K";
        return number.ToString();
    }
    
    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= OnCoinsChanged;
            CurrencyManager.Instance.OnGemsChanged -= OnGemsChanged;
        }
    }
}
