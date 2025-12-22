using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject menuPanel;
    public GameObject orderPanel;
    public GameObject cookingPanel;
    public GameObject resultPanel;
    
    [Header("Panel Scripts")]
    public MenuPanel menuPanelScript;
    public OrderPanel orderPanelScript;
    public CookingPanel cookingPanelScript;
    public ResultPanel resultPanelScript;
    
    [Header("Manager References")]
    public CookingManager cookingManager;
    public FireBoostController fireBoostController;
    public JackpotController jackpotController;
    public ShieldController shieldController;
    
    void Awake()
    {
        Debug.Log("[UIManager] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[UIManager] Start() called");
        ValidateReferences();
        SetupPanels();
        HideAllPanels();
    }
    
    void ValidateReferences()
    {
        Debug.Log("[UIManager] Validating references...");
        
        if (menuPanel == null)
            Debug.LogError("[UIManager] ❌ Menu Panel is NULL!");
        else
            Debug.Log("[UIManager] ✅ Menu Panel found");
        
        if (orderPanel == null)
            Debug.LogError("[UIManager] ❌ Order Panel is NULL!");
        else
            Debug.Log("[UIManager] ✅ Order Panel found");
        
        if (cookingPanel == null)
            Debug.LogError("[UIManager] ❌ Cooking Panel is NULL!");
        else
            Debug.Log("[UIManager] ✅ Cooking Panel found");
        
        if (resultPanel == null)
            Debug.LogError("[UIManager] ❌ Result Panel is NULL!");
        else
            Debug.Log("[UIManager] ✅ Result Panel found");
    }
    
    void SetupPanels()
    {
        if (cookingPanelScript != null)
        {
            cookingPanelScript.Setup(cookingManager, fireBoostController);
        }
    }
    
    void HideAllPanels()
    {
        Debug.Log("[UIManager] Hiding all panels...");
        
        if (menuPanel != null) menuPanel.SetActive(false);
        if (orderPanel != null) orderPanel.SetActive(false);
        if (cookingPanel != null) cookingPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
    }
    
    public void ShowMenuPanel()
    {
        Debug.Log("[UIManager] ShowMenuPanel()");
        
        HideAllPanels();
        
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            
            if (menuPanelScript != null)
            {
                menuPanelScript.Show();
            }
            else
            {
                AnimatePanel(menuPanel);
            }
        }
    }
    
    public void ShowOrderPanel(RecipeData recipe)
    {
        Debug.Log($"[UIManager] ShowOrderPanel() - {recipe?.recipeName ?? "NULL"}");
        
        if (orderPanel != null)
        {
            orderPanel.SetActive(true);
            
            if (orderPanelScript != null)
            {
                orderPanelScript.DisplayOrder(recipe);
                orderPanelScript.Show();
            }
            else
            {
                AnimatePanel(orderPanel);
            }
        }
    }
    
    public void HideOrderPanel()
    {
        Debug.Log("[UIManager] HideOrderPanel()");
        
        if (orderPanelScript != null)
        {
            orderPanelScript.Hide();
        }
        else if (orderPanel != null)
        {
            orderPanel.SetActive(false);
        }
    }
    
    public void ShowCookingPanel()
    {
        Debug.Log("[UIManager] ShowCookingPanel()");
        
        HideAllPanels();
        
        if (cookingPanel != null)
        {
            cookingPanel.SetActive(true);
            
            if (cookingPanelScript != null)
            {
                cookingPanelScript.Show();
                
                if (GameManager.Instance.orderManager.currentOrder != null)
                {
                    cookingPanelScript.UpdateUI(
                        GameManager.Instance.orderManager.currentOrder,
                        GameManager.Instance.orderManager.currentOrder.totalTurns
                    );
                }
            }
            else
            {
                AnimatePanel(cookingPanel);
            }
        }
    }
    
    public void ShowResultPanel(bool victory)
    {
        Debug.Log($"[UIManager] ShowResultPanel() - Victory: {victory}");
        
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            
            if (resultPanelScript != null)
            {
                resultPanelScript.DisplayResult(victory);
                resultPanelScript.Show();
            }
            else
            {
                AnimatePanel(resultPanel);
            }
        }
    }
    
    public void ShowResultPanel(RewardResult result)
    {
        Debug.Log($"[UIManager] ShowResultPanel() - Result: {result?.grade ?? "NULL"}");
        
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
            
            if (resultPanelScript != null)
            {
                resultPanelScript.DisplayResult(result);
                resultPanelScript.Show();
            }
            else
            {
                AnimatePanel(resultPanel);
            }
        }
    }
    
    void AnimatePanel(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        panel.transform.localScale = Vector3.one * 0.9f;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1f, 0.3f));
        sequence.Join(panel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
    }
}
