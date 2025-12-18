using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MeterController : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public TextMeshProUGUI meterNameText;
    public TextMeshProUGUI currentValueText;
    public TextMeshProUGUI targetRangeText;
    public Image fillImage;
    
    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color targetColor = Color.yellow;
    public Color overflowColor = Color.red;
    
    private float currentValue = 0f;
    private float targetMin;
    private float targetMax;
    private string meterName;
    
    void Awake()
    {
        Debug.Log($"[MeterController] Awake() on {gameObject.name}");
    }
    
    public void Initialize(string name, float min, float max)
    {
        Debug.Log($"[MeterController] Initialize: {name}, range: {min}-{max}");
        
        meterName = name;
        targetMin = min;
        targetMax = max;
        currentValue = 0f;
        
        if (meterNameText != null)
            meterNameText.text = name;
        
        if (targetRangeText != null)
            targetRangeText.text = $"Target: {min:F0} - {max:F0}";
        
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.value = 0f;
        }
        
        UpdateDisplay();
    }
    
    public void AddValue(float amount)
    {
        Debug.Log($"[MeterController] {meterName}: Adding {amount:F1} (current: {currentValue:F1})");
        
        float oldValue = currentValue;
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0f, 100f);
        
        Debug.Log($"[MeterController] {meterName}: New value: {currentValue:F1}");
        
        if (slider != null)
        {
            slider.DOValue(currentValue, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
                Debug.Log($"[MeterController] {meterName}: Animation complete");
            });
        }
        
        UpdateDisplay();
        CheckMeterState();
    }
    
    public bool IsInTarget()
    {
        bool inRange = currentValue >= targetMin && currentValue <= targetMax;
        Debug.Log($"[MeterController] {meterName}: IsInTarget? {inRange} (value: {currentValue:F1}, range: {targetMin}-{targetMax})");
        return inRange;
    }
    
    public bool IsOverTarget()
    {
        bool over = currentValue > targetMax;
        Debug.Log($"[MeterController] {meterName}: IsOverTarget? {over} (value: {currentValue:F1}, max: {targetMax})");
        return over;
    }
    
    public float GetCurrentValue()
    {
        return currentValue;
    }
    
    void UpdateDisplay()
    {
        if (currentValueText != null)
            currentValueText.text = $"{currentValue:F1}";
    }
    
    void CheckMeterState()
    {
        if (fillImage == null) return;
        
        if (currentValue > targetMax)
        {
            fillImage.color = overflowColor;
            Debug.Log($"[MeterController] {meterName}: State = OVERFLOW (red)");
        }
        else if (currentValue >= targetMin && currentValue <= targetMax)
        {
            fillImage.color = targetColor;
            Debug.Log($"[MeterController] {meterName}: State = IN TARGET (yellow)");
        }
        else
        {
            fillImage.color = normalColor;
            Debug.Log($"[MeterController] {meterName}: State = NORMAL (green)");
        }
    }
}
