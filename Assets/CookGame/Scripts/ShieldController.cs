using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public enum MeterType
{
    Taste,
    Stability,
    Magic
}

public class ShieldController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shieldSelectionPanel;
    public Button tasteShieldButton;
    public Button stabilityShieldButton;
    public Button magicShieldButton;
    
    [Header("Shield Indicators")]
    public GameObject tasteShieldIcon;
    public GameObject stabilityShieldIcon;
    public GameObject magicShieldIcon;
    
    [Header("Visual")]
    public Color shieldActiveColor = new Color(0.3f, 0.8f, 1f);
    
    public bool HasActiveShield { get; private set; }
    public MeterType? ShieldedMeter { get; private set; }
    public bool IsSelectingShield { get; private set; }
    
    public event Action<MeterType> OnShieldActivated;
    public event Action<MeterType> OnShieldUsed;
    public event Action OnShieldSelectionRequired;
    
    void Awake()
    {
        Debug.Log("[ShieldController] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[ShieldController] Start() called");
        SetupButtons();
        HideSelectionPanel();
        HideAllShieldIcons();
    }
    
    void SetupButtons()
    {
        if (tasteShieldButton != null)
        {
            tasteShieldButton.onClick.AddListener(() => SelectShieldMeter(MeterType.Taste));
            Debug.Log("[ShieldController] ✅ Taste shield button configured");
        }
        
        if (stabilityShieldButton != null)
        {
            stabilityShieldButton.onClick.AddListener(() => SelectShieldMeter(MeterType.Stability));
            Debug.Log("[ShieldController] ✅ Stability shield button configured");
        }
        
        if (magicShieldButton != null)
        {
            magicShieldButton.onClick.AddListener(() => SelectShieldMeter(MeterType.Magic));
            Debug.Log("[ShieldController] ✅ Magic shield button configured");
        }
    }
    
    public void ResetForNewCooking()
    {
        Debug.Log("[ShieldController] Reset for new cooking session");
        
        HasActiveShield = false;
        ShieldedMeter = null;
        IsSelectingShield = false;
        
        HideSelectionPanel();
        HideAllShieldIcons();
    }
    
    public void ActivateShieldSelection()
    {
        Debug.Log("[ShieldController] 🛡️ Shield selection activated");
        
        IsSelectingShield = true;
        ShowSelectionPanel();
        OnShieldSelectionRequired?.Invoke();
    }
    
    void ShowSelectionPanel()
    {
        Debug.Log("[ShieldController] Showing shield selection panel");
        
        if (shieldSelectionPanel != null)
        {
            shieldSelectionPanel.SetActive(true);
            shieldSelectionPanel.transform.localScale = Vector3.zero;
            shieldSelectionPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
    }
    
    void HideSelectionPanel()
    {
        if (shieldSelectionPanel != null)
        {
            shieldSelectionPanel.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
                .OnComplete(() => shieldSelectionPanel.SetActive(false));
        }
    }
    
    void SelectShieldMeter(MeterType meter)
    {
        Debug.Log($"[ShieldController] 🛡️ Shield placed on: {meter}");
        
        HasActiveShield = true;
        ShieldedMeter = meter;
        IsSelectingShield = false;
        
        HideSelectionPanel();
        ShowShieldIcon(meter);
        
        OnShieldActivated?.Invoke(meter);
    }
    
    void ShowShieldIcon(MeterType meter)
    {
        HideAllShieldIcons();
        
        GameObject icon = meter switch
        {
            MeterType.Taste => tasteShieldIcon,
            MeterType.Stability => stabilityShieldIcon,
            MeterType.Magic => magicShieldIcon,
            _ => null
        };
        
        if (icon != null)
        {
            icon.SetActive(true);
            icon.transform.localScale = Vector3.zero;
            icon.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            
            icon.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 2)
                .SetLoops(-1, LoopType.Restart)
                .SetDelay(0.5f);
        }
    }
    
    void HideAllShieldIcons()
    {
        if (tasteShieldIcon != null) 
        {
            DOTween.Kill(tasteShieldIcon.transform);
            tasteShieldIcon.SetActive(false);
        }
        if (stabilityShieldIcon != null) 
        {
            DOTween.Kill(stabilityShieldIcon.transform);
            stabilityShieldIcon.SetActive(false);
        }
        if (magicShieldIcon != null) 
        {
            DOTween.Kill(magicShieldIcon.transform);
            magicShieldIcon.SetActive(false);
        }
    }
    
    public bool TryBlockOverflow(MeterType overflowedMeter)
    {
        if (!HasActiveShield || ShieldedMeter == null)
        {
            Debug.Log("[ShieldController] No active shield");
            return false;
        }
        
        if (ShieldedMeter.Value != overflowedMeter)
        {
            Debug.Log($"[ShieldController] Shield on {ShieldedMeter.Value}, but {overflowedMeter} overflowed");
            return false;
        }
        
        Debug.Log($"[ShieldController] 🛡️ SHIELD ACTIVATED! Blocking {overflowedMeter} overflow!");
        
        UseShield();
        OnShieldUsed?.Invoke(overflowedMeter);
        
        return true;
    }
    
    void UseShield()
    {
        MeterType usedMeter = ShieldedMeter.Value;
        
        HasActiveShield = false;
        ShieldedMeter = null;
        
        GameObject icon = usedMeter switch
        {
            MeterType.Taste => tasteShieldIcon,
            MeterType.Stability => stabilityShieldIcon,
            MeterType.Magic => magicShieldIcon,
            _ => null
        };
        
        if (icon != null)
        {
            DOTween.Kill(icon.transform);
            
            Sequence breakSequence = DOTween.Sequence();
            breakSequence.Append(icon.transform.DOScale(1.5f, 0.2f));
            breakSequence.Append(icon.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack));
            breakSequence.OnComplete(() => icon.SetActive(false));
        }
        
        Debug.Log("[ShieldController] 🛡️ Shield consumed!");
    }
}
