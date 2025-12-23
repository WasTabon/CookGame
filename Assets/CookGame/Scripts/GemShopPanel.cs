using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GemShopPanel : MonoBehaviour
{
    [Header("Currency Display")]
    public TMP_Text gemsText;
    public TMP_Text coinsText;
    
    [Header("IAP Section")]
    public Button buyGemsButton;
    public TMP_Text buyGemsButtonText;
    public TMP_Text gemPackInfoText;
    public Button restoreButton;
    
    [Header("Exchange Section")]
    public Button exchange1Button;
    public Button exchange5Button;
    public Button exchange10Button;
    public TMP_Text exchange1Text;
    public TMP_Text exchange5Text;
    public TMP_Text exchange10Text;
    
    [Header("Exchange Rates")]
    public int coinsPerGem = 50;
    
    [Header("Buttons")]
    public Button closeButton;
    
    [Header("Status")]
    public TMP_Text statusText;
    
    void Awake()
    {
        Debug.Log("[GemShopPanel] Awake() called");
    }
    
    void Start()
    {
        SetupButtons();
        SetupCurrencyDisplay();
        SetupIAPDisplay();
        UpdateExchangeButtons();
    }
    
    void SetupButtons()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
        
        if (buyGemsButton != null)
            buyGemsButton.onClick.AddListener(OnBuyGemsClicked);
        
        if (restoreButton != null)
            restoreButton.onClick.AddListener(OnRestoreClicked);
        
        if (exchange1Button != null)
            exchange1Button.onClick.AddListener(() => ExchangeGemsForCoins(1));
        
        if (exchange5Button != null)
            exchange5Button.onClick.AddListener(() => ExchangeGemsForCoins(5));
        
        if (exchange10Button != null)
            exchange10Button.onClick.AddListener(() => ExchangeGemsForCoins(10));
        
        UpdateExchangeTexts();
    }
    
    void UpdateExchangeTexts()
    {
        if (exchange1Text != null)
            exchange1Text.text = $"1 💎 → {coinsPerGem} 💰";
        
        if (exchange5Text != null)
            exchange5Text.text = $"5 💎 → {coinsPerGem * 5} 💰";
        
        if (exchange10Text != null)
            exchange10Text.text = $"10 💎 → {coinsPerGem * 10} 💰";
    }
    
    void SetupCurrencyDisplay()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;
            CurrencyManager.Instance.OnGemsChanged += UpdateGemsDisplay;
        }
    }
    
    void SetupIAPDisplay()
    {
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.OnPurchaseComplete += OnIAPPurchaseComplete;
            IAPManager.Instance.PurchaseFailed += OnIAPPurchaseFailed;
            IAPManager.Instance.OnRestoreComplete += OnIAPRestoreComplete;
            IAPManager.Instance.OnRestoreFailed += OnIAPRestoreFailed;
        }
    }
    
    public void Show()
    {
        Debug.Log("[GemShopPanel] Show()");
        
        gameObject.SetActive(true);
        
        UpdateCurrencyDisplay();
        UpdateIAPButton();
        UpdateExchangeButtons();
        ClearStatus();
        
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
        Debug.Log("[GemShopPanel] Hide()");
        
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
    
    void UpdateCurrencyDisplay()
    {
        if (CurrencyManager.Instance == null) return;
        
        UpdateGemsDisplay(CurrencyManager.Instance.Gems);
        UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
    }
    
    void UpdateGemsDisplay(int amount)
    {
        if (gemsText != null)
        {
            gemsText.text = amount.ToString();
            gemsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
        
        UpdateExchangeButtons();
    }
    
    void UpdateCoinsDisplay(int amount)
    {
        if (coinsText != null)
        {
            coinsText.text = FormatNumber(amount);
            coinsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
    }
    
    void UpdateIAPButton()
    {
        if (IAPManager.Instance != null && buyGemsButtonText != null)
        {
            string price = IAPManager.Instance.GetLocalizedPrice();
            buyGemsButtonText.text = $"BUY 50 💎\n{price}";
        }
        
        if (buyGemsButton != null)
        {
            bool canPurchase = IAPManager.Instance != null && IAPManager.Instance.CanPurchase();
            buyGemsButton.interactable = canPurchase;
        }
        
        if (gemPackInfoText != null && IAPManager.Instance != null)
        {
            gemPackInfoText.text = $"Get {IAPManager.Instance.gemsPerPurchase} gems";
        }
    }
    
    void UpdateExchangeButtons()
    {
        int gems = 0;
        if (CurrencyManager.Instance != null)
        {
            gems = CurrencyManager.Instance.Gems;
        }
        
        if (exchange1Button != null)
            exchange1Button.interactable = gems >= 1;
        
        if (exchange5Button != null)
            exchange5Button.interactable = gems >= 5;
        
        if (exchange10Button != null)
            exchange10Button.interactable = gems >= 10;
    }
    
    void OnBuyGemsClicked()
    {
        Debug.Log("[GemShopPanel] Buy Gems clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (buyGemsButton != null)
        {
            buyGemsButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
        
        SetStatus("Processing purchase...", Color.yellow);
        
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.BuyGemPack();
        }
        else
        {
            SetStatus("Store not available", Color.red);
        }
    }
    
    void OnRestoreClicked()
    {
        Debug.Log("[GemShopPanel] Restore clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        SetStatus("Restoring purchases...", Color.yellow);
        
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.RestorePurchases();
        }
        else
        {
            SetStatus("Store not available", Color.red);
        }
    }
    
    void ExchangeGemsForCoins(int gemAmount)
    {
        Debug.Log($"[GemShopPanel] Exchange {gemAmount} gems for coins");
        
        if (CurrencyManager.Instance == null) return;
        
        if (!CurrencyManager.Instance.SpendGems(gemAmount))
        {
            Debug.Log("[GemShopPanel] Not enough gems");
            SetStatus("Not enough gems!", Color.red);
            return;
        }
        
        int coinsToAdd = gemAmount * coinsPerGem;
        CurrencyManager.Instance.AddCoins(coinsToAdd);
        
        Debug.Log($"[GemShopPanel] ✅ Exchanged {gemAmount} 💎 for {coinsToAdd} 💰");
        SetStatus($"+{coinsToAdd} coins!", Color.green);
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinCollect();
        }
        
        HapticController.Success();
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.FlashSuccess();
        }
    }
    
    void OnIAPPurchaseComplete(int gemsAdded)
    {
        Debug.Log($"[GemShopPanel] IAP complete: +{gemsAdded} gems");
        SetStatus($"+{gemsAdded} gems!", Color.green);
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.FlashSuccess();
        }
    }
    
    void OnIAPPurchaseFailed(string error)
    {
        Debug.Log($"[GemShopPanel] IAP failed: {error}");
        SetStatus($"Purchase failed: {error}", Color.red);
    }
    
    void OnIAPRestoreComplete()
    {
        Debug.Log("[GemShopPanel] Restore complete");
        SetStatus("Purchases restored!", Color.green);
    }
    
    void OnIAPRestoreFailed(string error)
    {
        Debug.Log($"[GemShopPanel] Restore failed: {error}");
        SetStatus($"Restore failed: {error}", Color.red);
    }
    
    void SetStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
            statusText.gameObject.SetActive(true);
            
            DOVirtual.DelayedCall(3f, ClearStatus);
        }
    }
    
    void ClearStatus()
    {
        if (statusText != null)
        {
            statusText.gameObject.SetActive(false);
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
            CurrencyManager.Instance.OnGemsChanged -= UpdateGemsDisplay;
        }
        
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.OnPurchaseComplete -= OnIAPPurchaseComplete;
            IAPManager.Instance.PurchaseFailed -= OnIAPPurchaseFailed;
            IAPManager.Instance.OnRestoreComplete -= OnIAPRestoreComplete;
            IAPManager.Instance.OnRestoreFailed -= OnIAPRestoreFailed;
        }
    }
}
