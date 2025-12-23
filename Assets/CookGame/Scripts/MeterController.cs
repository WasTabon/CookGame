using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MeterController : MonoBehaviour
{
    [Header("UI References")]
    public Slider meterSlider;
    public Image fillImage;
    public Image targetZoneImage;
    public TMP_Text valueText;
    
    [Header("Glow Effect")]
    public MeterGlowController glowController;
    
    [Header("Colors")]
    public Color normalColor = new Color(0.2f, 0.6f, 1f);
    public Color inTargetColor = new Color(0.2f, 0.8f, 0.2f);
    public Color overflowColor = new Color(1f, 0.2f, 0.2f);
    public Color targetZoneColor = new Color(0.2f, 0.8f, 0.2f, 0.3f);
    
    [Header("Settings")]
    public float maxValue = 100f;
    public float animationDuration = 0.5f;
    
    public float CurrentValue { get; private set; }
    public float TargetMin { get; private set; }
    public float TargetMax { get; private set; }
    
    private Tween valueTween;
    private bool wasInTarget = false;
    
    void Awake()
    {
        Debug.Log($"[MeterController] Awake() - {gameObject.name}");
    }
    
    void Start()
    {
        ValidateReferences();
        SetupSlider();
    }
    
    void ValidateReferences()
    {
        if (meterSlider == null)
            Debug.LogError($"[MeterController] ❌ {gameObject.name} - Meter Slider is NULL!");
        else
            Debug.Log($"[MeterController] ✅ {gameObject.name} - Meter Slider found");
        
        if (valueText == null)
            Debug.LogWarning($"[MeterController] ⚠️ {gameObject.name} - Value Text is NULL");
        
        if (glowController == null)
            Debug.Log($"[MeterController] ℹ️ {gameObject.name} - No GlowController (optional)");
        else
            Debug.Log($"[MeterController] ✅ {gameObject.name} - GlowController found");
    }
    
    void SetupSlider()
    {
        if (meterSlider != null)
        {
            meterSlider.minValue = 0f;
            meterSlider.maxValue = maxValue;
            meterSlider.interactable = false;
            Debug.Log($"[MeterController] ✅ {gameObject.name} - Slider configured (0 to {maxValue})");
        }
    }
    
    public void Initialize(float startValue, float targetMin, float targetMax)
    {
        Debug.Log($"[MeterController] {gameObject.name} Initialize - Start: {startValue}, Target: {targetMin}-{targetMax}");
        
        CurrentValue = startValue;
        TargetMin = targetMin;
        TargetMax = targetMax;
        wasInTarget = false;
        
        UpdateSliderImmediate();
        UpdateTargetZone();
        UpdateColor();
        UpdateGlow();
        
        if (glowController != null)
        {
            glowController.Reset();
        }
        
        Debug.Log($"[MeterController] {gameObject.name} ✅ Initialized");
    }
    
    public void AddValue(float amount)
    {
        Debug.Log($"[MeterController] {gameObject.name} AddValue: +{amount:F2} (Current: {CurrentValue:F2})");
        
        float newValue = Mathf.Clamp(CurrentValue + amount, 0f, maxValue);
        SetValue(newValue);
        
        if (AudioManager.Instance != null && amount > 0.5f)
        {
            AudioManager.Instance.PlayMeterIncrease();
        }
    }
    
    public void SetValue(float newValue)
    {
        Debug.Log($"[MeterController] {gameObject.name} SetValue: {CurrentValue:F2} → {newValue:F2}");
    
        valueTween?.Kill();
    
        float startValue = CurrentValue;
        CurrentValue = Mathf.Clamp(newValue, 0f, maxValue);
    
        if (meterSlider != null)
        {
            valueTween = meterSlider.DOValue(CurrentValue, animationDuration).SetEase(Ease.OutQuad);
        }
    
        if (valueText != null)
        {
            DOTween.To(
                () => startValue,
                x => valueText.text = x.ToString("F0"),
                CurrentValue,
                animationDuration
            );
        }
    
        UpdateColor();
        UpdateGlow();
        CheckTargetTransition();
    }
    
    void CheckTargetTransition()
    {
        bool isNowInTarget = IsInTargetRange();
        
        if (isNowInTarget && !wasInTarget)
        {
            Debug.Log($"[MeterController] {gameObject.name} 🎯 Entered target zone!");
            
            if (VFXController.Instance != null)
            {
                VFXController.Instance.PlayMeterInTarget();
            }
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMeterInTarget();
            }
            
            if (valueText != null)
            {
                valueText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5);
            }
        }
        
        wasInTarget = isNowInTarget;
    }
    
    void UpdateSliderImmediate()
    {
        if (meterSlider != null)
        {
            meterSlider.value = CurrentValue;
            Debug.Log($"[MeterController] {gameObject.name} Slider value set to {CurrentValue}");
        }
        
        if (valueText != null)
        {
            valueText.text = CurrentValue.ToString("F0");
        }
    }
    
    void UpdateTargetZone()
    {
        if (targetZoneImage == null || meterSlider == null) return;
        
        RectTransform sliderRect = meterSlider.GetComponent<RectTransform>();
        RectTransform fillArea = meterSlider.fillRect?.parent?.GetComponent<RectTransform>();
        
        if (fillArea == null)
        {
            Debug.LogWarning($"[MeterController] {gameObject.name} - Cannot find fill area for target zone");
            return;
        }
        
        float sliderWidth = fillArea.rect.width;
        float minNormalized = TargetMin / maxValue;
        float maxNormalized = TargetMax / maxValue;
        
        float zoneWidth = (maxNormalized - minNormalized) * sliderWidth;
        float zoneX = minNormalized * sliderWidth;
        
        RectTransform targetRect = targetZoneImage.rectTransform;
        targetRect.anchorMin = new Vector2(0, 0);
        targetRect.anchorMax = new Vector2(0, 1);
        targetRect.pivot = new Vector2(0, 0.5f);
        targetRect.anchoredPosition = new Vector2(zoneX, 0);
        targetRect.sizeDelta = new Vector2(zoneWidth, 0);
        
        targetZoneImage.color = targetZoneColor;
        
        Debug.Log($"[MeterController] {gameObject.name} Target zone: {TargetMin}-{TargetMax} ({minNormalized:F2}-{maxNormalized:F2})");
    }
    
    void UpdateColor()
    {
        if (fillImage == null) return;
        
        Color targetColor;
        
        if (CurrentValue > TargetMax)
        {
            targetColor = overflowColor;
            Debug.Log($"[MeterController] {gameObject.name} Color: OVERFLOW (red)");
        }
        else if (CurrentValue >= TargetMin && CurrentValue <= TargetMax)
        {
            targetColor = inTargetColor;
            Debug.Log($"[MeterController] {gameObject.name} Color: IN TARGET (green)");
        }
        else
        {
            targetColor = normalColor;
        }
        
        fillImage.DOColor(targetColor, 0.3f);
    }
    
    void UpdateGlow()
    {
        if (glowController != null)
        {
            glowController.UpdateGlow(CurrentValue, TargetMin, TargetMax, maxValue);
        }
    }
    
    public bool IsInTargetRange()
    {
        bool inRange = CurrentValue >= TargetMin && CurrentValue <= TargetMax;
        return inRange;
    }
    
    public bool IsOverflow()
    {
        bool overflow = CurrentValue > TargetMax;
        if (overflow)
        {
            Debug.Log($"[MeterController] {gameObject.name} OVERFLOW: {CurrentValue:F1} > {TargetMax}");
        }
        return overflow;
    }
    
    void OnDestroy()
    {
        valueTween?.Kill();
    }
}
