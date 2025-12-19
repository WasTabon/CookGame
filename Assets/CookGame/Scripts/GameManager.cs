using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameState { Menu, Cooking, Result }
    public GameState currentState = GameState.Menu;
    
    [Header("References")]
    public OrderManager orderManager;
    public CookingManager cookingManager;
    public UIManager uiManager;
    public FireBoostController fireBoostController;
    
    void Awake()
    {
        Debug.Log("[GameManager] ========================================");
        Debug.Log("[GameManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[GameManager] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[GameManager] ⚠️ Duplicate GameManager detected! Destroying...");
            Destroy(gameObject);
            return;
        }
        
        ValidateReferences();
    }
    
    void ValidateReferences()
    {
        Debug.Log("[GameManager] Validating references...");
        
        if (orderManager == null)
            Debug.LogError("[GameManager] ❌ OrderManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ OrderManager reference found");
        
        if (cookingManager == null)
            Debug.LogError("[GameManager] ❌ CookingManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ CookingManager reference found");
            
        if (uiManager == null)
            Debug.LogError("[GameManager] ❌ UIManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ UIManager reference found");
        
        if (fireBoostController == null)
            Debug.LogWarning("[GameManager] ⚠️ FireBoostController reference is NULL - Fire Boost disabled");
        else
            Debug.Log("[GameManager] ✅ FireBoostController reference found");
        
        Debug.Log("[GameManager] ========================================");
    }
    
    void Start()
    {
        Debug.Log("[GameManager] Start() called");
        Debug.Log("[GameManager] Showing Menu Panel...");
        
        uiManager.ShowMenuPanel();
    }
    
    public void ChangeState(GameState newState)
    {
        Debug.Log($"[GameManager] State change: {currentState} → {newState}");
        currentState = newState;
    }
    
    public void StartCooking()
    {
        Debug.Log("[GameManager] StartCooking() called");
        
        if (orderManager.currentOrder == null)
        {
            Debug.LogError("[GameManager] ❌ No current order!");
            return;
        }
        
        ChangeState(GameState.Cooking);
        uiManager.ShowCookingPanel();
        cookingManager.StartCooking(orderManager.currentOrder);
    }
    
    public void ReturnToMenu()
    {
        Debug.Log("[GameManager] ReturnToMenu() called");
        ChangeState(GameState.Menu);
        uiManager.ShowMenuPanel();
    }
}
