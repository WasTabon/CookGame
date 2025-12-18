using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameState { Menu, Cooking, Result }
    public GameState currentState = GameState.Menu;
    
    [Header("References")]
    public OrderManager orderManager;
    public UIManager uiManager;
    public CookingManager cookingManager;
    
    void Awake()
    {
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
        
        if (orderManager == null)
            Debug.LogError("[GameManager] ❌ OrderManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ OrderManager reference found");
            
        if (uiManager == null)
            Debug.LogError("[GameManager] ❌ UIManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ UIManager reference found");
            
        if (cookingManager == null)
            Debug.LogError("[GameManager] ❌ CookingManager reference is NULL!");
        else
            Debug.Log("[GameManager] ✅ CookingManager reference found");
    }
    
    void Start()
    {
        Debug.Log("[GameManager] Start() called");
        
        if (uiManager != null)
        {
            Debug.Log("[GameManager] Calling uiManager.ShowMenuPanel()");
            uiManager.ShowMenuPanel();
        }
        else
        {
            Debug.LogError("[GameManager] ❌ Cannot show menu panel - UIManager is null!");
        }
    }
    
    public void ChangeState(GameState newState)
    {
        Debug.Log($"[GameManager] State changed: {currentState} → {newState}");
        currentState = newState;
    }
}
