using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class OrderPanel : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text recipeNameText;
    public TMP_Text tasteTargetText;
    public TMP_Text stabilityTargetText;
    public TMP_Text magicTargetText;
    public TMP_Text turnsText;
    public TMP_Text difficultyText;
    
    [Header("Button References")]
    public Button acceptButton;
    public Button declineButton;
    
    [Header("Visual")]
    public Image recipeIcon;
    
    private RecipeData currentRecipe;
    
    void Awake()
    {
        Debug.Log("[OrderPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[OrderPanel] Start() called");
        ValidateReferences();
        SetupButtons();
    }
    
    void ValidateReferences()
    {
        if (recipeNameText == null)
            Debug.LogWarning("[OrderPanel] ⚠️ Recipe Name Text is NULL");
        if (acceptButton == null)
            Debug.LogError("[OrderPanel] ❌ Accept Button is NULL!");
    }
    
    void SetupButtons()
    {
        if (acceptButton != null)
        {
            acceptButton.onClick.AddListener(OnAcceptClicked);
            Debug.Log("[OrderPanel] ✅ Accept button listener added");
        }
        
        if (declineButton != null)
        {
            declineButton.onClick.AddListener(OnDeclineClicked);
            Debug.Log("[OrderPanel] ✅ Decline button listener added");
        }
    }
    
    public void DisplayOrder(RecipeData recipe)
    {
        Debug.Log($"[OrderPanel] DisplayOrder: {recipe?.recipeName ?? "NULL"}");
        
        if (recipe == null)
        {
            Debug.LogError("[OrderPanel] ❌ Recipe is NULL!");
            return;
        }
        
        currentRecipe = recipe;
        
        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.recipeName;
            Debug.Log($"[OrderPanel] Recipe name set: {recipe.recipeName}");
        }
        
        if (tasteTargetText != null)
        {
            tasteTargetText.text = $"Taste: {recipe.tasteMin:F0} - {recipe.tasteMax:F0}";
        }
        
        if (stabilityTargetText != null)
        {
            stabilityTargetText.text = $"Stability: {recipe.stabilityMin:F0} - {recipe.stabilityMax:F0}";
        }
        
        if (magicTargetText != null)
        {
            magicTargetText.text = $"Magic: {recipe.magicMin:F0} - {recipe.magicMax:F0}";
        }
        
        if (turnsText != null)
        {
            turnsText.text = $"Turns: {recipe.totalTurns}";
        }
        
        if (difficultyText != null)
        {
            difficultyText.text = recipe.difficulty.ToString();
            
            Color diffColor = recipe.difficulty switch
            {
                RecipeData.Difficulty.Easy => Color.green,
                RecipeData.Difficulty.Medium => Color.yellow,
                RecipeData.Difficulty.Hard => new Color(1f, 0.5f, 0f),
                RecipeData.Difficulty.Elite => Color.red,
                _ => Color.white
            };
            difficultyText.color = diffColor;
        }
        
        if (recipeIcon != null && recipe.icon != null)
        {
            recipeIcon.sprite = recipe.icon;
        }
        
        Debug.Log("[OrderPanel] ✅ Order displayed");
    }
    
    void OnAcceptClicked()
    {
        Debug.Log("[OrderPanel] Accept button clicked!");
        
        acceptButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        
        GameManager.Instance.uiManager.HideOrderPanel();
        GameManager.Instance.StartCooking();
    }
    
    void OnDeclineClicked()
    {
        Debug.Log("[OrderPanel] Decline button clicked!");
        
        if (declineButton != null)
        {
            declineButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
        
        GameManager.Instance.uiManager.HideOrderPanel();
    }
    
    public void Show()
    {
        Debug.Log("[OrderPanel] Show()");
        
        gameObject.SetActive(true);
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.8f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.3f));
        showSequence.Join(transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        
        AnimateTexts();
        
        Debug.Log("[OrderPanel] ✅ Show animation started");
    }
    
    void AnimateTexts()
    {
        float delay = 0.1f;
        
        TMP_Text[] texts = { recipeNameText, tasteTargetText, stabilityTargetText, magicTargetText, turnsText };
        
        foreach (var text in texts)
        {
            if (text != null)
            {
                text.transform.localScale = Vector3.zero;
                text.transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                delay += 0.1f;
            }
        }
    }
    
    public void Hide()
    {
        Debug.Log("[OrderPanel] Hide()");
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log("[OrderPanel] ✅ Hide animation started");
    }
}
