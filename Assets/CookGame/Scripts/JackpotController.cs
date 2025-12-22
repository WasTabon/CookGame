using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public enum JackpotEffectType
{
    MeterBoost,
    WildMultiplier,
    ZoneShield,
    TripleApply
}

public class JackpotController : MonoBehaviour
{
    [Header("Jackpot Settings")]
    public int minRollsForJackpot = 5;
    public int maxRollsForJackpot = 10;
    
    [Header("UI References")]
    public GameObject jackpotPanel;
    public TMP_Text jackpotTitleText;
    public Button[] effectButtons;
    public TMP_Text[] effectButtonTexts;
    
    [Header("Effect Descriptions")]
    public string meterBoostDesc = "+10 to ALL meters";
    public string wildMultiplierDesc = "x2 effect on selected";
    public string zoneShieldDesc = "Block ONE overflow";
    public string tripleApplyDesc = "Apply ALL 3 ingredients";
    
    [Header("Visual Effects")]
    public Image flashOverlay;
    public Color jackpotFlashColor = new Color(1f, 0.84f, 0f, 0.5f);
    
    public bool IsJackpotActive { get; private set; }
    public JackpotEffectType? SelectedEffect { get; private set; }
    public bool HasActiveWildMultiplier { get; private set; }
    
    public event Action<JackpotEffectType> OnEffectSelected;
    public event Action OnJackpotTriggered;
    
    private int currentRollCount;
    private int rollsUntilJackpot;
    
    void Awake()
    {
        Debug.Log("[JackpotController] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[JackpotController] Start() called");
        SetupButtons();
        HideJackpotPanel();
    }
    
    void SetupButtons()
    {
        string[] descriptions = { meterBoostDesc, wildMultiplierDesc, zoneShieldDesc, tripleApplyDesc };
        JackpotEffectType[] effects = { 
            JackpotEffectType.MeterBoost, 
            JackpotEffectType.WildMultiplier, 
            JackpotEffectType.ZoneShield, 
            JackpotEffectType.TripleApply 
        };
        
        for (int i = 0; i < effectButtons.Length && i < 4; i++)
        {
            int index = i;
            if (effectButtons[i] != null)
            {
                effectButtons[i].onClick.AddListener(() => SelectEffect(effects[index]));
                
                if (effectButtonTexts != null && i < effectButtonTexts.Length && effectButtonTexts[i] != null)
                {
                    effectButtonTexts[i].text = descriptions[index];
                }
                
                Debug.Log($"[JackpotController] ✅ Effect button {i} configured: {effects[i]}");
            }
        }
    }
    
    public void ResetForNewCooking()
    {
        Debug.Log("[JackpotController] Reset for new cooking session");
        
        currentRollCount = 0;
        rollsUntilJackpot = UnityEngine.Random.Range(minRollsForJackpot, maxRollsForJackpot + 1);
        IsJackpotActive = false;
        SelectedEffect = null;
        HasActiveWildMultiplier = false;
        
        Debug.Log($"[JackpotController] Next jackpot in {rollsUntilJackpot} rolls");
    }
    
    public bool CheckForJackpot(IngredientInstance[] ingredients)
    {
        currentRollCount++;
        Debug.Log($"[JackpotController] Roll #{currentRollCount} (Jackpot at roll #{rollsUntilJackpot})");
        
        bool naturalJackpot = ingredients[0].data == ingredients[1].data &&
                             ingredients[1].data == ingredients[2].data;
        
        bool guaranteedJackpot = currentRollCount >= rollsUntilJackpot;
        
        if (naturalJackpot)
        {
            Debug.Log($"[JackpotController] 🎰 NATURAL JACKPOT! Three {ingredients[0].data.ingredientName}!");
        }
        
        if (guaranteedJackpot && !naturalJackpot)
        {
            Debug.Log("[JackpotController] 🎰 GUARANTEED JACKPOT triggered!");
        }
        
        if (naturalJackpot || guaranteedJackpot)
        {
            TriggerJackpot();
            return true;
        }
        
        return false;
    }
    
    void TriggerJackpot()
    {
        Debug.Log("[JackpotController] 🎰🎰🎰 JACKPOT TRIGGERED! 🎰🎰🎰");
        
        IsJackpotActive = true;
        
        rollsUntilJackpot = currentRollCount + UnityEngine.Random.Range(minRollsForJackpot, maxRollsForJackpot + 1);
        Debug.Log($"[JackpotController] Next jackpot scheduled at roll #{rollsUntilJackpot}");
        
        PlayJackpotAnimation();
        OnJackpotTriggered?.Invoke();
    }
    
    void PlayJackpotAnimation()
    {
        Debug.Log("[JackpotController] Playing jackpot animation");
        
        if (flashOverlay != null)
        {
            flashOverlay.gameObject.SetActive(true);
            flashOverlay.color = new Color(jackpotFlashColor.r, jackpotFlashColor.g, jackpotFlashColor.b, 0f);
            
            Sequence flashSequence = DOTween.Sequence();
            flashSequence.Append(flashOverlay.DOFade(jackpotFlashColor.a, 0.2f));
            flashSequence.Append(flashOverlay.DOFade(0f, 0.2f));
            flashSequence.SetLoops(3);
            flashSequence.OnComplete(() => {
                flashOverlay.gameObject.SetActive(false);
                ShowJackpotPanel();
            });
        }
        else
        {
            ShowJackpotPanel();
        }
    }
    
    void ShowJackpotPanel()
    {
        Debug.Log("[JackpotController] Showing jackpot panel");
        
        if (jackpotPanel != null)
        {
            jackpotPanel.SetActive(true);
            jackpotPanel.transform.localScale = Vector3.zero;
            
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(jackpotPanel.transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack));
            showSequence.Append(jackpotPanel.transform.DOScale(1f, 0.1f));
            
            if (jackpotTitleText != null)
            {
                jackpotTitleText.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 5).SetDelay(0.3f);
            }
        }
    }
    
    void HideJackpotPanel()
    {
        Debug.Log("[JackpotController] Hiding jackpot panel");
        
        if (jackpotPanel != null)
        {
            jackpotPanel.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
                .OnComplete(() => jackpotPanel.SetActive(false));
        }
    }
    
    void SelectEffect(JackpotEffectType effect)
    {
        Debug.Log($"[JackpotController] Effect selected: {effect}");
        
        SelectedEffect = effect;
        IsJackpotActive = false;
        
        if (effect == JackpotEffectType.WildMultiplier)
        {
            HasActiveWildMultiplier = true;
        }
        
        HideJackpotPanel();
        OnEffectSelected?.Invoke(effect);
    }
    
    public void ClearWildMultiplier()
    {
        HasActiveWildMultiplier = false;
        Debug.Log("[JackpotController] Wild multiplier cleared");
    }
    
    public float GetWildMultiplier()
    {
        return HasActiveWildMultiplier ? 2f : 1f;
    }
}
