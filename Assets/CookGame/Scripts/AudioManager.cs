using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    
    [Header("UI Sounds")]
    public AudioClip buttonClick;
    public AudioClip panelOpen;
    public AudioClip panelClose;
    
    [Header("Cooking Sounds")]
    public AudioClip ingredientSelect;
    public AudioClip ingredientRoll;
    public AudioClip meterIncrease;
    public AudioClip meterInTarget;
    public AudioClip meterOverflow;
    
    [Header("Game Events")]
    public AudioClip victorySound;
    public AudioClip defeatSound;
    public AudioClip jackpotSound;
    public AudioClip shieldActivate;
    public AudioClip shieldBlock;
    
    [Header("Fire Boost")]
    public AudioClip fireBoostStart;
    public AudioClip fireBoostTick;
    public AudioClip fireBoostEnd;
    
    [Header("Currency")]
    public AudioClip coinCollect;
    public AudioClip coinCounter;
    
    [Header("Settings")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    public bool sfxEnabled = true;
    public bool musicEnabled = true;
    
    private Dictionary<string, float> lastPlayTimes = new Dictionary<string, float>();
    private const float MIN_PLAY_INTERVAL = 0.05f;
    
    void Awake()
    {
        Debug.Log("[AudioManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
            Debug.Log("[AudioManager] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[AudioManager] ⚠️ Duplicate instance destroyed");
            Destroy(gameObject);
        }
    }
    
    void SetupAudioSources()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
        
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }
        
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
    }
    
    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (!sfxEnabled || clip == null || sfxSource == null) return;
        
        string clipName = clip.name;
        if (lastPlayTimes.ContainsKey(clipName))
        {
            if (Time.time - lastPlayTimes[clipName] < MIN_PLAY_INTERVAL)
            {
                return;
            }
        }
        lastPlayTimes[clipName] = Time.time;
        
        sfxSource.PlayOneShot(clip, sfxVolume * volumeMultiplier);
        Debug.Log($"[AudioManager] 🔊 Playing: {clip.name}");
    }
    
    public void PlaySFXWithPitch(AudioClip clip, float pitch, float volumeMultiplier = 1f)
    {
        if (!sfxEnabled || clip == null || sfxSource == null) return;
        
        float originalPitch = sfxSource.pitch;
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip, sfxVolume * volumeMultiplier);
        sfxSource.pitch = originalPitch;
    }
    
    public void PlayButtonClick()
    {
        PlaySFX(buttonClick, 0.7f);
        HapticController.LightImpact();
    }
    
    public void PlayPanelOpen()
    {
        PlaySFX(panelOpen);
    }
    
    public void PlayPanelClose()
    {
        PlaySFX(panelClose, 0.8f);
    }
    
    public void PlayIngredientSelect()
    {
        PlaySFX(ingredientSelect);
        HapticController.MediumImpact();
    }
    
    public void PlayIngredientRoll()
    {
        PlaySFX(ingredientRoll);
    }
    
    public void PlayMeterIncrease()
    {
        PlaySFX(meterIncrease, 0.5f);
        HapticController.SelectionChanged();
    }
    
    public void PlayMeterInTarget()
    {
        PlaySFX(meterInTarget);
        HapticController.Success();
    }
    
    public void PlayMeterOverflow()
    {
        PlaySFX(meterOverflow);
        HapticController.HeavyImpact();
    }
    
    public void PlayVictory()
    {
        PlaySFX(victorySound);
        HapticController.Success();
    }
    
    public void PlayDefeat()
    {
        PlaySFX(defeatSound);
        HapticController.Error();
    }
    
    public void PlayJackpot()
    {
        PlaySFX(jackpotSound);
        HapticController.HeavyImpact();
    }
    
    public void PlayShieldActivate()
    {
        PlaySFX(shieldActivate);
        HapticController.MediumImpact();
    }
    
    public void PlayShieldBlock()
    {
        PlaySFX(shieldBlock);
        HapticController.HeavyImpact();
    }
    
    public void PlayFireBoostStart()
    {
        PlaySFX(fireBoostStart);
        HapticController.MediumImpact();
    }
    
    public void PlayFireBoostTick()
    {
        PlaySFX(fireBoostTick, 0.3f);
    }
    
    public void PlayFireBoostEnd()
    {
        PlaySFX(fireBoostEnd);
    }
    
    public void PlayCoinCollect()
    {
        PlaySFX(coinCollect);
        HapticController.LightImpact();
    }
    
    public void PlayCoinCounter()
    {
        PlaySFX(coinCounter, 0.4f);
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        Debug.Log($"[AudioManager] SFX Volume: {sfxVolume}");
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        Debug.Log($"[AudioManager] Music Volume: {musicVolume}");
    }
    
    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        PlayerPrefs.SetInt("SFXEnabled", sfxEnabled ? 1 : 0);
        Debug.Log($"[AudioManager] SFX Enabled: {sfxEnabled}");
    }
    
    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        
        if (musicSource != null)
        {
            if (musicEnabled)
                musicSource.UnPause();
            else
                musicSource.Pause();
        }
        Debug.Log($"[AudioManager] Music Enabled: {musicEnabled}");
    }
    
    void Start()
    {
        LoadSettings();
    }
    
    void LoadSettings()
    {
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        
        if (sfxSource != null) sfxSource.volume = sfxVolume;
        if (musicSource != null) musicSource.volume = musicVolume;
        
        Debug.Log($"[AudioManager] ✅ Settings loaded - SFX: {sfxVolume}, Music: {musicVolume}");
    }
}
