using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MeterGlowController : MonoBehaviour
{
    [Header("Glow Image")]
    public Image glowImage;
    
    [Header("Glow Colors")]
    public Color warningColor = new Color(1f, 0.8f, 0f, 0.5f);
    public Color dangerColor = new Color(1f, 0.2f, 0.2f, 0.6f);
    public Color perfectColor = new Color(0f, 1f, 0.5f, 0.4f);
    public Color overflowColor = new Color(1f, 0f, 0f, 0.8f);
    
    [Header("Animation Settings")]
    public float warningPulseSpeed = 1f;
    public float dangerPulseSpeed = 2f;
    public float maxGlowAlpha = 0.6f;
    
    private GlowState currentState = GlowState.None;
    private Tween glowTween;
    private bool isGlowing = false;
    
    void Awake()
    {
        Debug.Log($"[MeterGlowController] Awake() - {gameObject.name}");
        
        if (glowImage != null)
        {
            Color c = glowImage.color;
            c.a = 0f;
            glowImage.color = c;
        }
    }
    
    public void UpdateGlow(float currentValue, float targetMin, float targetMax, float maxValue)
    {
        GlowState newState = DetermineState(currentValue, targetMin, targetMax, maxValue);
        
        if (newState != currentState)
        {
            Debug.Log($"[MeterGlowController] {gameObject.name} State: {currentState} → {newState}");
            currentState = newState;
            ApplyGlowState(newState);
        }
    }
    
    GlowState DetermineState(float currentValue, float targetMin, float targetMax, float maxValue)
    {
        if (currentValue > targetMax)
        {
            return GlowState.Overflow;
        }
        
        if (currentValue >= targetMin && currentValue <= targetMax)
        {
            return GlowState.Perfect;
        }
        
        float distanceToMax = targetMax - currentValue;
        float rangeSize = targetMax - targetMin;
        
        if (distanceToMax < rangeSize * 0.3f && distanceToMax > 0)
        {
            return GlowState.Danger;
        }
        
        if (distanceToMax < rangeSize * 0.6f && distanceToMax > 0)
        {
            return GlowState.Warning;
        }
        
        return GlowState.None;
    }
    
    void ApplyGlowState(GlowState state)
    {
        StopGlow();
        
        switch (state)
        {
            case GlowState.None:
                FadeOutGlow();
                break;
            case GlowState.Warning:
                StartPulse(warningColor, warningPulseSpeed);
                break;
            case GlowState.Danger:
                StartPulse(dangerColor, dangerPulseSpeed);
                break;
            case GlowState.Perfect:
                StartStaticGlow(perfectColor);
                break;
            case GlowState.Overflow:
                StartRapidFlash(overflowColor);
                break;
        }
    }
    
    void StartPulse(Color color, float speed)
    {
        if (glowImage == null) return;
        
        isGlowing = true;
        
        Color startColor = color;
        startColor.a = 0f;
        
        Color endColor = color;
        endColor.a = maxGlowAlpha;
        
        glowImage.color = startColor;
        
        float duration = 1f / speed;
        glowTween = glowImage.DOColor(endColor, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    void StartStaticGlow(Color color)
    {
        if (glowImage == null) return;
        
        isGlowing = true;
        
        Color targetColor = color;
        targetColor.a = maxGlowAlpha * 0.7f;
        
        glowImage.DOColor(targetColor, 0.3f).SetEase(Ease.OutQuad);
    }
    
    void StartRapidFlash(Color color)
    {
        if (glowImage == null) return;
        
        isGlowing = true;
        
        Color startColor = color;
        startColor.a = 0.2f;
        
        Color endColor = color;
        endColor.a = maxGlowAlpha;
        
        glowImage.color = startColor;
        
        glowTween = glowImage.DOColor(endColor, 0.1f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    void FadeOutGlow()
    {
        if (glowImage == null) return;
        
        isGlowing = false;
        
        Color targetColor = glowImage.color;
        targetColor.a = 0f;
        
        glowImage.DOColor(targetColor, 0.3f).SetEase(Ease.OutQuad);
    }
    
    void StopGlow()
    {
        glowTween?.Kill();
        glowTween = null;
    }
    
    public void Reset()
    {
        StopGlow();
        currentState = GlowState.None;
        
        if (glowImage != null)
        {
            Color c = glowImage.color;
            c.a = 0f;
            glowImage.color = c;
        }
    }
    
    public GlowState GetCurrentState()
    {
        return currentState;
    }
    
    void OnDestroy()
    {
        StopGlow();
    }
}

public enum GlowState
{
    None,
    Warning,
    Danger,
    Perfect,
    Overflow
}
