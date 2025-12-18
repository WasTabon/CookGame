using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup menuPanel;
    public CanvasGroup orderPanel;
    public CanvasGroup cookingPanel;
    public CanvasGroup resultPanel;
    
    [Header("Script References")]
    public OrderPanel orderPanelScript;
    public CookingPanel cookingPanelScript;
    public ResultPanel resultPanelScript;
    public CookingManager cookingManager;
    
    void Awake()
    {
        Debug.Log("[UIManager] Awake() called");
        
        if (menuPanel == null) Debug.LogError("[UIManager] ❌ menuPanel reference is NULL!");
        if (orderPanel == null) Debug.LogError("[UIManager] ❌ orderPanel reference is NULL!");
        if (cookingPanel == null) Debug.LogError("[UIManager] ❌ cookingPanel reference is NULL!");
        if (resultPanel == null) Debug.LogError("[UIManager] ❌ resultPanel reference is NULL!");
        if (orderPanelScript == null) Debug.LogError("[UIManager] ❌ orderPanelScript is NULL!");
        if (cookingPanelScript == null) Debug.LogError("[UIManager] ❌ cookingPanelScript is NULL!");
        if (resultPanelScript == null) Debug.LogError("[UIManager] ❌ resultPanelScript is NULL!");
        if (cookingManager == null) Debug.LogError("[UIManager] ❌ cookingManager is NULL!");
    }
    
    void Start()
    {
        Debug.Log("[UIManager] Start() - initializing panel states");
        
        if (menuPanel != null)
        {
            menuPanel.alpha = 0;
            menuPanel.gameObject.SetActive(false);
        }
        
        if (orderPanel != null)
        {
            orderPanel.alpha = 0;
            orderPanel.gameObject.SetActive(false);
        }
        
        if (cookingPanel != null)
        {
            cookingPanel.alpha = 0;
            cookingPanel.gameObject.SetActive(false);
        }
        
        if (resultPanel != null)
        {
            resultPanel.alpha = 0;
            resultPanel.gameObject.SetActive(false);
        }
    }
    
    public void ShowMenuPanel()
    {
        Debug.Log("[UIManager] ShowMenuPanel() called");
        
        HideAllPanels();
        
        if (menuPanel != null)
        {
            menuPanel.gameObject.SetActive(true);
            menuPanel.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            menuPanel.transform.DOScale(Vector3.one, 0.5f).From(Vector3.one * 0.8f).SetEase(Ease.OutBack);
        }
    }
    
    public void ShowOrderPanel(RecipeData recipe)
    {
        Debug.Log($"[UIManager] ShowOrderPanel() with recipe: {recipe.recipeName}");
        
        HideAllPanels();
        
        if (orderPanel != null)
        {
            orderPanel.gameObject.SetActive(true);
            orderPanel.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            orderPanel.transform.DOScale(Vector3.one, 0.5f).From(Vector3.one * 0.8f).SetEase(Ease.OutBack);
            
            if (orderPanelScript != null)
                orderPanelScript.DisplayRecipe(recipe);
        }
    }
    
    public void ShowCookingPanel(RecipeData recipe)
    {
        Debug.Log($"[UIManager] ShowCookingPanel() with recipe: {recipe.recipeName}");
        
        HideAllPanels();
        
        if (cookingPanel != null)
        {
            cookingPanel.gameObject.SetActive(true);
            cookingPanel.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            
            if (cookingPanelScript != null)
                cookingPanelScript.Initialize(recipe, cookingManager);
            
            if (cookingManager != null)
            {
                Debug.Log("[UIManager] Starting cooking...");
                cookingManager.StartCooking(recipe);
            }
        }
    }
    
    public void ShowResultPanel(bool victory)
    {
        Debug.Log($"[UIManager] ShowResultPanel() - Victory: {victory}");
        
        HideAllPanels();
        
        if (resultPanel != null)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            
            if (resultPanelScript != null)
                resultPanelScript.ShowResult(victory);
        }
    }
    
    void HideAllPanels()
    {
        Debug.Log("[UIManager] Hiding all panels");
        
        if (menuPanel != null) menuPanel.gameObject.SetActive(false);
        if (orderPanel != null) orderPanel.gameObject.SetActive(false);
        if (cookingPanel != null) cookingPanel.gameObject.SetActive(false);
        if (resultPanel != null) resultPanel.gameObject.SetActive(false);
    }
}
