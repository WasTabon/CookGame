using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuPanel : MonoBehaviour
{
    [Header("UI References")]
    public Button getOrderButton;
    public Button statsButton;
    public Button settingsButton;
    public Button shopButton;
    public Button gemShopButton;
    
    [Header("Currency Display")]
    public TMP_Text coinsText;
    public TMP_Text gemsText;
    
    [Header("Level Display")]
    public TMP_Text levelText;
    public Slider xpSlider;
    public TMP_Text xpText;
    public TMP_Text streakText;
    
    [Header("Panels")]
    public PlayerStatsPanel statsPanel;
    public SettingsPanel settingsPanel;
    public ShopPanel shopPanel;
    public GemShopPanel gemShopPanel;
    
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
        SetupProgressDisplay();
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
        
        if (statsButton != null)
        {
            statsButton.onClick.AddListener(OnStatsClicked);
            Debug.Log("[MenuPanel] ✅ Stats button listener added");
        }
        
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClicked);
            Debug.Log("[MenuPanel] ✅ Settings button listener added");
        }
        
        if (shopButton != null)
        {
            shopButton.onClick.AddListener(OnShopClicked);
            Debug.Log("[MenuPanel] ✅ Shop button listener added");
        }
        
        if (gemShopButton != null)
        {
            gemShopButton.onClick.AddListener(OnGemShopClicked);
            Debug.Log("[MenuPanel] ✅ Gem Shop button listener added");
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
    
    void SetupProgressDisplay()
    {
        if (PlayerProgressManager.Instance != null)
        {
            PlayerProgressManager.Instance.OnXPChanged += UpdateXPDisplay;
            PlayerProgressManager.Instance.OnLevelUp += OnLevelUp;
            PlayerProgressManager.Instance.OnStreakChanged += UpdateStreakDisplay;
            
            UpdateLevelDisplay();
            UpdateStreakDisplay(PlayerProgressManager.Instance.Data.currentStreak);
            
            Debug.Log("[MenuPanel] ✅ Progress display setup complete");
        }
        else
        {
            Debug.LogWarning("[MenuPanel] ⚠️ PlayerProgressManager.Instance is NULL");
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
            gemsText.text = amount.ToString();
            gemsText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
    }
    
    void UpdateLevelDisplay()
    {
        if (PlayerProgressManager.Instance == null) return;
        
        var data = PlayerProgressManager.Instance.Data;
        var manager = PlayerProgressManager.Instance;
        
        if (levelText != null)
        {
            levelText.text = $"Lv.{data.level}";
        }
        
        if (xpSlider != null)
        {
            xpSlider.value = manager.GetLevelProgress();
        }
        
        if (xpText != null)
        {
            xpText.text = $"{data.currentXP}/{manager.GetXPForNextLevel()}";
        }
    }
    
    void UpdateXPDisplay(int currentXP, int xpNeeded)
    {
        if (xpSlider != null)
        {
            float progress = xpNeeded > 0 ? (float)currentXP / xpNeeded : 0f;
            xpSlider.DOValue(progress, 0.5f).SetEase(Ease.OutQuad);
        }
        
        if (xpText != null)
        {
            xpText.text = $"{currentXP}/{xpNeeded}";
            xpText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
    }
    
    void OnLevelUp(int newLevel)
    {
        if (levelText != null)
        {
            levelText.text = $"Lv.{newLevel}";
            levelText.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 5);
        }
        
        if (xpSlider != null)
        {
            xpSlider.value = 0f;
        }
        
        UpdateLevelDisplay();
    }
    
    void UpdateStreakDisplay(int streak)
    {
        if (streakText != null)
        {
            if (streak > 0)
            {
                streakText.text = $"🔥 {streak}";
                streakText.gameObject.SetActive(true);
                streakText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5);
            }
            else
            {
                streakText.gameObject.SetActive(false);
            }
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
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
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
    
    void OnStatsClicked()
    {
        Debug.Log("[MenuPanel] Stats button clicked!");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (statsPanel != null)
        {
            statsPanel.Show();
        }
    }
    
    void OnSettingsClicked()
    {
        Debug.Log("[MenuPanel] Settings button clicked!");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (settingsPanel != null)
        {
            settingsPanel.Show();
        }
    }
    
    void OnShopClicked()
    {
        Debug.Log("[MenuPanel] Shop button clicked!");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (shopPanel != null)
        {
            shopPanel.Show();
        }
    }
    
    void OnGemShopClicked()
    {
        Debug.Log("[MenuPanel] Gem Shop button clicked!");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (gemShopPanel != null)
        {
            gemShopPanel.Show();
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
        
        UpdateLevelDisplay();
        
        if (PlayerProgressManager.Instance != null)
        {
            UpdateStreakDisplay(PlayerProgressManager.Instance.Data.currentStreak);
        }
        
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
        
        if (PlayerProgressManager.Instance != null)
        {
            PlayerProgressManager.Instance.OnXPChanged -= UpdateXPDisplay;
            PlayerProgressManager.Instance.OnLevelUp -= OnLevelUp;
            PlayerProgressManager.Instance.OnStreakChanged -= UpdateStreakDisplay;
        }
    }
}
