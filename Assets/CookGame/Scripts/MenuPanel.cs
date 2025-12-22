using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuPanel : MonoBehaviour
{
    [Header("UI References")]
    public Button getOrderButton;
    
    [Header("Currency Display")]
    public TMP_Text coinsText;
    public TMP_Text gemsText;
    
    void Awake()
    {
        Debug.Log("[MenuPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[MenuPanel] Start() called");
        ValidateReferences();
        SetupButtons();
        SetupCurrencyDisplay();
    }
    
    void ValidateReferences()
    {
        if (getOrderButton == null)
            Debug.LogError("[MenuPanel] ❌ Get Order Button is NULL!");
        else
            Debug.Log("[MenuPanel] ✅ Get Order Button found");
    }
    
    void SetupButtons()
    {
        if (getOrderButton != null)
        {
            getOrderButton.onClick.AddListener(OnGetOrderClicked);
            Debug.Log("[MenuPanel] ✅ Get Order button listener added");
        }
    }
    
    void SetupCurrencyDisplay()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoinsDisplay;
            CurrencyManager.Instance.OnGemsChanged += UpdateGemsDisplay;
            
            UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
            UpdateGemsDisplay(CurrencyManager.Instance.Gems);
            
            Debug.Log("[MenuPanel] ✅ Currency display setup complete");
        }
        else
        {
            Debug.LogWarning("[MenuPanel] ⚠️ CurrencyManager.Instance is NULL");
        }
    }
    
    void UpdateCoinsDisplay(int amount)
    {
        if (coinsText != null)
        {
            coinsText.text = FormatNumber(amount);
            coinsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
    }
    
    void UpdateGemsDisplay(int amount)
    {
        if (gemsText != null)
        {
            gemsText.text = FormatNumber(amount);
            gemsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
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
    
    void OnGetOrderClicked()
    {
        Debug.Log("[MenuPanel] Get Order button clicked!");
        
        getOrderButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        
        RecipeData newOrder = GameManager.Instance.orderManager.GenerateRandomOrder();
        
        if (newOrder != null)
        {
            Debug.Log($"[MenuPanel] Showing order: {newOrder.recipeName}");
            GameManager.Instance.uiManager.ShowOrderPanel(newOrder);
            Hide();
        }
        else
        {
            Debug.LogError("[MenuPanel] ❌ Failed to generate order!");
        }
    }
    
    public void Show()
    {
        Debug.Log("[MenuPanel] Show()");
        
        gameObject.SetActive(true);
        
        if (CurrencyManager.Instance != null)
        {
            UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
            UpdateGemsDisplay(CurrencyManager.Instance.Gems);
        }
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.9f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.3f));
        showSequence.Join(transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        
        Debug.Log("[MenuPanel] ✅ Show animation started");
    }
    
    public void Hide()
    {
        Debug.Log("[MenuPanel] Hide()");
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.9f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log("[MenuPanel] ✅ Hide animation started");
    }
    
    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoinsDisplay;
            CurrencyManager.Instance.OnGemsChanged -= UpdateGemsDisplay;
        }
    }
}
