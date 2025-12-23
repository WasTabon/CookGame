using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class BoosterSelectionPanel : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text titleText;
    
    [Header("Booster Buttons")]
    public Button extraTurnButton;
    public TMP_Text extraTurnCountText;
    public Button shieldButton;
    public TMP_Text shieldCountText;
    public Button doubleCoinsButton;
    public TMP_Text doubleCoinsCountText;
    public Button doubleXPButton;
    public TMP_Text doubleXPCountText;
    
    [Header("Selected Indicators")]
    public GameObject extraTurnSelected;
    public GameObject shieldSelected;
    public GameObject doubleCoinsSelected;
    public GameObject doubleXPSelected;
    
    [Header("Buttons")]
    public Button startButton;
    public Button closeButton;
    
    [Header("Colors")]
    public Color availableColor = new Color(0.3f, 0.6f, 0.3f, 1f);
    public Color unavailableColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public Color selectedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    
    private bool extraTurnSelected_flag = false;
    private bool shieldSelected_flag = false;
    private bool doubleCoinsSelected_flag = false;
    private bool doubleXPSelected_flag = false;
    
    public event Action OnStartClicked;
    public event Action OnCancelled;
    
    void Awake()
    {
        Debug.Log("[BoosterSelectionPanel] Awake() called");
    }
    
    void Start()
    {
        SetupButtons();
        HideAllSelectedIndicators();
    }
    
    void SetupButtons()
    {
        if (extraTurnButton != null)
            extraTurnButton.onClick.AddListener(() => ToggleBooster(ShopItem.BoosterType.ExtraTurn));
        
        if (shieldButton != null)
            shieldButton.onClick.AddListener(() => ToggleBooster(ShopItem.BoosterType.Shield));
        
        if (doubleCoinsButton != null)
            doubleCoinsButton.onClick.AddListener(() => ToggleBooster(ShopItem.BoosterType.DoubleCoins));
        
        if (doubleXPButton != null)
            doubleXPButton.onClick.AddListener(() => ToggleBooster(ShopItem.BoosterType.DoubleXP));
        
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseButtonClicked);
    }
    
    public void Show()
    {
        Debug.Log("[BoosterSelectionPanel] Show()");
        
        gameObject.SetActive(true);
        
        ResetSelections();
        UpdateAllBoosterButtons();
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelOpen();
        }
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.8f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.3f));
        showSequence.Join(transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
    }
    
    public void Hide()
    {
        Debug.Log("[BoosterSelectionPanel] Hide()");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPanelClose();
        }
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
    }
    
    void ResetSelections()
    {
        extraTurnSelected_flag = false;
        shieldSelected_flag = false;
        doubleCoinsSelected_flag = false;
        doubleXPSelected_flag = false;
        HideAllSelectedIndicators();
    }
    
    void HideAllSelectedIndicators()
    {
        if (extraTurnSelected != null) extraTurnSelected.SetActive(false);
        if (shieldSelected != null) shieldSelected.SetActive(false);
        if (doubleCoinsSelected != null) doubleCoinsSelected.SetActive(false);
        if (doubleXPSelected != null) doubleXPSelected.SetActive(false);
    }
    
    void UpdateAllBoosterButtons()
    {
        UpdateBoosterButton(ShopItem.BoosterType.ExtraTurn, extraTurnButton, extraTurnCountText, extraTurnSelected, extraTurnSelected_flag);
        UpdateBoosterButton(ShopItem.BoosterType.Shield, shieldButton, shieldCountText, shieldSelected, shieldSelected_flag);
        UpdateBoosterButton(ShopItem.BoosterType.DoubleCoins, doubleCoinsButton, doubleCoinsCountText, doubleCoinsSelected, doubleCoinsSelected_flag);
        UpdateBoosterButton(ShopItem.BoosterType.DoubleXP, doubleXPButton, doubleXPCountText, doubleXPSelected, doubleXPSelected_flag);
    }
    
    void UpdateBoosterButton(ShopItem.BoosterType type, Button button, TMP_Text countText, GameObject selectedIndicator, bool isSelected)
    {
        if (button == null) return;
        
        int count = 0;
        if (BoosterManager.Instance != null)
        {
            count = BoosterManager.Instance.GetBoosterCount(type);
        }
        
        bool hasBooster = count > 0;
        button.interactable = hasBooster;
        
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (isSelected)
                buttonImage.color = selectedColor;
            else if (hasBooster)
                buttonImage.color = availableColor;
            else
                buttonImage.color = unavailableColor;
        }
        
        if (countText != null)
        {
            countText.text = count.ToString();
            countText.color = hasBooster ? Color.white : Color.gray;
        }
        
        if (selectedIndicator != null)
        {
            selectedIndicator.SetActive(isSelected);
        }
    }
    
    void ToggleBooster(ShopItem.BoosterType type)
    {
        if (BoosterManager.Instance == null) return;
        
        if (!BoosterManager.Instance.HasBooster(type))
        {
            Debug.Log($"[BoosterSelectionPanel] No {type} boosters available");
            return;
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        switch (type)
        {
            case ShopItem.BoosterType.ExtraTurn:
                extraTurnSelected_flag = !extraTurnSelected_flag;
                break;
            case ShopItem.BoosterType.Shield:
                shieldSelected_flag = !shieldSelected_flag;
                break;
            case ShopItem.BoosterType.DoubleCoins:
                doubleCoinsSelected_flag = !doubleCoinsSelected_flag;
                break;
            case ShopItem.BoosterType.DoubleXP:
                doubleXPSelected_flag = !doubleXPSelected_flag;
                break;
        }
        
        UpdateAllBoosterButtons();
        
        HapticController.LightImpact();
    }
    
    void OnStartButtonClicked()
    {
        Debug.Log("[BoosterSelectionPanel] Start clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        ApplySelectedBoosters();
        
        Hide();
        OnStartClicked?.Invoke();
    }
    
    void ApplySelectedBoosters()
    {
        if (BoosterManager.Instance == null) return;
        
        BoosterManager.Instance.ResetActiveBoostersForNewGame();
        
        if (extraTurnSelected_flag)
        {
            BoosterManager.Instance.ActivateBoosterForGame(ShopItem.BoosterType.ExtraTurn);
            BoosterManager.Instance.ActiveBoosters.extraTurnsThisGame = 1;
        }
        
        if (shieldSelected_flag)
        {
            BoosterManager.Instance.ActivateBoosterForGame(ShopItem.BoosterType.Shield);
            BoosterManager.Instance.ActiveBoosters.shieldActive = true;
        }
        
        if (doubleCoinsSelected_flag)
        {
            BoosterManager.Instance.ActivateBoosterForGame(ShopItem.BoosterType.DoubleCoins);
        }
        
        if (doubleXPSelected_flag)
        {
            BoosterManager.Instance.ActivateBoosterForGame(ShopItem.BoosterType.DoubleXP);
        }
    }
    
    void OnCloseButtonClicked()
    {
        Debug.Log("[BoosterSelectionPanel] Close clicked");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Hide();
        OnCancelled?.Invoke();
    }
}
