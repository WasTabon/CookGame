using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CookingManager : MonoBehaviour
{
    [Header("Ingredient Database")]
    public List<IngredientData> allIngredients = new List<IngredientData>();
    
    [Header("Meter References")]
    public MeterController tasteMeter;
    public MeterController stabilityMeter;
    public MeterController magicMeter;
    
    [Header("Slot References")]
    public IngredientSlot slot1;
    public IngredientSlot slot2;
    public IngredientSlot slot3;
    
    [Header("Controllers")]
    public FireBoostController fireBoostController;
    public JackpotController jackpotController;
    public ShieldController shieldController;
    public RewardCalculator rewardCalculator;
    
    public RecipeData currentRecipe { get; private set; }
    public int turnsRemaining { get; private set; }
    public int turnsUsed { get; private set; }
    public bool isGameOver { get; private set; }
    public bool canServeEarly { get; private set; }
    
    private IngredientInstance[] currentIngredients = new IngredientInstance[3];
    private bool waitingForSelection = false;
    private bool waitingForJackpotSelection = false;
    private bool waitingForShieldSelection = false;
    
    void Awake()
    {
        Debug.Log("[CookingManager] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[CookingManager] Start() called");
        ValidateReferences();
        SetupControllers();
    }
    
    void ValidateReferences()
    {
        if (allIngredients == null || allIngredients.Count == 0)
            Debug.LogError("[CookingManager] ❌ No ingredients in database!");
        else
            Debug.Log($"[CookingManager] ✅ {allIngredients.Count} ingredients loaded");
        
        if (tasteMeter == null)
            Debug.LogError("[CookingManager] ❌ Taste Meter is NULL!");
        if (stabilityMeter == null)
            Debug.LogError("[CookingManager] ❌ Stability Meter is NULL!");
        if (magicMeter == null)
            Debug.LogError("[CookingManager] ❌ Magic Meter is NULL!");
        
        if (slot1 == null)
            Debug.LogError("[CookingManager] ❌ Slot1 is NULL!");
        if (slot2 == null)
            Debug.LogError("[CookingManager] ❌ Slot2 is NULL!");
        if (slot3 == null)
            Debug.LogError("[CookingManager] ❌ Slot3 is NULL!");
        
        if (fireBoostController == null)
            Debug.LogWarning("[CookingManager] ⚠️ FireBoostController is NULL");
        else
            Debug.Log("[CookingManager] ✅ FireBoostController found");
        
        if (jackpotController == null)
            Debug.LogWarning("[CookingManager] ⚠️ JackpotController is NULL");
        else
            Debug.Log("[CookingManager] ✅ JackpotController found");
        
        if (shieldController == null)
            Debug.LogWarning("[CookingManager] ⚠️ ShieldController is NULL");
        else
            Debug.Log("[CookingManager] ✅ ShieldController found");
        
        if (rewardCalculator == null)
            Debug.LogWarning("[CookingManager] ⚠️ RewardCalculator is NULL");
        else
            Debug.Log("[CookingManager] ✅ RewardCalculator found");
    }
    
    void SetupControllers()
    {
        if (fireBoostController != null)
        {
            fireBoostController.OnBoostTick += OnFireBoostTick;
            fireBoostController.OnBoostEnded += OnFireBoostEnded;
            Debug.Log("[CookingManager] ✅ Fire Boost events subscribed");
        }
        
        if (jackpotController != null)
        {
            jackpotController.OnEffectSelected += OnJackpotEffectSelected;
            jackpotController.OnJackpotTriggered += OnJackpotTriggered;
            Debug.Log("[CookingManager] ✅ Jackpot events subscribed");
        }
        
        if (shieldController != null)
        {
            shieldController.OnShieldActivated += OnShieldActivated;
            shieldController.OnShieldUsed += OnShieldUsed;
            Debug.Log("[CookingManager] ✅ Shield events subscribed");
        }
    }
    
    void OnFireBoostTick(float tasteBoost, float stabilityBoost, float magicBoost)
    {
        Debug.Log($"[CookingManager] 🔥 Fire Boost tick: T+{tasteBoost:F2} S+{stabilityBoost:F2} M+{magicBoost:F2}");
        
        if (isGameOver) return;
        
        tasteMeter.AddValue(tasteBoost);
        stabilityMeter.AddValue(stabilityBoost);
        magicMeter.AddValue(magicBoost);
        
        CheckForOverflow();
    }
    
    void OnFireBoostEnded()
    {
        Debug.Log("[CookingManager] 🔥 Fire Boost ended notification received");
    }
    
    void OnJackpotTriggered()
    {
        Debug.Log("[CookingManager] 🎰 Jackpot triggered - waiting for effect selection");
        waitingForJackpotSelection = true;
        waitingForSelection = false;
    }
    
    void OnJackpotEffectSelected(JackpotEffectType effect)
    {
        Debug.Log($"[CookingManager] 🎰 Jackpot effect selected: {effect}");
        waitingForJackpotSelection = false;
        
        switch (effect)
        {
            case JackpotEffectType.MeterBoost:
                ApplyMeterBoost();
                break;
            case JackpotEffectType.WildMultiplier:
                Debug.Log("[CookingManager] 🎰 Wild Multiplier active - next ingredient x2");
                break;
            case JackpotEffectType.ZoneShield:
                if (shieldController != null)
                {
                    waitingForShieldSelection = true;
                    shieldController.ActivateShieldSelection();
                    return;
                }
                break;
            case JackpotEffectType.TripleApply:
                ApplyTripleIngredients();
                return;
        }
        
        waitingForSelection = true;
    }
    
    void ApplyMeterBoost()
    {
        Debug.Log("[CookingManager] 🎰 Applying Meter Boost: +10 to all meters");
        
        tasteMeter.AddValue(10f);
        stabilityMeter.AddValue(10f);
        magicMeter.AddValue(10f);
        
        CheckForOverflow();
    }
    
    void ApplyTripleIngredients()
    {
        Debug.Log("[CookingManager] 🎰 TRIPLE APPLY - Applying all 3 ingredients!");
        
        waitingForSelection = false;
        
        if (fireBoostController != null)
        {
            fireBoostController.DisableBoost();
        }
        
        float totalTaste = 0f;
        float totalStability = 0f;
        float totalMagic = 0f;
        
        for (int i = 0; i < 3; i++)
        {
            totalTaste += currentIngredients[i].tasteEffect;
            totalStability += currentIngredients[i].stabilityEffect;
            totalMagic += currentIngredients[i].magicEffect;
            
            Debug.Log($"[CookingManager]   Ingredient {i + 1}: T+{currentIngredients[i].tasteEffect:F1} S+{currentIngredients[i].stabilityEffect:F1} M+{currentIngredients[i].magicEffect:F1}");
        }
        
        Debug.Log($"[CookingManager]   TOTAL: T+{totalTaste:F1} S+{totalStability:F1} M+{totalMagic:F1}");
        
        tasteMeter.AddValue(totalTaste);
        stabilityMeter.AddValue(totalStability);
        magicMeter.AddValue(totalMagic);
        
        slot1.Hide();
        slot2.Hide();
        slot3.Hide();
        
        DOVirtual.DelayedCall(0.5f, () => CheckGameState());
    }
    
    void OnShieldActivated(MeterType meter)
    {
        Debug.Log($"[CookingManager] 🛡️ Shield activated on: {meter}");
        waitingForShieldSelection = false;
        waitingForSelection = true;
    }
    
    void OnShieldUsed(MeterType meter)
    {
        Debug.Log($"[CookingManager] 🛡️ Shield used to block {meter} overflow!");
    }
    
    public void StartCooking(RecipeData recipe)
    {
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] Starting cooking: {recipe.recipeName}");
        Debug.Log($"[CookingManager] Targets - Taste: {recipe.tasteMin}-{recipe.tasteMax}");
        Debug.Log($"[CookingManager] Targets - Stability: {recipe.stabilityMin}-{recipe.stabilityMax}");
        Debug.Log($"[CookingManager] Targets - Magic: {recipe.magicMin}-{recipe.magicMax}");
        Debug.Log($"[CookingManager] Turns: {recipe.totalTurns}");
        Debug.Log($"[CookingManager] Base Reward: {recipe.baseReward}");
        Debug.Log($"[CookingManager] ========================================");
        
        currentRecipe = recipe;
        turnsRemaining = recipe.totalTurns;
        turnsUsed = 0;
        isGameOver = false;
        canServeEarly = false;
        waitingForJackpotSelection = false;
        waitingForShieldSelection = false;
        
        InitializeMeters();
        
        if (fireBoostController != null)
        {
            fireBoostController.EnableBoost();
        }
        
        if (jackpotController != null)
        {
            jackpotController.ResetForNewCooking();
        }
        
        if (shieldController != null)
        {
            shieldController.ResetForNewCooking();
        }
        
        DOVirtual.DelayedCall(0.5f, () => RollNewIngredients());
    }
    
    void InitializeMeters()
    {
        Debug.Log("[CookingManager] Initializing meters...");
        
        tasteMeter.Initialize(0f, currentRecipe.tasteMin, currentRecipe.tasteMax);
        stabilityMeter.Initialize(0f, currentRecipe.stabilityMin, currentRecipe.stabilityMax);
        magicMeter.Initialize(0f, currentRecipe.magicMin, currentRecipe.magicMax);
        
        Debug.Log("[CookingManager] ✅ Meters initialized");
    }
    
    void RollNewIngredients()
    {
        if (isGameOver)
        {
            Debug.Log("[CookingManager] ⚠️ Game is over, not rolling new ingredients");
            return;
        }
        
        Debug.Log($"[CookingManager] Rolling new ingredients... (Turns remaining: {turnsRemaining})");
        
        if (fireBoostController != null)
        {
            fireBoostController.ResetForNewTurn();
        }
        
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, allIngredients.Count);
            IngredientData ingredientData = allIngredients[randomIndex];
            currentIngredients[i] = ingredientData.GenerateInstance();
            
            Debug.Log($"[CookingManager] Slot {i + 1}: {currentIngredients[i].data.ingredientName}");
            Debug.Log($"[CookingManager]   Effects: T+{currentIngredients[i].tasteEffect:F1} S+{currentIngredients[i].stabilityEffect:F1} M+{currentIngredients[i].magicEffect:F1}");
        }
        
        bool isJackpot = false;
        if (jackpotController != null)
        {
            isJackpot = jackpotController.CheckForJackpot(currentIngredients);
        }
        
        DisplayIngredients();
        
        if (!isJackpot)
        {
            waitingForSelection = true;
        }
    }
    
    void DisplayIngredients()
    {
        Debug.Log("[CookingManager] Displaying ingredients in slots...");
        
        slot1.DisplayIngredient(currentIngredients[0], () => OnIngredientSelected(0));
        slot2.DisplayIngredient(currentIngredients[1], () => OnIngredientSelected(1));
        slot3.DisplayIngredient(currentIngredients[2], () => OnIngredientSelected(2));
        
        Debug.Log("[CookingManager] ✅ All ingredients displayed");
    }
    
    void OnIngredientSelected(int slotIndex)
    {
        if (!waitingForSelection)
        {
            Debug.Log("[CookingManager] ⚠️ Not waiting for selection, ignoring click");
            return;
        }
        
        if (isGameOver)
        {
            Debug.Log("[CookingManager] ⚠️ Game is over, ignoring selection");
            return;
        }
        
        if (waitingForJackpotSelection)
        {
            Debug.Log("[CookingManager] ⚠️ Waiting for jackpot selection, ignoring click");
            return;
        }
        
        if (waitingForShieldSelection)
        {
            Debug.Log("[CookingManager] ⚠️ Waiting for shield selection, ignoring click");
            return;
        }
        
        waitingForSelection = false;
        
        if (fireBoostController != null)
        {
            fireBoostController.DisableBoost();
        }
        
        IngredientInstance selected = currentIngredients[slotIndex];
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] 🍳 Selected: {selected.data.ingredientName}");
        
        float multiplier = 1f;
        if (jackpotController != null && jackpotController.HasActiveWildMultiplier)
        {
            multiplier = jackpotController.GetWildMultiplier();
            jackpotController.ClearWildMultiplier();
            Debug.Log($"[CookingManager] 🎰 Wild Multiplier applied: x{multiplier}");
        }
        
        Debug.Log($"[CookingManager] Applying: T+{selected.tasteEffect * multiplier:F1} S+{selected.stabilityEffect * multiplier:F1} M+{selected.magicEffect * multiplier:F1}");
        
        HideUnselectedSlots(slotIndex);
        
        ApplyIngredient(selected, multiplier);
    }
    
    void HideUnselectedSlots(int selectedIndex)
    {
        Debug.Log($"[CookingManager] Hiding unselected slots (selected: {selectedIndex})");
        
        if (selectedIndex != 0) slot1.Hide();
        if (selectedIndex != 1) slot2.Hide();
        if (selectedIndex != 2) slot3.Hide();
    }
    
    void ApplyIngredient(IngredientInstance ingredient, float multiplier = 1f)
    {
        Debug.Log("[CookingManager] Applying ingredient effects...");
        
        float prevTaste = tasteMeter.CurrentValue;
        float prevStability = stabilityMeter.CurrentValue;
        float prevMagic = magicMeter.CurrentValue;
        
        tasteMeter.AddValue(ingredient.tasteEffect * multiplier);
        stabilityMeter.AddValue(ingredient.stabilityEffect * multiplier);
        magicMeter.AddValue(ingredient.magicEffect * multiplier);
        
        Debug.Log($"[CookingManager] Taste: {prevTaste:F1} → {tasteMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] Stability: {prevStability:F1} → {stabilityMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] Magic: {prevMagic:F1} → {magicMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] ========================================");
        
        turnsUsed++;
        canServeEarly = turnsUsed >= 1;
        
        DOVirtual.DelayedCall(0.5f, () => CheckGameState());
    }
    
    void CheckGameState()
    {
        Debug.Log("[CookingManager] Checking game state...");
        
        if (CheckForOverflow())
        {
            return;
        }
        
        turnsRemaining--;
        Debug.Log($"[CookingManager] Turns remaining: {turnsRemaining}");
        
        if (turnsRemaining <= 0)
        {
            CompleteOrder();
        }
        else
        {
            DOVirtual.DelayedCall(1f, () => RollNewIngredients());
        }
    }
    
    bool CheckForOverflow()
    {
        bool tasteOverflow = tasteMeter.CurrentValue > currentRecipe.tasteMax;
        bool stabilityOverflow = stabilityMeter.CurrentValue > currentRecipe.stabilityMax;
        bool magicOverflow = magicMeter.CurrentValue > currentRecipe.magicMax;
        
        if (tasteOverflow || stabilityOverflow || magicOverflow)
        {
            Debug.Log("[CookingManager] ⚠️ OVERFLOW DETECTED - Checking shield...");
            
            if (shieldController != null && shieldController.HasActiveShield)
            {
                if (tasteOverflow && shieldController.TryBlockOverflow(MeterType.Taste))
                {
                    tasteMeter.SetValue(currentRecipe.tasteMax);
                    return false;
                }
                if (stabilityOverflow && shieldController.TryBlockOverflow(MeterType.Stability))
                {
                    stabilityMeter.SetValue(currentRecipe.stabilityMax);
                    return false;
                }
                if (magicOverflow && shieldController.TryBlockOverflow(MeterType.Magic))
                {
                    magicMeter.SetValue(currentRecipe.magicMax);
                    return false;
                }
            }
            
            Debug.Log("[CookingManager] ❌ OVERFLOW - No shield protection!");
            if (tasteOverflow) Debug.Log($"[CookingManager]   Taste: {tasteMeter.CurrentValue:F1} > {currentRecipe.tasteMax}");
            if (stabilityOverflow) Debug.Log($"[CookingManager]   Stability: {stabilityMeter.CurrentValue:F1} > {currentRecipe.stabilityMax}");
            if (magicOverflow) Debug.Log($"[CookingManager]   Magic: {magicMeter.CurrentValue:F1} > {currentRecipe.magicMax}");
            
            EndGame(null);
            return true;
        }
        
        return false;
    }
    
    public void ServeEarly()
    {
        if (!canServeEarly)
        {
            Debug.Log("[CookingManager] ⚠️ Cannot serve early - need at least 1 ingredient used");
            return;
        }
        
        if (isGameOver)
        {
            Debug.Log("[CookingManager] ⚠️ Game already over");
            return;
        }
        
        Debug.Log("[CookingManager] 🍽️ SERVE EARLY requested!");
        CompleteOrder();
    }
    
    void CompleteOrder()
    {
        Debug.Log("[CookingManager] Completing order...");
        
        if (rewardCalculator != null)
        {
            RewardResult result = rewardCalculator.CalculateReward(
                currentRecipe,
                tasteMeter.CurrentValue,
                stabilityMeter.CurrentValue,
                magicMeter.CurrentValue,
                turnsRemaining,
                currentRecipe.totalTurns
            );
            
            EndGame(result);
        }
        else
        {
            bool victory = tasteMeter.CurrentValue >= currentRecipe.tasteMin && 
                          tasteMeter.CurrentValue <= currentRecipe.tasteMax &&
                          stabilityMeter.CurrentValue >= currentRecipe.stabilityMin && 
                          stabilityMeter.CurrentValue <= currentRecipe.stabilityMax &&
                          magicMeter.CurrentValue >= currentRecipe.magicMin && 
                          magicMeter.CurrentValue <= currentRecipe.magicMax;
            
            EndGame(victory ? new RewardResult { finalReward = currentRecipe.baseReward, grade = "PERFECT" } : null);
        }
    }
    
    void EndGame(RewardResult result)
    {
        isGameOver = true;
        waitingForSelection = false;
        canServeEarly = false;
        
        if (fireBoostController != null)
        {
            fireBoostController.DisableBoost();
        }
        
        if (result != null && result.finalReward > 0)
        {
            Debug.Log($"[CookingManager] 🎉 ORDER COMPLETE! Grade: {result.grade}, Reward: {result.finalReward}");
            
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddCoins(result.finalReward);
            }
        }
        else
        {
            Debug.Log("[CookingManager] 💀 ORDER FAILED!");
        }
        
        GameManager.Instance.uiManager.ShowResultPanel(result);
    }
    
    public int GetCurrentPotentialReward()
    {
        if (rewardCalculator == null || currentRecipe == null) return 0;
        
        return rewardCalculator.GetCurrentPotentialReward(
            currentRecipe,
            tasteMeter.CurrentValue,
            stabilityMeter.CurrentValue,
            magicMeter.CurrentValue,
            turnsRemaining
        );
    }
    
    void OnDestroy()
    {
        if (fireBoostController != null)
        {
            fireBoostController.OnBoostTick -= OnFireBoostTick;
            fireBoostController.OnBoostEnded -= OnFireBoostEnded;
        }
        
        if (jackpotController != null)
        {
            jackpotController.OnEffectSelected -= OnJackpotEffectSelected;
            jackpotController.OnJackpotTriggered -= OnJackpotTriggered;
        }
        
        if (shieldController != null)
        {
            shieldController.OnShieldActivated -= OnShieldActivated;
            shieldController.OnShieldUsed -= OnShieldUsed;
        }
    }
}
