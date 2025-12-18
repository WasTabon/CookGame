using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Events;

public class ProbabilityKitchenSetup : EditorWindow
{
    [MenuItem("Probability Kitchen/Setup Iteration 1 - Order System")]
    static void SetupIteration1()
    {
        Debug.Log("========================================");
        Debug.Log("[SETUP] Starting Iteration 1 Setup...");
        Debug.Log("========================================");
        
        SetupCanvas();
        GameObject gameManagerObj = SetupGameManager();
        GameObject menuPanel = SetupMenuPanel();
        GameObject orderPanel = SetupOrderPanel();
        SetupReferences(gameManagerObj, menuPanel, orderPanel);
        CreateTestRecipes();
        
        Debug.Log("========================================");
        Debug.Log("[SETUP] ✅ Iteration 1 Setup Complete!");
        Debug.Log("[SETUP] Press Play to test: Get Order → See Order Details → Accept");
        Debug.Log("========================================");
    }
    
    static void SetupCanvas()
    {
        Debug.Log("[SETUP] Setting up Canvas...");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        
        if (canvas == null)
        {
            Debug.LogError("[SETUP] ❌ Canvas not found! Create a Canvas first.");
            return;
        }
        
        Debug.Log("[SETUP] ✅ Canvas found");
        
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(2340, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        
        Debug.Log("[SETUP] Canvas configured: 2340x1080, Match 0.5");
        
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            canvas.gameObject.AddComponent<GraphicRaycaster>();
            Debug.Log("[SETUP] GraphicRaycaster added");
        }
        
        Transform existingSafeArea = canvas.transform.Find("SafeArea");
        if (existingSafeArea != null)
        {
            Debug.Log("[SETUP] Destroying existing SafeArea");
            DestroyImmediate(existingSafeArea.gameObject);
        }
        
        GameObject safeAreaObj = new GameObject("SafeArea");
        safeAreaObj.transform.SetParent(canvas.transform, false);
        
        RectTransform safeRect = safeAreaObj.AddComponent<RectTransform>();
        safeRect.anchorMin = Vector2.zero;
        safeRect.anchorMax = Vector2.one;
        safeRect.sizeDelta = Vector2.zero;
        
        safeAreaObj.AddComponent<SafeAreaAdapter>();
        
        Debug.Log("[SETUP] ✓ Canvas configured with Safe Area");
    }
    
    static GameObject SetupGameManager()
    {
        Debug.Log("[SETUP] Setting up GameManager...");
        
        GameObject existing = GameObject.Find("GameManager");
        if (existing != null)
        {
            Debug.Log("[SETUP] Destroying existing GameManager");
            DestroyImmediate(existing);
        }
        
        GameObject gmObj = new GameObject("GameManager");
        gmObj.AddComponent<GameManager>();
        gmObj.AddComponent<OrderManager>();
        gmObj.AddComponent<UIManager>();
        
        Debug.Log("[SETUP] ✓ GameManager created with OrderManager and UIManager");
        return gmObj;
    }
    
    static GameObject SetupMenuPanel()
    {
        Debug.Log("[SETUP] Setting up MenuPanel...");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        Transform safeArea = canvas.transform.Find("SafeArea");
        
        if (safeArea == null)
        {
            Debug.LogError("[SETUP] ❌ SafeArea not found!");
            return null;
        }
        
        GameObject existing = safeArea.Find("MenuPanel")?.gameObject;
        if (existing != null)
        {
            Debug.Log("[SETUP] Destroying existing MenuPanel");
            DestroyImmediate(existing);
        }
        
        GameObject panel = new GameObject("MenuPanel");
        panel.transform.SetParent(safeArea, false);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        panel.AddComponent<CanvasGroup>();
        MenuPanel menuScript = panel.AddComponent<MenuPanel>();
        
        GameObject buttonObj = new GameObject("GetOrderButton");
        buttonObj.transform.SetParent(panel.transform, false);
        
        RectTransform btnRect = buttonObj.AddComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(400, 120);
        btnRect.anchoredPosition = Vector2.zero;
        
        Image btnImage = buttonObj.AddComponent<Image>();
        btnImage.color = new Color(0.2f, 0.6f, 1f);
        
        Button btn = buttonObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = "GET ORDER";
        text.fontSize = 36;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        
        UnityAction action = new UnityAction(menuScript.OnGetOrderButtonPressed);
        btn.onClick.AddListener(action);
        
        EditorUtility.SetDirty(buttonObj);
        EditorUtility.SetDirty(panel);
        
        Debug.Log("[SETUP] ✓ MenuPanel created with GET ORDER button");
        Debug.Log("[SETUP]   Button listener connected to MenuPanel.OnGetOrderButtonPressed()");
        
        return panel;
    }
    
    static GameObject SetupOrderPanel()
    {
        Debug.Log("[SETUP] Setting up OrderPanel...");
        
        Canvas canvas = FindObjectOfType<Canvas>();
        Transform safeArea = canvas.transform.Find("SafeArea");
        
        if (safeArea == null)
        {
            Debug.LogError("[SETUP] ❌ SafeArea not found!");
            return null;
        }
        
        GameObject existing = safeArea.Find("OrderPanel")?.gameObject;
        if (existing != null)
        {
            Debug.Log("[SETUP] Destroying existing OrderPanel");
            DestroyImmediate(existing);
        }
        
        GameObject panel = new GameObject("OrderPanel");
        panel.transform.SetParent(safeArea, false);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        
        panel.AddComponent<CanvasGroup>();
        OrderPanel orderScript = panel.AddComponent<OrderPanel>();
        
        panel.SetActive(false);
        
        float yPos = 300;
        orderScript.recipeNameText = CreateText(panel.transform, "RecipeName", new Vector2(0, yPos), 48, "Recipe Name");
        
        yPos -= 100;
        orderScript.tasteTargetText = CreateText(panel.transform, "TasteTarget", new Vector2(0, yPos), 32, "Taste: 0-0");
        
        yPos -= 80;
        orderScript.stabilityTargetText = CreateText(panel.transform, "StabilityTarget", new Vector2(0, yPos), 32, "Stability: 0-0");
        
        yPos -= 80;
        orderScript.magicTargetText = CreateText(panel.transform, "MagicTarget", new Vector2(0, yPos), 32, "Magic: 0-0");
        
        yPos -= 80;
        orderScript.turnsText = CreateText(panel.transform, "Turns", new Vector2(0, yPos), 32, "Turns: 0");
        
        GameObject acceptBtn = CreateButton(panel.transform, "AcceptButton", new Vector2(0, -300), "ACCEPT ORDER");
        Button btn = acceptBtn.GetComponent<Button>();
        
        UnityAction action = new UnityAction(orderScript.OnAcceptOrderButtonPressed);
        btn.onClick.AddListener(action);
        
        EditorUtility.SetDirty(acceptBtn);
        EditorUtility.SetDirty(panel);
        
        Debug.Log("[SETUP] ✓ OrderPanel created with recipe info texts and ACCEPT button");
        Debug.Log("[SETUP]   Button listener connected to OrderPanel.OnAcceptOrderButtonPressed()");
        
        return panel;
    }
    
    static TextMeshProUGUI CreateText(Transform parent, string name, Vector2 pos, float fontSize, string text)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(800, 80);
        rect.anchoredPosition = pos;
        
        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        return tmp;
    }
    
    static GameObject CreateButton(Transform parent, string name, Vector2 pos, string buttonText)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(400, 120);
        btnRect.anchoredPosition = pos;
        
        Image img = btnObj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.8f, 0.3f);
        
        btnObj.AddComponent<Button>();
        
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = buttonText;
        text.fontSize = 36;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        
        return btnObj;
    }
    
    static void SetupReferences(GameObject gameManager, GameObject menuPanel, GameObject orderPanel)
    {
        Debug.Log("[SETUP] Setting up references...");
        
        if (gameManager == null)
        {
            Debug.LogError("[SETUP] ❌ gameManager is NULL!");
            return;
        }
        
        if (menuPanel == null)
        {
            Debug.LogError("[SETUP] ❌ menuPanel is NULL!");
            return;
        }
        
        if (orderPanel == null)
        {
            Debug.LogError("[SETUP] ❌ orderPanel is NULL!");
            return;
        }
        
        GameManager gm = gameManager.GetComponent<GameManager>();
        gm.orderManager = gameManager.GetComponent<OrderManager>();
        gm.uiManager = gameManager.GetComponent<UIManager>();
        
        Debug.Log("[SETUP]   GameManager.orderManager = " + (gm.orderManager != null ? "✓" : "❌"));
        Debug.Log("[SETUP]   GameManager.uiManager = " + (gm.uiManager != null ? "✓" : "❌"));
        
        UIManager uiManager = gm.uiManager;
        uiManager.menuPanel = menuPanel.GetComponent<CanvasGroup>();
        uiManager.orderPanel = orderPanel.GetComponent<CanvasGroup>();
        uiManager.orderPanelScript = orderPanel.GetComponent<OrderPanel>();
        
        Debug.Log("[SETUP]   UIManager.menuPanel = " + (uiManager.menuPanel != null ? "✓" : "❌"));
        Debug.Log("[SETUP]   UIManager.orderPanel = " + (uiManager.orderPanel != null ? "✓" : "❌"));
        Debug.Log("[SETUP]   UIManager.orderPanelScript = " + (uiManager.orderPanelScript != null ? "✓" : "❌"));
        
        EditorUtility.SetDirty(gameManager);
        Debug.Log("[SETUP] ✓ All references linked");
    }
    
    static void CreateTestRecipes()
    {
        Debug.Log("[SETUP] Creating test recipes...");
        
        string folderPath = "Assets/ScriptableObjects/Recipes";
        
        if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
        {
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
            Debug.Log("[SETUP] Created folder: Assets/ScriptableObjects");
        }
        
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Recipes");
            Debug.Log("[SETUP] Created folder: Assets/ScriptableObjects/Recipes");
        }
        
        CreateRecipe(folderPath, "Easy Salad", 20, 35, 40, 55, 10, 25, 3, RecipeData.Difficulty.Easy);
        CreateRecipe(folderPath, "Medium Soup", 30, 40, 60, 75, 20, 30, 4, RecipeData.Difficulty.Medium);
        CreateRecipe(folderPath, "Hard Steak", 80, 90, 85, 92, 70, 78, 6, RecipeData.Difficulty.Hard);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        LoadRecipesToOrderManager();
        
        Debug.Log("[SETUP] ✓ 3 test recipes created in Assets/ScriptableObjects/Recipes/");
    }
    
    static void CreateRecipe(string folderPath, string name, float tMin, float tMax, float sMin, float sMax, float mMin, float mMax, int turns, RecipeData.Difficulty difficulty)
    {
        string assetPath = $"{folderPath}/{name}.asset";
        
        RecipeData existingRecipe = AssetDatabase.LoadAssetAtPath<RecipeData>(assetPath);
        if (existingRecipe != null)
        {
            Debug.Log($"[SETUP] Recipe '{name}' already exists, skipping");
            return;
        }
        
        RecipeData recipe = ScriptableObject.CreateInstance<RecipeData>();
        recipe.recipeName = name;
        recipe.tasteMin = tMin;
        recipe.tasteMax = tMax;
        recipe.stabilityMin = sMin;
        recipe.stabilityMax = sMax;
        recipe.magicMin = mMin;
        recipe.magicMax = mMax;
        recipe.totalTurns = turns;
        recipe.difficulty = difficulty;
        
        AssetDatabase.CreateAsset(recipe, assetPath);
        Debug.Log($"[SETUP] Created recipe: {name}");
    }
    
    static void LoadRecipesToOrderManager()
    {
        Debug.Log("[SETUP] Loading recipes into OrderManager...");
        
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager == null)
        {
            Debug.LogError("[SETUP] ❌ GameManager not found!");
            return;
        }
        
        OrderManager orderManager = gameManager.GetComponent<OrderManager>();
        if (orderManager == null)
        {
            Debug.LogError("[SETUP] ❌ OrderManager component not found!");
            return;
        }
        
        string[] guids = AssetDatabase.FindAssets("t:RecipeData", new[] { "Assets/ScriptableObjects/Recipes" });
        
        orderManager.allRecipes.Clear();
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RecipeData recipe = AssetDatabase.LoadAssetAtPath<RecipeData>(path);
            if (recipe != null)
            {
                orderManager.allRecipes.Add(recipe);
                Debug.Log($"[SETUP]   Loaded recipe: {recipe.recipeName}");
            }
        }
        
        EditorUtility.SetDirty(gameManager);
        Debug.Log($"[SETUP] ✓ Loaded {orderManager.allRecipes.Count} recipes into OrderManager");
    }
}
