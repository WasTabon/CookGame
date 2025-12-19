using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdapter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;
    
    void Awake()
    {
        Debug.Log("[SafeAreaAdapter] Awake() called");
        rectTransform = GetComponent<RectTransform>();
    }
    
    void Start()
    {
        Debug.Log("[SafeAreaAdapter] Start() called");
        ApplySafeArea();
    }
    
    void Update()
    {
        if (Screen.safeArea != lastSafeArea)
        {
            ApplySafeArea();
        }
    }
    
    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;
        
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        
        Debug.Log($"[SafeAreaAdapter] Applied safe area: {safeArea}");
        Debug.Log($"[SafeAreaAdapter] Anchors: min={anchorMin}, max={anchorMax}");
    }
}
