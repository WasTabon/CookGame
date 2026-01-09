using UnityEngine;
using UnityEditor;
using System.IO;

public class IngredientsRecipesCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create All Ingredients & Recipes")]
    static void CreateAll()
    {
        CreateIngredients();
        CreateRecipes();
        UpdateExistingWithIcons();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Creator] ✅ All Ingredients & Recipes created!");
    }

    static void CreateIngredients()
    {
        string path = "Assets/CookGame/ScriptableObjects/Ingredients";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        CreateIngredient(path, "SaltCrystal", "Salt Crystal", IngredientData.Rarity.Common, 
            5f, 12f, 5f, 12f, 5f, 12f, "salt", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "ButterPat", "Butter Pat", IngredientData.Rarity.Common,
            6f, 13f, 6f, 13f, 6f, 13f, "butter", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "GarlicClove", "Garlic Clove", IngredientData.Rarity.Common,
            7f, 14f, 7f, 14f, 7f, 14f, "garlic", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "FreshHerbs", "Fresh Herbs", IngredientData.Rarity.Common,
            5f, 13f, 5f, 13f, 5f, 13f, "fresh_herbs", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "CheeseWedge", "Cheese Wedge", IngredientData.Rarity.Common,
            6f, 14f, 6f, 14f, 6f, 14f, "cheese", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "OnionBulb", "Onion Bulb", IngredientData.Rarity.Common,
            5f, 12f, 5f, 12f, 5f, 12f, "onion", new Color(0.8f, 0.8f, 0.8f, 1f));

        CreateIngredient(path, "TruffleMushroom", "Truffle Mushroom", IngredientData.Rarity.Rare,
            10f, 20f, 10f, 20f, 10f, 20f, "truffle", new Color(0.4f, 0.6f, 1f, 1f));

        CreateIngredient(path, "MagicSpice", "Magic Spice", IngredientData.Rarity.Rare,
            12f, 22f, 12f, 22f, 12f, 22f, "magic", new Color(0.4f, 0.6f, 1f, 1f));

        CreateIngredient(path, "PremiumWine", "Premium Wine", IngredientData.Rarity.Rare,
            11f, 21f, 11f, 21f, 11f, 21f, "wine", new Color(0.4f, 0.6f, 1f, 1f));

        CreateIngredient(path, "WagyuBeef", "Wagyu Beef", IngredientData.Rarity.Epic,
            20f, 35f, 20f, 35f, 20f, 35f, "wagyu_beef", new Color(1f, 0.75f, 0.2f, 1f));

        Debug.Log("[Creator] ✅ Created 10 Ingredients");
    }

    static void CreateIngredient(string path, string fileName, string name, IngredientData.Rarity rarity,
        float tasteMin, float tasteMax, float stabMin, float stabMax, float magMin, float magMax,
        string iconName, Color color)
    {
        string fullPath = $"{path}/{fileName}.asset";
        
        IngredientData existing = AssetDatabase.LoadAssetAtPath<IngredientData>(fullPath);
        if (existing != null)
        {
            Debug.Log($"[Creator] ⏭️ Ingredient already exists: {name}");
            return;
        }

        IngredientData ingredient = ScriptableObject.CreateInstance<IngredientData>();
        ingredient.ingredientName = name;
        ingredient.rarity = rarity;
        ingredient.tasteMin = tasteMin;
        ingredient.tasteMax = tasteMax;
        ingredient.stabilityMin = stabMin;
        ingredient.stabilityMax = stabMax;
        ingredient.magicMin = magMin;
        ingredient.magicMax = magMax;
        ingredient.rarityColor = color;

        Sprite icon = LoadIcon(iconName);
        if (icon != null)
        {
            ingredient.icon = icon;
        }

        AssetDatabase.CreateAsset(ingredient, fullPath);
        Debug.Log($"[Creator] ✅ Created: {name}");
    }

    static void CreateRecipes()
    {
        string path = "Assets/CookGame/ScriptableObjects/Recipes";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        CreateRecipe(path, "ScrambledEggs", "Scrambled Eggs", 1, RecipeData.Difficulty.Easy,
            40f, 60f, 40f, 60f, 40f, 60f, 5, 50, "scrambled_eggs");

        CreateRecipe(path, "GardenSalad", "Garden Salad", 2, RecipeData.Difficulty.Easy,
            35f, 65f, 35f, 65f, 35f, 65f, 5, 60, "salad");

        CreateRecipe(path, "ButterToast", "Butter Toast", 3, RecipeData.Difficulty.Easy,
            45f, 65f, 40f, 60f, 40f, 60f, 5, 70, "butter_toast");

        CreateRecipe(path, "PastaCarbonara", "Pasta Carbonara", 5, RecipeData.Difficulty.Medium,
            50f, 65f, 45f, 60f, 50f, 70f, 6, 100, "pasta_carbonara");

        CreateRecipe(path, "MushroomRisotto", "Mushroom Risotto", 8, RecipeData.Difficulty.Medium,
            55f, 70f, 50f, 65f, 45f, 60f, 6, 120, "mushroom_risotto");

        CreateRecipe(path, "ChefsSpecial", "Chef's Special", 12, RecipeData.Difficulty.Medium,
            60f, 75f, 50f, 65f, 55f, 70f, 6, 150, "taste");

        CreateRecipe(path, "GrilledChicken", "Grilled Chicken", 15, RecipeData.Difficulty.Hard,
            65f, 75f, 60f, 70f, 70f, 85f, 7, 200, "grilled_chicken");

        CreateRecipe(path, "BerryDessert", "Berry Dessert", 20, RecipeData.Difficulty.Hard,
            70f, 80f, 65f, 75f, 65f, 80f, 7, 250, "molecular_sphere");

        CreateRecipe(path, "SeafoodPlatter", "Seafood Platter", 25, RecipeData.Difficulty.Hard,
            75f, 85f, 70f, 80f, 75f, 90f, 7, 300, "lobster");

        CreateRecipe(path, "BeefWellington", "Beef Wellington", 35, RecipeData.Difficulty.Elite,
            85f, 95f, 85f, 95f, 85f, 95f, 8, 500, "beef_wellington");

        Debug.Log("[Creator] ✅ Created 10 Recipes");
    }

    static void CreateRecipe(string path, string fileName, string name, int unlockLevel, 
        RecipeData.Difficulty difficulty, float tasteMin, float tasteMax, float stabMin, 
        float stabMax, float magMin, float magMax, int turns, int reward, string iconName)
    {
        string fullPath = $"{path}/{fileName}.asset";
        
        RecipeData existing = AssetDatabase.LoadAssetAtPath<RecipeData>(fullPath);
        if (existing != null)
        {
            Debug.Log($"[Creator] ⏭️ Recipe already exists: {name}");
            return;
        }

        RecipeData recipe = ScriptableObject.CreateInstance<RecipeData>();
        recipe.recipeName = name;
        recipe.unlockLevel = unlockLevel;
        recipe.difficulty = difficulty;
        recipe.tasteMin = tasteMin;
        recipe.tasteMax = tasteMax;
        recipe.stabilityMin = stabMin;
        recipe.stabilityMax = stabMax;
        recipe.magicMin = magMin;
        recipe.magicMax = magMax;
        recipe.totalTurns = turns;
        recipe.baseReward = reward;

        Sprite icon = LoadIcon(iconName);
        if (icon != null)
        {
            recipe.icon = icon;
        }

        AssetDatabase.CreateAsset(recipe, fullPath);
        Debug.Log($"[Creator] ✅ Created: {name}");
    }

    static void UpdateExistingWithIcons()
    {
        Debug.Log("[Creator] 🔄 Updating existing ScriptableObjects with icons...");

        string ingredientsPath = "Assets/CookGame/ScriptableObjects/Ingredients";
        string recipesPath = "Assets/CookGame/ScriptableObjects/Recipes";

        if (Directory.Exists(ingredientsPath))
        {
            string[] ingredientGuids = AssetDatabase.FindAssets("t:IngredientData", new[] { ingredientsPath });
            foreach (string guid in ingredientGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                IngredientData ingredient = AssetDatabase.LoadAssetAtPath<IngredientData>(path);
                if (ingredient != null && ingredient.icon == null)
                {
                    Sprite icon = TryFindIconForIngredient(ingredient.ingredientName);
                    if (icon != null)
                    {
                        ingredient.icon = icon;
                        EditorUtility.SetDirty(ingredient);
                        Debug.Log($"[Creator] 🖼️ Updated icon for: {ingredient.ingredientName}");
                    }
                }
            }
        }

        if (Directory.Exists(recipesPath))
        {
            string[] recipeGuids = AssetDatabase.FindAssets("t:RecipeData", new[] { recipesPath });
            foreach (string guid in recipeGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                RecipeData recipe = AssetDatabase.LoadAssetAtPath<RecipeData>(path);
                if (recipe != null && recipe.icon == null)
                {
                    Sprite icon = TryFindIconForRecipe(recipe.recipeName);
                    if (icon != null)
                    {
                        recipe.icon = icon;
                        EditorUtility.SetDirty(recipe);
                        Debug.Log($"[Creator] 🖼️ Updated icon for: {recipe.recipeName}");
                    }
                }
            }
        }
    }

    static Sprite TryFindIconForIngredient(string name)
    {
        string lowerName = name.ToLower().Replace(" ", "_");
        
        if (lowerName.Contains("salt")) return LoadIcon("salt");
        if (lowerName.Contains("butter")) return LoadIcon("butter");
        if (lowerName.Contains("garlic")) return LoadIcon("garlic");
        if (lowerName.Contains("herb")) return LoadIcon("fresh_herbs");
        if (lowerName.Contains("cheese")) return LoadIcon("cheese");
        if (lowerName.Contains("onion")) return LoadIcon("onion");
        if (lowerName.Contains("truffle")) return LoadIcon("truffle");
        if (lowerName.Contains("magic")) return LoadIcon("magic");
        if (lowerName.Contains("wine")) return LoadIcon("wine");
        if (lowerName.Contains("wagyu") || lowerName.Contains("beef")) return LoadIcon("wagyu_beef");

        return null;
    }

    static Sprite TryFindIconForRecipe(string name)
    {
        string lowerName = name.ToLower().Replace(" ", "_");

        if (lowerName.Contains("scrambled") || lowerName.Contains("egg")) return LoadIcon("scrambled_eggs");
        if (lowerName.Contains("salad")) return LoadIcon("salad");
        if (lowerName.Contains("toast")) return LoadIcon("butter_toast");
        if (lowerName.Contains("pasta") || lowerName.Contains("carbonara")) return LoadIcon("pasta_carbonara");
        if (lowerName.Contains("risotto")) return LoadIcon("mushroom_risotto");
        if (lowerName.Contains("chef")) return LoadIcon("taste");
        if (lowerName.Contains("chicken") || lowerName.Contains("grilled")) return LoadIcon("grilled_chicken");
        if (lowerName.Contains("berry") || lowerName.Contains("dessert")) return LoadIcon("molecular_sphere");
        if (lowerName.Contains("seafood") || lowerName.Contains("platter")) return LoadIcon("lobster");
        if (lowerName.Contains("wellington") || lowerName.Contains("beef")) return LoadIcon("beef_wellington");

        return null;
    }

    static Sprite LoadIcon(string iconName)
    {
        string iconsPath = "Assets/CookGame/Sprites/New/Icons";
        
        string normalPath = $"{iconsPath}/{iconName}.png";
        Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(normalPath);
        if (icon != null)
        {
            return icon;
        }

        string strokePath = $"{iconsPath}/{iconName}_stroke.png";
        icon = AssetDatabase.LoadAssetAtPath<Sprite>(strokePath);
        if (icon != null)
        {
            Debug.Log($"[Creator] ℹ️ Using stroke version for: {iconName}");
            return icon;
        }

        Debug.LogWarning($"[Creator] ⚠️ Icon not found: {iconName}");
        return null;
    }
}
