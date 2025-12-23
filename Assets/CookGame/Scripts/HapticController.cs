using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public static class HapticController
{
    private static bool hapticsEnabled = true;
    private static bool isInitialized = false;
    
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _PlayHapticLight();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticMedium();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticHeavy();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticSelection();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticSuccess();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticWarning();
    
    [DllImport("__Internal")]
    private static extern void _PlayHapticError();
    
    [DllImport("__Internal")]
    private static extern bool _SupportsHaptics();
#endif

    public static void Initialize()
    {
        if (isInitialized) return;
        
        hapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
        isInitialized = true;
        
        Debug.Log($"[HapticController] ✅ Initialized - Enabled: {hapticsEnabled}");
    }
    
    public static bool SupportsHaptics()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return _SupportsHaptics();
#else
        return false;
#endif
    }
    
    public static void SetEnabled(bool enabled)
    {
        hapticsEnabled = enabled;
        PlayerPrefs.SetInt("HapticsEnabled", enabled ? 1 : 0);
        Debug.Log($"[HapticController] Haptics Enabled: {enabled}");
    }
    
    public static bool IsEnabled()
    {
        return hapticsEnabled;
    }
    
    public static void LightImpact()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticLight();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Light Impact");
    }
    
    public static void MediumImpact()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticMedium();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Medium Impact");
    }
    
    public static void HeavyImpact()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticHeavy();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Heavy Impact");
    }
    
    public static void SelectionChanged()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticSelection();
#endif
        Debug.Log("[HapticController] 📳 Selection Changed");
    }
    
    public static void Success()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticSuccess();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Success");
    }
    
    public static void Warning()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticWarning();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Warning");
    }
    
    public static void Error()
    {
        if (!hapticsEnabled) return;
        
#if UNITY_IOS && !UNITY_EDITOR
        _PlayHapticError();
#elif UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        Debug.Log("[HapticController] 📳 Error");
    }
}
