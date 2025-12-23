using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsPanelCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create PlayerStatsPanel UI")]
    public static void CreatePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        GameObject panel = CreatePanel(canvas.transform, "PlayerStatsPanel", new Color(0, 0, 0, 0.9f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject container = CreatePanel(panel.transform, "Container", new Color(0.15f, 0.15f, 0.2f, 1f));
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(600, 800);
        containerRect.anchoredPosition = Vector2.zero;

        float yPos = 350;

        CreateText(container.transform, "TitleText", "PLAYER STATS", 36, yPos, FontStyles.Bold);
        yPos -= 80;

        CreateText(container.transform, "LevelText", "Level 1", 28, yPos, FontStyles.Bold);
        yPos -= 50;

        GameObject xpSlider = CreateSlider(container.transform, "XPSlider", yPos);
        yPos -= 30;

        CreateText(container.transform, "XPText", "0 / 100 XP", 18, yPos);
        yPos -= 40;

        CreateText(container.transform, "XPToNextText", "100 XP to next level", 16, yPos, FontStyles.Italic);
        yPos -= 60;

        CreateDivider(container.transform, yPos);
        yPos -= 30;

        CreateText(container.transform, "StatsHeader", "STATISTICS", 24, yPos, FontStyles.Bold);
        yPos -= 50;

        CreateStatRow(container.transform, "OrdersCompleted", "Orders Completed", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "OrdersFailed", "Orders Failed", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "SuccessRate", "Success Rate", "0%", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "PerfectOrders", "Perfect Orders", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "CurrentStreak", "Current Streak", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "HighestStreak", "Highest Streak", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "Jackpots", "Jackpots", "0", yPos);
        yPos -= 40;

        CreateStatRow(container.transform, "TotalXP", "Total XP Earned", "0", yPos);
        yPos -= 60;

        CreateDivider(container.transform, yPos);
        yPos -= 30;

        CreateText(container.transform, "NextUnlockText", "Next: Magic Stew", 20, yPos);
        yPos -= 35;

        CreateText(container.transform, "LevelsUntilUnlockText", "3 levels away", 16, yPos, FontStyles.Italic);
        yPos -= 60;

        CreateButton(container.transform, "CloseButton", "CLOSE", new Vector2(0, yPos), new Vector2(200, 50), new Color(0.3f, 0.6f, 0.3f, 1f));
        CreateButton(container.transform, "ResetButton", "RESET", new Vector2(0, yPos - 60), new Vector2(150, 40), new Color(0.6f, 0.3f, 0.3f, 1f));

        panel.SetActive(false);

        Selection.activeGameObject = panel;
        Debug.Log("PlayerStatsPanel UI created!");
    }

    static GameObject CreatePanel(Transform parent, string name, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        Image image = panel.AddComponent<Image>();
        image.color = color;

        return panel;
    }

    static GameObject CreateText(Transform parent, string name, string text, int fontSize, float yPos, FontStyles style = FontStyles.Normal)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(500, 40);
        rect.anchoredPosition = new Vector2(0, yPos);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return textObj;
    }

    static void CreateStatRow(Transform parent, string name, string label, string value, float yPos)
    {
        GameObject row = new GameObject(name + "Row");
        row.transform.SetParent(parent, false);

        RectTransform rowRect = row.AddComponent<RectTransform>();
        rowRect.anchorMin = new Vector2(0.5f, 0.5f);
        rowRect.anchorMax = new Vector2(0.5f, 0.5f);
        rowRect.sizeDelta = new Vector2(500, 35);
        rowRect.anchoredPosition = new Vector2(0, yPos);

        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(row.transform, false);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0.5f);
        labelRect.anchorMax = new Vector2(0, 0.5f);
        labelRect.pivot = new Vector2(0, 0.5f);
        labelRect.sizeDelta = new Vector2(300, 35);
        labelRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI labelTmp = labelObj.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 20;
        labelTmp.alignment = TextAlignmentOptions.Left;
        labelTmp.color = new Color(0.8f, 0.8f, 0.8f, 1f);

        GameObject valueObj = new GameObject(name + "Text");
        valueObj.transform.SetParent(row.transform, false);
        RectTransform valueRect = valueObj.AddComponent<RectTransform>();
        valueRect.anchorMin = new Vector2(1, 0.5f);
        valueRect.anchorMax = new Vector2(1, 0.5f);
        valueRect.pivot = new Vector2(1, 0.5f);
        valueRect.sizeDelta = new Vector2(150, 35);
        valueRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI valueTmp = valueObj.AddComponent<TextMeshProUGUI>();
        valueTmp.text = value;
        valueTmp.fontSize = 22;
        valueTmp.fontStyle = FontStyles.Bold;
        valueTmp.alignment = TextAlignmentOptions.Right;
        valueTmp.color = Color.white;
    }

    static GameObject CreateSlider(Transform parent, string name, float yPos)
    {
        GameObject sliderObj = new GameObject(name);
        sliderObj.transform.SetParent(parent, false);

        RectTransform sliderRect = sliderObj.AddComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 0.5f);
        sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
        sliderRect.sizeDelta = new Vector2(400, 20);
        sliderRect.anchoredPosition = new Vector2(0, yPos);

        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.35f;
        slider.interactable = false;

        GameObject background = new GameObject("Background");
        background.transform.SetParent(sliderObj.transform, false);
        RectTransform bgRect = background.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = Vector2.zero;
        fillAreaRect.offsetMax = Vector2.zero;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        RectTransform fillRect = fill.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0, 1);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color(0.3f, 0.7f, 1f, 1f);

        slider.fillRect = fillRect;

        return sliderObj;
    }

    static void CreateDivider(Transform parent, float yPos)
    {
        GameObject divider = new GameObject("Divider");
        divider.transform.SetParent(parent, false);

        RectTransform rect = divider.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(450, 2);
        rect.anchoredPosition = new Vector2(0, yPos);

        Image image = divider.AddComponent<Image>();
        image.color = new Color(0.4f, 0.4f, 0.5f, 1f);
    }

    static GameObject CreateButton(Transform parent, string name, string text, Vector2 position, Vector2 size, Color color)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Image image = buttonObj.AddComponent<Image>();
        image.color = color;

        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = new Color(color.r + 0.1f, color.g + 0.1f, color.b + 0.1f, 1f);
        colors.pressedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f, 1f);
        button.colors = colors;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 20;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return buttonObj;
    }
}
