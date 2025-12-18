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
    
    void OnEnable()
    {
        Debug.Log($"[RecipeData] Recipe loaded: {recipeName}");
    }
}
