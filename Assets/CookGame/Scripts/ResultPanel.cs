using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultPanel : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text resultTitleText;
    public TMP_Text gradeText;
    public TMP_Text resultMessageText;
    
    [Header("Meter Status")]
    public TMP_Text tasteStatusText;
    public TMP_Text stabilityStatusText;
    public TMP_Text magicStatusText;
    
    [Header("Reward Breakdown")]
    public TMP_Text baseRewardText;
    public TMP_Text meterBonusText;
    public TMP_Text turnsBonusText;
    public TMP_Text finalRewardText;
    
    [Header("Button References")]
    public Button continueButton;
    
    [Header("Visual")]
    public Image backgroundImage;
    public Color perfectColor = new Color(0.2f, 0.8f, 0.2f, 0.9f);
    public Color goodColor = new Color(0.8f, 0.8f, 0.2f, 0.9f);
    public Color okayColor = new Color(0.8f, 0.5f, 0.2f, 0.9f);
    public Color failColor = new Color(0.8f, 0.2f, 0.2f, 0.9f);
    
    [Header("Animation Settings")]
    public float elementStagger = 0.15f;
    public float counterDuration = 1.5f;
    
    private RewardResult currentResult;
    
    void Awake()
    {
        Debug.Log("[ResultPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[ResultPanel] Start() called");
        ValidateReferences();
        SetupButtons();
    }
    
    void ValidateReferences()
    {
        if (resultTitleText == null)
            Debug.LogWarning("[ResultPanel] ⚠️ Result Title Text is NULL");
        if (continueButton == null)
            Debug.LogError("[ResultPanel] ❌ Continue Button is NULL!");
    }
    
    void SetupButtons()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
            Debug.Log("[ResultPanel] ✅ Continue button listener added");
        }
    }
    
    public void DisplayResult(bool victory)
    {
        Debug.Log($"[ResultPanel] DisplayResult (legacy): Victory = {victory}");
        
        RewardResult result = new RewardResult
        {
            grade = victory ? "PERFECT" : "FAILED",
            finalReward = victory ? 100 : 0,
            metersInRange = victory ? 3 : 0
        };
        
        DisplayResult(result);
    }
    
    public void DisplayResult(RewardResult result)
    {
        currentResult = result;
        
        bool hasReward = result != null && result.finalReward > 0;
        string grade = result?.grade ?? "FAILED";
        
        Debug.Log($"[ResultPanel] DisplayResult: Grade = {grade}, Reward = {result?.finalReward ?? 0}");
        
        if (resultTitleText != null)
        {
            resultTitleText.text = hasReward ? "ORDER COMPLETE!" : "ORDER FAILED!";
            resultTitleText.color = hasReward ? Color.green : Color.red;
        }
        
        if (gradeText != null)
        {
            gradeText.text = grade;
            gradeText.color = GetGradeColor(grade);
        }
        
        if (resultMessageText != null)
        {
            resultMessageText.text = GetGradeMessage(grade);
        }
        
        if (result != null)
        {
            UpdateMeterStatus(result);
            UpdateRewardBreakdown(result);
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = GetBackgroundColor(grade);
        }
        
        Debug.Log("[ResultPanel] ✅ Result displayed");
    }
    
    void UpdateMeterStatus(RewardResult result)
    {
        if (tasteStatusText != null)
        {
            tasteStatusText.text = result.tasteInRange ? "✓ Taste" : "✗ Taste";
            tasteStatusText.color = result.tasteInRange ? Color.green : Color.red;
        }
        
        if (stabilityStatusText != null)
        {
            stabilityStatusText.text = result.stabilityInRange ? "✓ Stability" : "✗ Stability";
            stabilityStatusText.color = result.stabilityInRange ? Color.green : Color.red;
        }
        
        if (magicStatusText != null)
        {
            magicStatusText.text = result.magicInRange ? "✓ Magic" : "✗ Magic";
            magicStatusText.color = result.magicInRange ? Color.green : Color.red;
        }
    }
    
    void UpdateRewardBreakdown(RewardResult result)
    {
        if (baseRewardText != null)
        {
            baseRewardText.text = $"Base: {result.baseReward}";
        }
        
        if (meterBonusText != null)
        {
            int meterPercent = Mathf.RoundToInt(result.meterPercentage * 100);
            meterBonusText.text = $"Meters ({result.metersInRange}/3): {meterPercent}%";
        }
        
        if (turnsBonusText != null)
        {
            if (result.turnsBonus > 0)
            {
                turnsBonusText.text = $"Turns bonus ({result.turnsRemaining} left): +{result.turnsBonus}";
                turnsBonusText.gameObject.SetActive(true);
            }
            else
            {
                turnsBonusText.gameObject.SetActive(false);
            }
        }
        
        if (finalRewardText != null)
        {
            finalRewardText.text = "0 Coins";
        }
    }
    
    void AnimateRewardCounter(int targetValue)
    {
        if (finalRewardText == null || targetValue <= 0) return;
        
        Debug.Log($"[ResultPanel] 🔢 Animating reward counter to {targetValue}");
        
        if (VFXController.Instance != null)
        {
            VFXController.Instance.AnimateCounter(finalRewardText, 0, targetValue, counterDuration, "{0} Coins");
        }
        else
        {
            int currentValue = 0;
            DOTween.To(() => currentValue, x => {
                currentValue = x;
                finalRewardText.text = $"{currentValue} Coins";
            }, targetValue, counterDuration).SetEase(Ease.OutQuad);
        }
        
        DOVirtual.DelayedCall(counterDuration, () => {
            if (finalRewardText != null)
            {
                finalRewardText.transform.DOPunchScale(Vector3.one * 0.3f, 0.4f, 5);
                
                if (VFXController.Instance != null)
                {
                    VFXController.Instance.FlashSuccess();
                }
            }
        });
    }
    
    Color GetGradeColor(string grade)
    {
        return grade switch
        {
            "PERFECT" => new Color(1f, 0.84f, 0f),
            "GOOD" => Color.green,
            "OKAY" => Color.yellow,
            _ => Color.red
        };
    }
    
    Color GetBackgroundColor(string grade)
    {
        return grade switch
        {
            "PERFECT" => perfectColor,
            "GOOD" => goodColor,
            "OKAY" => okayColor,
            _ => failColor
        };
    }
    
    string GetGradeMessage(string grade)
    {
        return grade switch
        {
            "PERFECT" => "All meters in target range!\nPerfect cooking!",
            "GOOD" => "2 out of 3 meters in range.\nGood job!",
            "OKAY" => "1 meter in range.\nKeep practicing!",
            _ => "No meters in range.\nTry again!"
        };
    }
    
    void OnContinueClicked()
    {
        Debug.Log("[ResultPanel] Continue button clicked!");
        
        continueButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        
        DOVirtual.DelayedCall(0.3f, () => {
            Hide();
            GameManager.Instance.ReturnToMenu();
        });
    }
    
    public void Show()
    {
        Debug.Log("[ResultPanel] Show()");
        
        gameObject.SetActive(true);
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.5f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.4f));
        showSequence.Join(transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic));
        
        if (resultTitleText != null)
        {
            resultTitleText.transform.localScale = Vector3.zero;
            resultTitleText.transform.DOScale(1f, 0.4f).SetDelay(0.2f).SetEase(Ease.OutBack);
        }
        
        if (gradeText != null)
        {
            gradeText.transform.localScale = Vector3.zero;
            gradeText.transform.DOScale(1f, 0.4f).SetDelay(0.35f).SetEase(Ease.OutBack);
        }
        
        AnimateMeterStatus();
        AnimateRewardSection();
        
        Debug.Log("[ResultPanel] ✅ Show animation started");
    }
    
    void AnimateMeterStatus()
    {
        float delay = 0.5f;
        
        TMP_Text[] statusTexts = { tasteStatusText, stabilityStatusText, magicStatusText };
        
        foreach (var text in statusTexts)
        {
            if (text != null)
            {
                text.transform.localScale = Vector3.zero;
                text.transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                delay += elementStagger;
            }
        }
    }
    
    void AnimateRewardSection()
    {
        float delay = 1.0f;
        
        TMP_Text[] rewardTexts = { baseRewardText, meterBonusText, turnsBonusText };
        
        foreach (var text in rewardTexts)
        {
            if (text != null && text.gameObject.activeSelf)
            {
                text.transform.localScale = Vector3.zero;
                text.transform.DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                delay += elementStagger;
            }
        }
        
        if (finalRewardText != null)
        {
            finalRewardText.transform.localScale = Vector3.zero;
            finalRewardText.transform.DOScale(1f, 0.4f).SetDelay(delay).SetEase(Ease.OutBack);
            
            if (currentResult != null && currentResult.finalReward > 0)
            {
                DOVirtual.DelayedCall(delay + 0.5f, () => AnimateRewardCounter(currentResult.finalReward));
            }
        }
    }
    
    public void Hide()
    {
        Debug.Log("[ResultPanel] Hide()");
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log("[ResultPanel] ✅ Hide animation started");
    }
}
