using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Probability Kitchen/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    
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
    
    [Header("Visual")]
    public Sprite icon;
    
    void OnEnable()
    {
        Debug.Log($"[RecipeData] Recipe loaded: {recipeName}");
        Debug.Log($"[RecipeData]   Taste: {tasteMin}-{tasteMax}");
        Debug.Log($"[RecipeData]   Stability: {stabilityMin}-{stabilityMax}");
        Debug.Log($"[RecipeData]   Magic: {magicMin}-{magicMax}");
        Debug.Log($"[RecipeData]   Turns: {totalTurns}, Difficulty: {difficulty}");
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
}
