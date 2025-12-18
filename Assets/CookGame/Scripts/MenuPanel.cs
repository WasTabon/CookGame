using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("[MenuPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[MenuPanel] Start() called");
    }
    
    public void OnGetOrderButtonPressed()
    {
        Debug.Log("[MenuPanel] ========================================");
        Debug.Log("[MenuPanel] GET ORDER button pressed!");
        Debug.Log("[MenuPanel] ========================================");
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("[MenuPanel] ❌ GameManager.Instance is NULL!");
            return;
        }
        
        Debug.Log("[MenuPanel] ✅ GameManager.Instance exists");
        
        if (GameManager.Instance.orderManager == null)
        {
            Debug.LogError("[MenuPanel] ❌ GameManager.Instance.orderManager is NULL!");
            return;
        }
        
        Debug.Log("[MenuPanel] ✅ OrderManager exists");
        Debug.Log("[MenuPanel] Calling GenerateRandomOrder()...");
        
        RecipeData order = GameManager.Instance.orderManager.GenerateRandomOrder();
        
        if (order == null)
        {
            Debug.LogError("[MenuPanel] ❌ GenerateRandomOrder() returned NULL!");
            return;
        }
        
        Debug.Log($"[MenuPanel] ✅ Order received: {order.recipeName}");
        
        if (GameManager.Instance.uiManager == null)
        {
            Debug.LogError("[MenuPanel] ❌ GameManager.Instance.uiManager is NULL!");
            return;
        }
        
        Debug.Log("[MenuPanel] ✅ UIManager exists");
        Debug.Log("[MenuPanel] Calling ShowOrderPanel()...");
        
        GameManager.Instance.uiManager.ShowOrderPanel(order);
        
        Debug.Log("[MenuPanel] ✅ ShowOrderPanel() called successfully");
        Debug.Log("[MenuPanel] ========================================");
    }
}
