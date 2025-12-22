using UnityEngine;

[System.Serializable]
public class RewardResult
{
    public int baseReward;
    public float meterPercentage;
    public int metersInRange;
    public int turnsBonus;
    public int turnsRemaining;
    public int finalReward;
    public string grade;
    
    public bool tasteInRange;
    public bool stabilityInRange;
    public bool magicInRange;
}

public class RewardCalculator : MonoBehaviour
{
    [Header("Reward Settings")]
    public float perfectMultiplier = 1.0f;
    public float goodMultiplier = 0.6f;
    public float okayMultiplier = 0.3f;
    public float failedMultiplier = 0.1f;
    
    [Header("Turn Bonus")]
    public float bonusPerTurn = 0.05f;
    public float maxTurnBonus = 0.25f;
    
    void Awake()
    {
        Debug.Log("[RewardCalculator] Awake() called");
    }
    
    public RewardResult CalculateReward(
        RecipeData recipe,
        float tasteValue, float stabilityValue, float magicValue,
        int turnsRemaining, int totalTurns)
    {
        Debug.Log("[RewardCalculator] ========================================");
        Debug.Log("[RewardCalculator] Calculating reward...");
        
        RewardResult result = new RewardResult();
        result.baseReward = recipe.baseReward;
        result.turnsRemaining = turnsRemaining;
        
        result.tasteInRange = tasteValue >= recipe.tasteMin && tasteValue <= recipe.tasteMax;
        result.stabilityInRange = stabilityValue >= recipe.stabilityMin && stabilityValue <= recipe.stabilityMax;
        result.magicInRange = magicValue >= recipe.magicMin && magicValue <= recipe.magicMax;
        
        result.metersInRange = 0;
        if (result.tasteInRange) result.metersInRange++;
        if (result.stabilityInRange) result.metersInRange++;
        if (result.magicInRange) result.metersInRange++;
        
        Debug.Log($"[RewardCalculator] Taste in range: {result.tasteInRange} ({tasteValue:F1} in {recipe.tasteMin}-{recipe.tasteMax})");
        Debug.Log($"[RewardCalculator] Stability in range: {result.stabilityInRange} ({stabilityValue:F1} in {recipe.stabilityMin}-{recipe.stabilityMax})");
        Debug.Log($"[RewardCalculator] Magic in range: {result.magicInRange} ({magicValue:F1} in {recipe.magicMin}-{recipe.magicMax})");
        Debug.Log($"[RewardCalculator] Meters in range: {result.metersInRange}/3");
        
        result.meterPercentage = result.metersInRange switch
        {
            3 => perfectMultiplier,
            2 => goodMultiplier,
            1 => okayMultiplier,
            _ => failedMultiplier
        };
        
        result.grade = result.metersInRange switch
        {
            3 => "PERFECT",
            2 => "GOOD",
            1 => "OKAY",
            _ => "FAILED"
        };
        
        Debug.Log($"[RewardCalculator] Grade: {result.grade} ({result.meterPercentage * 100}%)");
        
        float turnBonusPercent = Mathf.Min(turnsRemaining * bonusPerTurn, maxTurnBonus);
        result.turnsBonus = Mathf.RoundToInt(result.baseReward * turnBonusPercent);
        
        Debug.Log($"[RewardCalculator] Turns remaining: {turnsRemaining}");
        Debug.Log($"[RewardCalculator] Turn bonus: +{turnBonusPercent * 100}% = +{result.turnsBonus} coins");
        
        int meterReward = Mathf.RoundToInt(result.baseReward * result.meterPercentage);
        result.finalReward = meterReward + result.turnsBonus;
        
        Debug.Log($"[RewardCalculator] Base reward: {result.baseReward}");
        Debug.Log($"[RewardCalculator] After meter multiplier: {meterReward}");
        Debug.Log($"[RewardCalculator] Final reward: {result.finalReward}");
        Debug.Log("[RewardCalculator] ========================================");
        
        return result;
    }
    
    public int GetCurrentPotentialReward(
        RecipeData recipe,
        float tasteValue, float stabilityValue, float magicValue,
        int turnsRemaining)
    {
        bool tasteInRange = tasteValue >= recipe.tasteMin && tasteValue <= recipe.tasteMax;
        bool stabilityInRange = stabilityValue >= recipe.stabilityMin && stabilityValue <= recipe.stabilityMax;
        bool magicInRange = magicValue >= recipe.magicMin && magicValue <= recipe.magicMax;
        
        int metersInRange = 0;
        if (tasteInRange) metersInRange++;
        if (stabilityInRange) metersInRange++;
        if (magicInRange) metersInRange++;
        
        float meterPercent = metersInRange switch
        {
            3 => perfectMultiplier,
            2 => goodMultiplier,
            1 => okayMultiplier,
            _ => failedMultiplier
        };
        
        float turnBonusPercent = Mathf.Min(turnsRemaining * bonusPerTurn, maxTurnBonus);
        
        int meterReward = Mathf.RoundToInt(recipe.baseReward * meterPercent);
        int turnBonus = Mathf.RoundToInt(recipe.baseReward * turnBonusPercent);
        
        return meterReward + turnBonus;
    }
}
