using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class FireBoostController : MonoBehaviour
{
    [Header("Fire Boost Settings")]
    public float boostPerSecond = 3f;
    
    [Header("Duration Options")]
    public float[] durationOptions = { 2f, 3f, 5f };
    
    [Header("UI References")]
    public Button fireBoostButton;
    public GameObject durationPanel;
    public Button[] durationButtons;
    public TMP_Text[] durationButtonTexts;
    public Image fireBoostIcon;
    public TMP_Text timerText;
    public Image timerFillImage;
    
    [Header("Visual Effects")]
    public Color normalColor = Color.white;
    public Color activeColor = new Color(1f, 0.5f, 0f);
    public float pulseScale = 1.2f;
    
    public bool IsBoostActive { get; private set; }
    public float RemainingTime { get; private set; }
    
    public event Action<float, float, float> OnBoostTick;
    public event Action OnBoostEnded;
    
    private float currentDuration;
    private float elapsedTime;
    private Tween pulseTween;
    private bool canActivate = true;
    
    void Awake()
    {
        Debug.Log("[FireBoostController] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[FireBoostController] Start() called");
        
        SetupUI();
        HideDurationPanel();
        HideTimer();
    }
    
    void SetupUI()
    {
        Debug.Log("[FireBoostController] Setting up UI...");
        
        if (fireBoostButton != null)
        {
            fireBoostButton.onClick.AddListener(OnFireBoostButtonClicked);
            Debug.Log("[FireBoostController] ✅ Fire Boost button listener added");
        }
        else
        {
            Debug.LogError("[FireBoostController] ❌ Fire Boost button is NULL!");
        }
        
        for (int i = 0; i < durationButtons.Length && i < durationOptions.Length; i++)
        {
            int index = i;
            if (durationButtons[i] != null)
            {
                durationButtons[i].onClick.AddListener(() => SelectDuration(index));
                
                if (durationButtonTexts != null && i < durationButtonTexts.Length && durationButtonTexts[i] != null)
                {
                    durationButtonTexts[i].text = $"{durationOptions[index]}s";
                }
                
                Debug.Log($"[FireBoostController] ✅ Duration button {i} set to {durationOptions[index]}s");
            }
        }
    }
    
    void OnFireBoostButtonClicked()
    {
        Debug.Log("[FireBoostController] Fire Boost button clicked");
        
        if (!canActivate)
        {
            Debug.Log("[FireBoostController] ⚠️ Cannot activate - boost not available");
            return;
        }
        
        if (IsBoostActive)
        {
            Debug.Log("[FireBoostController] ⚠️ Boost already active");
            return;
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        ShowDurationPanel();
    }
    
    void ShowDurationPanel()
    {
        Debug.Log("[FireBoostController] Showing duration panel");
        
        if (durationPanel != null)
        {
            durationPanel.SetActive(true);
            durationPanel.transform.localScale = Vector3.zero;
            durationPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayPanelOpen();
            }
        }
    }
    
    void HideDurationPanel()
    {
        Debug.Log("[FireBoostController] Hiding duration panel");
        
        if (durationPanel != null)
        {
            durationPanel.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
                .OnComplete(() => durationPanel.SetActive(false));
        }
    }
    
    void SelectDuration(int index)
    {
        Debug.Log($"[FireBoostController] Duration selected: {durationOptions[index]}s");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        currentDuration = durationOptions[index];
        HideDurationPanel();
        StartBoost();
    }
    
    void StartBoost()
    {
        Debug.Log($"[FireBoostController] 🔥 Starting Fire Boost for {currentDuration}s");
        
        IsBoostActive = true;
        elapsedTime = 0f;
        RemainingTime = currentDuration;
        canActivate = false;
        
        ShowTimer();
        StartPulseAnimation();
        
        if (fireBoostIcon != null)
        {
            fireBoostIcon.DOColor(activeColor, 0.3f);
        }
        
        if (fireBoostButton != null)
        {
            fireBoostButton.interactable = false;
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFireBoostStart();
        }
    }
    
    void Update()
    {
        if (!IsBoostActive) return;
        
        elapsedTime += Time.deltaTime;
        RemainingTime = Mathf.Max(0f, currentDuration - elapsedTime);
        
        UpdateTimerUI();
        
        float boostAmount = boostPerSecond * Time.deltaTime;
        Debug.Log($"[FireBoostController] 🔥 Boost tick: +{boostAmount:F2} to all meters");
        OnBoostTick?.Invoke(boostAmount, boostAmount, boostAmount);
        
        if (elapsedTime >= currentDuration)
        {
            EndBoost();
        }
    }
    
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = RemainingTime.ToString("F1") + "s";
        }
        
        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = RemainingTime / currentDuration;
        }
    }
    
    void EndBoost()
    {
        Debug.Log("[FireBoostController] 🔥 Fire Boost ended");
        
        IsBoostActive = false;
        
        StopPulseAnimation();
        HideTimer();
        
        if (fireBoostIcon != null)
        {
            fireBoostIcon.DOColor(normalColor, 0.3f);
        }
        
        OnBoostEnded?.Invoke();
    }
    
    void ShowTimer()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.transform.localScale = Vector3.zero;
            timerText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        }
        
        if (timerFillImage != null)
        {
            timerFillImage.gameObject.SetActive(true);
            timerFillImage.fillAmount = 1f;
        }
    }
    
    void HideTimer()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
        
        if (timerFillImage != null)
        {
            timerFillImage.gameObject.SetActive(false);
        }
    }
    
    void StartPulseAnimation()
    {
        if (fireBoostIcon != null)
        {
            pulseTween = fireBoostIcon.transform
                .DOScale(pulseScale, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
    
    void StopPulseAnimation()
    {
        pulseTween?.Kill();
        
        if (fireBoostIcon != null)
        {
            fireBoostIcon.transform.DOScale(1f, 0.2f);
        }
    }
    
    public void ResetForNewTurn()
    {
        Debug.Log("[FireBoostController] Reset for new turn");
        
        if (!IsBoostActive)
        {
            canActivate = true;
            
            if (fireBoostButton != null)
            {
                fireBoostButton.interactable = true;
            }
        }
    }
    
    public void DisableBoost()
    {
        Debug.Log("[FireBoostController] Boost disabled");
        
        canActivate = false;
        
        if (fireBoostButton != null)
        {
            fireBoostButton.interactable = false;
        }
        
        if (IsBoostActive)
        {
            EndBoost();
        }
    }
    
    public void EnableBoost()
    {
        Debug.Log("[FireBoostController] Boost enabled");
        
        canActivate = true;
        
        if (fireBoostButton != null && !IsBoostActive)
        {
            fireBoostButton.interactable = true;
        }
    }
    
    void OnDestroy()
    {
        pulseTween?.Kill();
    }
}
