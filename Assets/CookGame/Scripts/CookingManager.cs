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
    
    [Header("Fire Boost")]
    public FireBoostController fireBoostController;
    
    public RecipeData currentRecipe { get; private set; }
    public int turnsRemaining { get; private set; }
    public bool isGameOver { get; private set; }
    
    private IngredientInstance[] currentIngredients = new IngredientInstance[3];
    private bool waitingForSelection = false;
    
    void Awake()
    {
        Debug.Log("[CookingManager] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[CookingManager] Start() called");
        ValidateReferences();
        SetupFireBoost();
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
            Debug.LogWarning("[CookingManager] ⚠️ FireBoostController is NULL - Fire Boost disabled");
        else
            Debug.Log("[CookingManager] ✅ FireBoostController found");
    }
    
    void SetupFireBoost()
    {
        if (fireBoostController != null)
        {
            fireBoostController.OnBoostTick += OnFireBoostTick;
            fireBoostController.OnBoostEnded += OnFireBoostEnded;
            Debug.Log("[CookingManager] ✅ Fire Boost events subscribed");
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
    
    public void StartCooking(RecipeData recipe)
    {
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] Starting cooking: {recipe.recipeName}");
        Debug.Log($"[CookingManager] Targets - Taste: {recipe.tasteMin}-{recipe.tasteMax}");
        Debug.Log($"[CookingManager] Targets - Stability: {recipe.stabilityMin}-{recipe.stabilityMax}");
        Debug.Log($"[CookingManager] Targets - Magic: {recipe.magicMin}-{recipe.magicMax}");
        Debug.Log($"[CookingManager] Turns: {recipe.totalTurns}");
        Debug.Log($"[CookingManager] ========================================");
        
        currentRecipe = recipe;
        turnsRemaining = recipe.totalTurns;
        isGameOver = false;
        
        InitializeMeters();
        
        if (fireBoostController != null)
        {
            fireBoostController.EnableBoost();
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
        
        CheckForJackpot();
        
        DisplayIngredients();
        waitingForSelection = true;
    }
    
    void CheckForJackpot()
    {
        if (currentIngredients[0].data == currentIngredients[1].data &&
            currentIngredients[1].data == currentIngredients[2].data)
        {
            Debug.Log($"[CookingManager] 🎰 JACKPOT! Three {currentIngredients[0].data.ingredientName}!");
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
        
        waitingForSelection = false;
        
        if (fireBoostController != null)
        {
            fireBoostController.DisableBoost();
        }
        
        IngredientInstance selected = currentIngredients[slotIndex];
        Debug.Log($"[CookingManager] ========================================");
        Debug.Log($"[CookingManager] 🍳 Selected: {selected.data.ingredientName}");
        Debug.Log($"[CookingManager] Applying: T+{selected.tasteEffect:F1} S+{selected.stabilityEffect:F1} M+{selected.magicEffect:F1}");
        
        HideUnselectedSlots(slotIndex);
        
        ApplyIngredient(selected);
    }
    
    void HideUnselectedSlots(int selectedIndex)
    {
        Debug.Log($"[CookingManager] Hiding unselected slots (selected: {selectedIndex})");
        
        if (selectedIndex != 0) slot1.Hide();
        if (selectedIndex != 1) slot2.Hide();
        if (selectedIndex != 2) slot3.Hide();
    }
    
    void ApplyIngredient(IngredientInstance ingredient)
    {
        Debug.Log("[CookingManager] Applying ingredient effects...");
        
        float prevTaste = tasteMeter.CurrentValue;
        float prevStability = stabilityMeter.CurrentValue;
        float prevMagic = magicMeter.CurrentValue;
        
        tasteMeter.AddValue(ingredient.tasteEffect);
        stabilityMeter.AddValue(ingredient.stabilityEffect);
        magicMeter.AddValue(ingredient.magicEffect);
        
        Debug.Log($"[CookingManager] Taste: {prevTaste:F1} → {tasteMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] Stability: {prevStability:F1} → {stabilityMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] Magic: {prevMagic:F1} → {magicMeter.CurrentValue:F1}");
        Debug.Log($"[CookingManager] ========================================");
        
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
            CheckVictoryCondition();
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
            Debug.Log("[CookingManager] ❌ OVERFLOW DETECTED!");
            if (tasteOverflow) Debug.Log($"[CookingManager]   Taste: {tasteMeter.CurrentValue:F1} > {currentRecipe.tasteMax}");
            if (stabilityOverflow) Debug.Log($"[CookingManager]   Stability: {stabilityMeter.CurrentValue:F1} > {currentRecipe.stabilityMax}");
            if (magicOverflow) Debug.Log($"[CookingManager]   Magic: {magicMeter.CurrentValue:F1} > {currentRecipe.magicMax}");
            
            EndGame(false);
            return true;
        }
        
        return false;
    }
    
    void CheckVictoryCondition()
    {
        Debug.Log("[CookingManager] Checking victory condition...");
        
        bool tasteInRange = tasteMeter.CurrentValue >= currentRecipe.tasteMin && 
                           tasteMeter.CurrentValue <= currentRecipe.tasteMax;
        bool stabilityInRange = stabilityMeter.CurrentValue >= currentRecipe.stabilityMin && 
                               stabilityMeter.CurrentValue <= currentRecipe.stabilityMax;
        bool magicInRange = magicMeter.CurrentValue >= currentRecipe.magicMin && 
                           magicMeter.CurrentValue <= currentRecipe.magicMax;
        
        Debug.Log($"[CookingManager] Taste in range: {tasteInRange} ({tasteMeter.CurrentValue:F1} in {currentRecipe.tasteMin}-{currentRecipe.tasteMax})");
        Debug.Log($"[CookingManager] Stability in range: {stabilityInRange} ({stabilityMeter.CurrentValue:F1} in {currentRecipe.stabilityMin}-{currentRecipe.stabilityMax})");
        Debug.Log($"[CookingManager] Magic in range: {magicInRange} ({magicMeter.CurrentValue:F1} in {currentRecipe.magicMin}-{currentRecipe.magicMax})");
        
        bool victory = tasteInRange && stabilityInRange && magicInRange;
        EndGame(victory);
    }
    
    void EndGame(bool victory)
    {
        isGameOver = true;
        waitingForSelection = false;
        
        if (fireBoostController != null)
        {
            fireBoostController.DisableBoost();
        }
        
        if (victory)
        {
            Debug.Log("[CookingManager] 🎉🎉🎉 VICTORY! 🎉🎉🎉");
        }
        else
        {
            Debug.Log("[CookingManager] 💀💀💀 GAME OVER 💀💀💀");
        }
        
        GameManager.Instance.uiManager.ShowResultPanel(victory);
    }
    
    void OnDestroy()
    {
        if (fireBoostController != null)
        {
            fireBoostController.OnBoostTick -= OnFireBoostTick;
            fireBoostController.OnBoostEnded -= OnFireBoostEnded;
        }
    }
}
