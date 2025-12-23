using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingsPanel : MonoBehaviour
{
    [Header("Audio Controls")]
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle sfxToggle;
    public Toggle musicToggle;
    
    [Header("Haptics Controls")]
    public Toggle hapticsToggle;
    
    [Header("Labels")]
    public TMP_Text sfxVolumeText;
    public TMP_Text musicVolumeText;
    
    [Header("Buttons")]
    public Button closeButton;
    public Button resetButton;
    
    void Awake()
    {
        Debug.Log("[SettingsPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[SettingsPanel] Start() called");
        SetupControls();
        LoadSettings();
    }
    
    void SetupControls()
    {
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        }
        
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }
        
        if (hapticsToggle != null)
        {
            hapticsToggle.onValueChanged.AddListener(OnHapticsToggleChanged);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetClicked);
        }
        
        Debug.Log("[SettingsPanel] ✅ Controls setup complete");
    }
    
    void LoadSettings()
    {
        if (AudioManager.Instance != null)
        {
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;
            }
            
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = AudioManager.Instance.musicVolume;
            }
            
            if (sfxToggle != null)
            {
                sfxToggle.isOn = AudioManager.Instance.sfxEnabled;
            }
            
            if (musicToggle != null)
            {
                musicToggle.isOn = AudioManager.Instance.musicEnabled;
            }
        }
        
        if (hapticsToggle != null)
        {
            hapticsToggle.isOn = HapticController.IsEnabled();
        }
        
        UpdateLabels();
        Debug.Log("[SettingsPanel] ✅ Settings loaded");
    }
    
    void UpdateLabels()
    {
        if (sfxVolumeText != null && sfxVolumeSlider != null)
        {
            sfxVolumeText.text = $"{Mathf.RoundToInt(sfxVolumeSlider.value * 100)}%";
        }
        
        if (musicVolumeText != null && musicVolumeSlider != null)
        {
            musicVolumeText.text = $"{Mathf.RoundToInt(musicVolumeSlider.value * 100)}%";
        }
    }
    
    void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
        UpdateLabels();
    }
    
    void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
        UpdateLabels();
    }
    
    void OnSFXToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.sfxEnabled != isOn)
        {
            AudioManager.Instance.ToggleSFX();
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.interactable = isOn;
        }
    }
    
    void OnMusicToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.musicEnabled != isOn)
        {
            AudioManager.Instance.ToggleMusic();
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.interactable = isOn;
        }
    }
    
    void OnHapticsToggleChanged(bool isOn)
    {
        HapticController.SetEnabled(isOn);
        
        if (isOn)
        {
            HapticController.LightImpact();
        }
    }
    
    void OnCloseClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
        
        Hide();
    }
    
    void OnResetClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            AudioManager.Instance.SetSFXVolume(1f);
            AudioManager.Instance.SetMusicVolume(0.5f);
            
            if (!AudioManager.Instance.sfxEnabled)
                AudioManager.Instance.ToggleSFX();
            if (!AudioManager.Instance.musicEnabled)
                AudioManager.Instance.ToggleMusic();
        }
        
        HapticController.SetEnabled(true);
        
        LoadSettings();
        
        Debug.Log("[SettingsPanel] ✅ Settings reset to defaults");
    }
    
    public void Show()
    {
        Debug.Log("[SettingsPanel] Show()");
        
        gameObject.SetActive(true);
        LoadSettings();
        
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
        Debug.Log("[SettingsPanel] Hide()");
        
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
}
