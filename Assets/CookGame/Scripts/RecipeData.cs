using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Probability Kitchen/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    
    [Header("Unlock Settings")]
    public int unlockLevel = 1;
    
    [Header("Taste Target")]
    public float tasteMin;
    public float tasteMax;
    
    [Header("Stability Target")]
    public float stabilityMin;
    public float stabilityMax;
    
    [Header("Magic Target")]
    public float magicMin;
    public float magicMax;
    
    [Header("Game Settings")]
    public int totalTurns = 5;
    
    public enum Difficulty { Easy, Medium, Hard, Elite }
    public Difficulty difficulty;
    
    [Header("Reward")]
    public int baseReward = 100;
    
    [Header("Visual")]
    public Sprite icon;
    [TextArea(2, 4)]
    public string description;
    
    void OnEnable()
    {
        Debug.Log($"[RecipeData] Recipe loaded: {recipeName}");
        Debug.Log($"[RecipeData]   Unlock Level: {unlockLevel}");
        Debug.Log($"[RecipeData]   Taste: {tasteMin}-{tasteMax}");
        Debug.Log($"[RecipeData]   Stability: {stabilityMin}-{stabilityMax}");
        Debug.Log($"[RecipeData]   Magic: {magicMin}-{magicMax}");
        Debug.Log($"[RecipeData]   Turns: {totalTurns}, Difficulty: {difficulty}");
        Debug.Log($"[RecipeData]   Base Reward: {baseReward}");
    }
    
    public bool IsValueInTasteRange(float value)
    {
        return value >= tasteMin && value <= tasteMax;
    }
    
    public bool IsValueInStabilityRange(float value)
    {
        return value >= stabilityMin && value <= stabilityMax;
    }
    
    public bool IsValueInMagicRange(float value)
    {
        return value >= magicMin && value <= magicMax;
    }
    
    public int GetRewardByDifficulty()
    {
        return difficulty switch
        {
            Difficulty.Easy => 50,
            Difficulty.Medium => 100,
            Difficulty.Hard => 200,
            Difficulty.Elite => 500,
            _ => 100
        };
    }
    
    public int GetXPReward()
    {
        return difficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 20,
            Difficulty.Hard => 35,
            Difficulty.Elite => 50,
            _ => 10
        };
    }
}
