using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VFXController : MonoBehaviour
{
    public static VFXController Instance { get; private set; }
    
    [Header("Camera Reference")]
    public Camera mainCamera;
    
    [Header("Screen Flash")]
    public Image screenFlashImage;
    
    [Header("Shake Settings")]
    public float defaultShakeDuration = 0.3f;
    public float defaultShakeStrength = 10f;
    
    [Header("Flash Colors")]
    public Color victoryFlashColor = new Color(1f, 0.84f, 0f, 0.5f);
    public Color defeatFlashColor = new Color(1f, 0f, 0f, 0.5f);
    public Color jackpotFlashColor = new Color(1f, 0.84f, 0f, 0.6f);
    public Color overflowFlashColor = new Color(1f, 0f, 0f, 0.4f);
    public Color successFlashColor = new Color(0f, 1f, 0.5f, 0.3f);
    
    private Vector3 originalCameraPosition;
    private Tween currentShakeTween;
    
    void Awake()
    {
        Debug.Log("[VFXController] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[VFXController] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[VFXController] ⚠️ Duplicate instance destroyed");
            Destroy(gameObject);
            return;
        }
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.localPosition;
        }
        
        if (screenFlashImage != null)
        {
            Color c = screenFlashImage.color;
            c.a = 0f;
            screenFlashImage.color = c;
        }
    }
    
    public void ShakeCamera(float duration = -1f, float strength = -1f)
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("[VFXController] ⚠️ No camera for shake");
            return;
        }
        
        if (duration < 0) duration = defaultShakeDuration;
        if (strength < 0) strength = defaultShakeStrength;
        
        Debug.Log($"[VFXController] 📳 Camera shake: duration={duration}, strength={strength}");
        
        currentShakeTween?.Kill();
        mainCamera.transform.localPosition = originalCameraPosition;
        
        currentShakeTween = mainCamera.transform
            .DOShakePosition(duration, strength, 20, 90f, false, true)
            .OnComplete(() => mainCamera.transform.localPosition = originalCameraPosition);
    }
    
    public void ShakeLight()
    {
        ShakeCamera(0.15f, 5f);
    }
    
    public void ShakeMedium()
    {
        ShakeCamera(0.25f, 12f);
    }
    
    public void ShakeHeavy()
    {
        ShakeCamera(0.4f, 20f);
    }
    
    public void FlashScreen(Color color, float duration = 0.3f)
    {
        if (screenFlashImage == null)
        {
            Debug.LogWarning("[VFXController] ⚠️ No flash image assigned");
            return;
        }
        
        Debug.Log($"[VFXController] ✨ Screen flash: {color}");
        
        screenFlashImage.DOKill();
        
        Color startColor = color;
        Color endColor = color;
        endColor.a = 0f;
        
        screenFlashImage.color = startColor;
        screenFlashImage.DOFade(0f, duration).SetEase(Ease.OutQuad);
    }
    
    public void FlashVictory()
    {
        FlashScreen(victoryFlashColor, 0.5f);
        ShakeMedium();
    }
    
    public void FlashDefeat()
    {
        FlashScreen(defeatFlashColor, 0.4f);
        ShakeHeavy();
    }
    
    public void FlashJackpot()
    {
        Debug.Log("[VFXController] 🎰 Jackpot flash sequence");
        
        if (screenFlashImage == null) return;
        
        Sequence flashSequence = DOTween.Sequence();
        
        for (int i = 0; i < 3; i++)
        {
            flashSequence.AppendCallback(() => {
                screenFlashImage.color = jackpotFlashColor;
            });
            flashSequence.Append(screenFlashImage.DOFade(0f, 0.15f));
            flashSequence.AppendInterval(0.05f);
        }
        
        ShakeMedium();
    }
    
    public void FlashOverflow()
    {
        FlashScreen(overflowFlashColor, 0.3f);
        ShakeHeavy();
    }
    
    public void FlashSuccess()
    {
        FlashScreen(successFlashColor, 0.25f);
    }
    
    public void PlayIngredientSelect()
    {
        ShakeLight();
    }
    
    public void PlayMeterInTarget()
    {
        FlashSuccess();
    }
    
    public void PulseElement(Transform element, float scale = 1.2f, float duration = 0.3f)
    {
        if (element == null) return;
        
        element.DOKill();
        element.DOPunchScale(Vector3.one * (scale - 1f), duration, 5);
    }
    
    public void BounceElement(Transform element, float height = 20f, float duration = 0.3f)
    {
        if (element == null) return;
        
        RectTransform rect = element as RectTransform;
        if (rect == null) return;
        
        Vector2 originalPos = rect.anchoredPosition;
        
        rect.DOKill();
        
        Sequence bounce = DOTween.Sequence();
        bounce.Append(rect.DOAnchorPosY(originalPos.y + height, duration * 0.4f).SetEase(Ease.OutQuad));
        bounce.Append(rect.DOAnchorPosY(originalPos.y, duration * 0.6f).SetEase(Ease.OutBounce));
    }
    
    public void AnimateCounter(TMPro.TMP_Text textElement, int fromValue, int toValue, float duration = 1f, string format = "{0}")
    {
        if (textElement == null) return;
        
        Debug.Log($"[VFXController] 🔢 Counter: {fromValue} → {toValue}");
        
        int current = fromValue;
        DOTween.To(
            () => current,
            x => {
                current = x;
                textElement.text = string.Format(format, x);
            },
            toValue,
            duration
        ).SetEase(Ease.OutQuad);
    }
    
    void OnDestroy()
    {
        currentShakeTween?.Kill();
        
        if (mainCamera != null)
        {
            mainCamera.transform.localPosition = originalCameraPosition;
        }
    }
}
