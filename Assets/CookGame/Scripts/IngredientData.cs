using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Probability Kitchen/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    
    public enum Rarity { Common, Rare, Epic }
    public Rarity rarity;
    
    [Header("Effect Ranges")]
    public float tasteMin;
    public float tasteMax;
    
    public float stabilityMin;
    public float stabilityMax;
    
    public float magicMin;
    public float magicMax;
    
    [Header("Visual")]
    public Sprite icon;
    
    public IngredientInstance GenerateInstance()
    {
        Debug.Log($"[IngredientData] Generating instance for: {ingredientName}");
        
        IngredientInstance instance = new IngredientInstance();
        instance.data = this;
        instance.tasteEffect = Random.Range(tasteMin, tasteMax);
        instance.stabilityEffect = Random.Range(stabilityMin, stabilityMax);
        instance.magicEffect = Random.Range(magicMin, magicMax);
        
        Debug.Log($"[IngredientData]   Taste: {instance.tasteEffect:F1}");
        Debug.Log($"[IngredientData]   Stability: {instance.stabilityEffect:F1}");
        Debug.Log($"[IngredientData]   Magic: {instance.magicEffect:F1}");
        
        return instance;
    }
}

[System.Serializable]
public class IngredientInstance
{
    public IngredientData data;
    public float tasteEffect;
    public float stabilityEffect;
    public float magicEffect;
}
