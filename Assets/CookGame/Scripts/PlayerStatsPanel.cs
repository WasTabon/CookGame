using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatsPanel : MonoBehaviour
{
    [Header("Level Info")]
    public TMP_Text levelText;
    public TMP_Text xpText;
    public Slider xpSlider;
    public TMP_Text xpToNextText;
    
    [Header("Statistics")]
    public TMP_Text ordersCompletedText;
    public TMP_Text ordersFailedText;
    public TMP_Text successRateText;
    public TMP_Text perfectOrdersText;
    public TMP_Text currentStreakText;
    public TMP_Text highestStreakText;
    public TMP_Text jackpotsText;
    public TMP_Text totalXPText;
    
    [Header("Next Unlock")]
    public TMP_Text nextUnlockText;
    public TMP_Text levelsUntilUnlockText;
    
    [Header("Buttons")]
    public Button closeButton;
    public Button resetButton;
    
    void Awake()
    {
        Debug.Log("[PlayerStatsPanel] Awake() called");
    }
    
    void Start()
    {
        SetupButtons();
    }
    
    void SetupButtons()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetClicked);
        }
    }
    
    public void Show()
    {
        Debug.Log("[PlayerStatsPanel] Show()");
        
        gameObject.SetActive(true);
        UpdateAllStats();
        
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
        Debug.Log("[PlayerStatsPanel] Hide()");
        
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
    
    void UpdateAllStats()
    {
        if (PlayerProgressManager.Instance == null)
        {
            Debug.LogWarning("[PlayerStatsPanel] ⚠️ PlayerProgressManager is null");
            return;
        }
        
        var data = PlayerProgressManager.Instance.Data;
        var manager = PlayerProgressManager.Instance;
        
        if (levelText != null)
        {
            levelText.text = $"Level {data.level}";
        }
        
        if (xpText != null)
        {
            xpText.text = $"{data.currentXP} / {manager.GetXPForNextLevel()} XP";
        }
        
        if (xpSlider != null)
        {
            xpSlider.value = manager.GetLevelProgress();
        }
        
        if (xpToNextText != null)
        {
            int remaining = manager.GetXPForNextLevel() - data.currentXP;
            xpToNextText.text = $"{remaining} XP to next level";
        }
        
        if (ordersCompletedText != null)
        {
            ordersCompletedText.text = $"{data.ordersCompleted}";
        }
        
        if (ordersFailedText != null)
        {
            ordersFailedText.text = $"{data.ordersFailed}";
        }
        
        if (successRateText != null)
        {
            successRateText.text = $"{manager.GetSuccessRate():F1}%";
        }
        
        if (perfectOrdersText != null)
        {
            perfectOrdersText.text = $"{data.perfectOrders}";
        }
        
        if (currentStreakText != null)
        {
            currentStreakText.text = $"{data.currentStreak}";
        }
        
        if (highestStreakText != null)
        {
            highestStreakText.text = $"{data.highestStreak}";
        }
        
        if (jackpotsText != null)
        {
            jackpotsText.text = $"{data.jackpotsTriggered}";
        }
        
        if (totalXPText != null)
        {
            totalXPText.text = $"{data.totalXPEarned}";
        }
        
        UpdateNextUnlock();
    }
    
    void UpdateNextUnlock()
    {
        if (RecipeUnlockManager.Instance == null)
        {
            if (nextUnlockText != null) nextUnlockText.gameObject.SetActive(false);
            if (levelsUntilUnlockText != null) levelsUntilUnlockText.gameObject.SetActive(false);
            return;
        }
        
        var nextRecipe = RecipeUnlockManager.Instance.GetNextUnlockableRecipe();
        
        if (nextRecipe != null)
        {
            if (nextUnlockText != null)
            {
                nextUnlockText.text = $"Next: {nextRecipe.recipeName}";
                nextUnlockText.gameObject.SetActive(true);
            }
            
            if (levelsUntilUnlockText != null)
            {
                int levels = RecipeUnlockManager.Instance.GetLevelsUntilNextUnlock();
                levelsUntilUnlockText.text = levels == 1 ? "1 level away" : $"{levels} levels away";
                levelsUntilUnlockText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (nextUnlockText != null)
            {
                nextUnlockText.text = "All recipes unlocked!";
                nextUnlockText.gameObject.SetActive(true);
            }
            
            if (levelsUntilUnlockText != null)
            {
                levelsUntilUnlockText.gameObject.SetActive(false);
            }
        }
    }
    
    void OnCloseClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        Hide();
    }
    
    void OnResetClicked()
    {
        Debug.Log("[PlayerStatsPanel] Reset button clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        if (PlayerProgressManager.Instance != null)
        {
            PlayerProgressManager.Instance.ResetProgress();
        }
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetCurrency();
        }
        
        UpdateAllStats();
    }
}
