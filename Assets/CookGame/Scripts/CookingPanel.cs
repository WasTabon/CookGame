using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CookingPanel : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text recipeNameText;
    public TMP_Text turnsRemainingText;
    public TMP_Text potentialRewardText;
    
    [Header("Serve Now Button")]
    public Button serveNowButton;
    public TMP_Text serveNowButtonText;
    public Image serveNowButtonImage;
    
    [Header("Serve Now Colors")]
    public Color perfectColor = new Color(0.2f, 0.8f, 0.2f);
    public Color goodColor = new Color(0.8f, 0.8f, 0.2f);
    public Color okayColor = new Color(0.8f, 0.5f, 0.2f);
    public Color failedColor = new Color(0.5f, 0.5f, 0.5f);
    public Color disabledColor = new Color(0.3f, 0.3f, 0.3f);
    
    private CookingManager cookingManager;
    private FireBoostController fireBoostController;
    
    void Awake()
    {
        Debug.Log("[CookingPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[CookingPanel] Start() called");
        ValidateReferences();
        SetupButtons();
    }
    
    void ValidateReferences()
    {
        if (recipeNameText == null)
            Debug.LogError("[CookingPanel] ❌ Recipe Name Text is NULL!");
        if (turnsRemainingText == null)
            Debug.LogError("[CookingPanel] ❌ Turns Remaining Text is NULL!");
        if (serveNowButton == null)
            Debug.LogWarning("[CookingPanel] ⚠️ Serve Now Button is NULL!");
        if (potentialRewardText == null)
            Debug.LogWarning("[CookingPanel] ⚠️ Potential Reward Text is NULL!");
    }
    
    void SetupButtons()
    {
        if (serveNowButton != null)
        {
            serveNowButton.onClick.AddListener(OnServeNowClicked);
            Debug.Log("[CookingPanel] ✅ Serve Now button listener added");
        }
    }
    
    public void Setup(CookingManager manager, FireBoostController boostController)
    {
        Debug.Log("[CookingPanel] Setup called");
        
        cookingManager = manager;
        fireBoostController = boostController;
        
        if (cookingManager == null)
            Debug.LogError("[CookingPanel] ❌ CookingManager is NULL!");
        else
            Debug.Log("[CookingPanel] ✅ CookingManager reference set");
        
        if (fireBoostController == null)
            Debug.LogWarning("[CookingPanel] ⚠️ FireBoostController is NULL");
        else
            Debug.Log("[CookingPanel] ✅ FireBoostController reference set");
    }
    
    public void Show()
    {
        Debug.Log("[CookingPanel] Show() called");
        
        gameObject.SetActive(true);
        
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
        
        UpdateServeNowButton();
        
        Debug.Log("[CookingPanel] ✅ Show animation started");
    }
    
    public void Hide()
    {
        Debug.Log("[CookingPanel] Hide() called");
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.9f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log("[CookingPanel] ✅ Hide animation started");
    }
    
    public void UpdateUI(RecipeData recipe, int turnsRemaining)
    {
        Debug.Log($"[CookingPanel] UpdateUI - Recipe: {recipe.recipeName}, Turns: {turnsRemaining}");
        
        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.recipeName;
        }
        
        if (turnsRemainingText != null)
        {
            turnsRemainingText.text = $"Turns: {turnsRemaining}";
            turnsRemainingText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5);
        }
    }
    
    void Update()
    {
        if (cookingManager != null && cookingManager.currentRecipe != null && !cookingManager.isGameOver)
        {
            UpdateTurnsDisplay();
            UpdatePotentialReward();
            UpdateServeNowButton();
        }
    }
    
    void UpdateTurnsDisplay()
    {
        if (turnsRemainingText != null)
        {
            turnsRemainingText.text = $"Turns: {cookingManager.turnsRemaining}";
        }
    }
    
    void UpdatePotentialReward()
    {
        if (potentialRewardText != null && cookingManager != null)
        {
            int potentialReward = cookingManager.GetCurrentPotentialReward();
            potentialRewardText.text = $"Potential: {potentialReward} coins";
        }
    }
    
    void UpdateServeNowButton()
    {
        if (serveNowButton == null || cookingManager == null) return;
        
        bool canServe = cookingManager.canServeEarly && !cookingManager.isGameOver;
        serveNowButton.interactable = canServe;
        
        if (serveNowButtonImage != null)
        {
            if (!canServe)
            {
                serveNowButtonImage.color = disabledColor;
            }
            else
            {
                int metersInRange = CountMetersInRange();
                serveNowButtonImage.color = metersInRange switch
                {
                    3 => perfectColor,
                    2 => goodColor,
                    1 => okayColor,
                    _ => failedColor
                };
            }
        }
        
        if (serveNowButtonText != null)
        {
            if (!canServe)
            {
                serveNowButtonText.text = "SERVE NOW";
            }
            else
            {
                int reward = cookingManager.GetCurrentPotentialReward();
                serveNowButtonText.text = $"SERVE NOW\n({reward} Coins)";
            }
        }
    }
    
    int CountMetersInRange()
    {
        if (cookingManager == null || cookingManager.currentRecipe == null) return 0;
        
        int count = 0;
        RecipeData recipe = cookingManager.currentRecipe;
        
        if (cookingManager.tasteMeter.CurrentValue >= recipe.tasteMin && 
            cookingManager.tasteMeter.CurrentValue <= recipe.tasteMax)
            count++;
        
        if (cookingManager.stabilityMeter.CurrentValue >= recipe.stabilityMin && 
            cookingManager.stabilityMeter.CurrentValue <= recipe.stabilityMax)
            count++;
        
        if (cookingManager.magicMeter.CurrentValue >= recipe.magicMin && 
            cookingManager.magicMeter.CurrentValue <= recipe.magicMax)
            count++;
        
        return count;
    }
    
    void OnServeNowClicked()
    {
        Debug.Log("[CookingPanel] Serve Now button clicked!");
        
        if (cookingManager != null && cookingManager.canServeEarly)
        {
            serveNowButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
            cookingManager.ServeEarly();
        }
    }
}
