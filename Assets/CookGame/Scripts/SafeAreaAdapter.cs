using UnityEngine;

public class SafeAreaAdapter : MonoBehaviour
{
    RectTransform rectTransform;
    
    void Awake()
    {
        Debug.Log("[SafeAreaAdapter] Awake() called");
        
        rectTransform = GetComponent<RectTransform>();
        
        if (rectTransform == null)
        {
            Debug.LogError("[SafeAreaAdapter] ❌ RectTransform component not found!");
            return;
        }
        
        Debug.Log("[SafeAreaAdapter] ✅ RectTransform found");
        ApplySafeArea();
    }
    
    void ApplySafeArea()
    {
        Debug.Log("[SafeAreaAdapter] ApplySafeArea() called");
        
        Rect safeArea = Screen.safeArea;
        Debug.Log($"[SafeAreaAdapter] Screen size: {Screen.width}x{Screen.height}");
        Debug.Log($"[SafeAreaAdapter] Safe area: {safeArea}");
        
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        
        Debug.Log($"[SafeAreaAdapter] ✅ Safe area applied - anchorMin: {anchorMin}, anchorMax: {anchorMax}");
    }
}
