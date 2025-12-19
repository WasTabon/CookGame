using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MeterController : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage;
    public Image targetZoneImage;
    public TMP_Text valueText;
    public TMP_Text labelText;
    
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
    
    void Awake()
    {
        Debug.Log($"[MeterController] Awake() - {gameObject.name}");
    }
    
    void Start()
    {
        ValidateReferences();
    }
    
    void ValidateReferences()
    {
        if (fillImage == null)
            Debug.LogError($"[MeterController] ❌ {gameObject.name} - Fill Image is NULL!");
        if (valueText == null)
            Debug.LogWarning($"[MeterController] ⚠️ {gameObject.name} - Value Text is NULL");
    }
    
    public void Initialize(float startValue, float targetMin, float targetMax)
    {
        Debug.Log($"[MeterController] {gameObject.name} Initialize - Start: {startValue}, Target: {targetMin}-{targetMax}");
        
        CurrentValue = startValue;
        TargetMin = targetMin;
        TargetMax = targetMax;
        
        UpdateFillImmediate();
        UpdateTargetZone();
        UpdateColor();
        
        Debug.Log($"[MeterController] {gameObject.name} ✅ Initialized");
    }
    
    public void AddValue(float amount)
    {
        Debug.Log($"[MeterController] {gameObject.name} AddValue: +{amount:F2} (Current: {CurrentValue:F2})");
        
        float newValue = Mathf.Clamp(CurrentValue + amount, 0f, maxValue);
        SetValue(newValue);
    }
    
    public void SetValue(float newValue)
    {
        Debug.Log($"[MeterController] {gameObject.name} SetValue: {CurrentValue:F2} → {newValue:F2}");
        
        valueTween?.Kill();
        
        float startValue = CurrentValue;
        CurrentValue = Mathf.Clamp(newValue, 0f, maxValue);
        
        valueTween = DOTween.To(
            () => startValue,
            x => UpdateFillAnimated(x),
            CurrentValue,
            animationDuration
        ).SetEase(Ease.OutQuad);
        
        UpdateColor();
        
        if (valueText != null)
        {
            valueText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        }
    }
    
    void UpdateFillImmediate()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = CurrentValue / maxValue;
        }
        
        if (valueText != null)
        {
            valueText.text = CurrentValue.ToString("F0");
        }
    }
    
    void UpdateFillAnimated(float value)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = value / maxValue;
        }
        
        if (valueText != null)
        {
            valueText.text = value.ToString("F0");
        }
    }
    
    void UpdateTargetZone()
    {
        if (targetZoneImage != null)
        {
            float minNormalized = TargetMin / maxValue;
            float maxNormalized = TargetMax / maxValue;
            
            RectTransform rect = targetZoneImage.rectTransform;
            RectTransform parentRect = fillImage.rectTransform;
            
            float parentWidth = parentRect.rect.width;
            float zoneWidth = (maxNormalized - minNormalized) * parentWidth;
            float zoneX = minNormalized * parentWidth;
            
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 0.5f);
            rect.anchoredPosition = new Vector2(zoneX, 0);
            rect.sizeDelta = new Vector2(zoneWidth, 0);
            
            targetZoneImage.color = targetZoneColor;
            
            Debug.Log($"[MeterController] {gameObject.name} Target zone: {minNormalized:F2} - {maxNormalized:F2}");
        }
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
    
    public bool IsInTargetRange()
    {
        bool inRange = CurrentValue >= TargetMin && CurrentValue <= TargetMax;
        Debug.Log($"[MeterController] {gameObject.name} IsInTargetRange: {inRange} ({CurrentValue:F1} in {TargetMin}-{TargetMax})");
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
