using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class LevelUpPopup : MonoBehaviour
{
    public static LevelUpPopup Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject popupPanel;
    public TMP_Text levelText;
    public TMP_Text congratsText;
    public TMP_Text unlocksText;
    public Button continueButton;
    
    [Header("Particles")]
    public ParticleSystem confettiParticles;
    
    [Header("Animation")]
    public float showDuration = 0.5f;
    public float hideDuration = 0.3f;
    
    private Queue<int> pendingLevelUps = new Queue<int>();
    private bool isShowing = false;
    
    void Awake()
    {
        Debug.Log("[LevelUpPopup] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[LevelUpPopup] ✅ Singleton instance created");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SetupButton();
        HideImmediate();
        SubscribeToEvents();
    }
    
    void SetupButton()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }
    
    void SubscribeToEvents()
    {
        if (PlayerProgressManager.Instance != null)
        {
            PlayerProgressManager.Instance.OnLevelUp += OnLevelUp;
        }
    }
    
    void OnLevelUp(int newLevel)
    {
        Debug.Log($"[LevelUpPopup] Level up received: {newLevel}");
        pendingLevelUps.Enqueue(newLevel);
        
        if (!isShowing)
        {
            ShowNextLevelUp();
        }
    }
    
    void ShowNextLevelUp()
    {
        if (pendingLevelUps.Count == 0)
        {
            isShowing = false;
            return;
        }
        
        int level = pendingLevelUps.Dequeue();
        Show(level);
    }
    
    public void Show(int newLevel)
    {
        Debug.Log($"[LevelUpPopup] Showing level up: {newLevel}");
        
        isShowing = true;
        
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }
        
        if (levelText != null)
        {
            levelText.text = $"LEVEL {newLevel}";
        }
        
        if (congratsText != null)
        {
            congratsText.text = GetCongratsMessage(newLevel);
        }
        
        UpdateUnlocksText(newLevel);
        
        PlayShowAnimation();
        PlayParticles();
    }
    
    string GetCongratsMessage(int level)
    {
        if (level % 10 == 0)
            return "AMAZING MILESTONE!";
        if (level % 5 == 0)
            return "FANTASTIC!";
        if (level >= 25)
            return "MASTER CHEF!";
        if (level >= 10)
            return "GREAT PROGRESS!";
        return "LEVEL UP!";
    }
    
    void UpdateUnlocksText(int newLevel)
    {
        if (unlocksText == null) return;
        
        if (RecipeUnlockManager.Instance == null)
        {
            unlocksText.text = "";
            return;
        }
        
        List<string> newUnlocks = new List<string>();
        
        foreach (var recipe in RecipeUnlockManager.Instance.allRecipes)
        {
            if (recipe != null && recipe.unlockLevel == newLevel)
            {
                newUnlocks.Add(recipe.recipeName);
            }
        }
        
        if (newUnlocks.Count > 0)
        {
            unlocksText.text = "NEW RECIPES:\n" + string.Join("\n", newUnlocks);
            unlocksText.gameObject.SetActive(true);
        }
        else
        {
            unlocksText.gameObject.SetActive(false);
        }
    }
    
    void PlayShowAnimation()
    {
        if (popupPanel == null) return;
        
        CanvasGroup canvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popupPanel.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        popupPanel.transform.localScale = Vector3.zero;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, showDuration * 0.5f));
        showSequence.Join(popupPanel.transform.DOScale(1.1f, showDuration * 0.7f).SetEase(Ease.OutBack));
        showSequence.Append(popupPanel.transform.DOScale(1f, showDuration * 0.3f));
        
        if (levelText != null)
        {
            levelText.transform.localScale = Vector3.zero;
            levelText.transform.DOScale(1f, 0.4f).SetDelay(showDuration * 0.5f).SetEase(Ease.OutElastic);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelOpen();
        }
    }
    
    void PlayParticles()
    {
        if (confettiParticles != null)
        {
            confettiParticles.Play();
        }
    }
    
    void OnContinueClicked()
    {
        Debug.Log("[LevelUpPopup] Continue clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Hide();
    }
    
    void Hide()
    {
        Debug.Log("[LevelUpPopup] Hiding");
        
        if (popupPanel == null)
        {
            ShowNextLevelUp();
            return;
        }
        
        CanvasGroup canvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = popupPanel.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, hideDuration));
        hideSequence.Join(popupPanel.transform.DOScale(0.8f, hideDuration).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => {
            popupPanel.SetActive(false);
            ShowNextLevelUp();
        });
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelClose();
        }
    }
    
    void HideImmediate()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
    
    void OnDestroy()
    {
        if (PlayerProgressManager.Instance != null)
        {
            PlayerProgressManager.Instance.OnLevelUp -= OnLevelUp;
        }
    }
}
