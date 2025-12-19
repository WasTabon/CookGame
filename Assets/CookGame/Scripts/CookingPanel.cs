using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CookingPanel : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text recipeNameText;
    public TMP_Text turnsRemainingText;
    
    [Header("Fire Boost UI")]
    public Button fireBoostButton;
    public TMP_Text fireBoostButtonText;
    public Image fireBoostTimerFill;
    public TMP_Text fireBoostTimerText;
    
    [Header("Fire Boost Duration Panel")]
    public GameObject durationPanel;
    public Button duration2sButton;
    public Button duration3sButton;
    public Button duration5sButton;
    
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
    }
    
    void ValidateReferences()
    {
        if (recipeNameText == null)
            Debug.LogError("[CookingPanel] ❌ Recipe Name Text is NULL!");
        if (turnsRemainingText == null)
            Debug.LogError("[CookingPanel] ❌ Turns Remaining Text is NULL!");
        if (fireBoostButton == null)
            Debug.LogWarning("[CookingPanel] ⚠️ Fire Boost Button is NULL - Fire Boost UI disabled");
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
        if (cookingManager != null && cookingManager.currentRecipe != null)
        {
            UpdateTurnsDisplay();
        }
    }
    
    void UpdateTurnsDisplay()
    {
        if (turnsRemainingText != null)
        {
            turnsRemainingText.text = $"Turns: {cookingManager.turnsRemaining}";
        }
    }
}
