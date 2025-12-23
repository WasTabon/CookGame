using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class BoosterSelectionPanelCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create BoosterSelectionPanel UI")]
    public static void CreatePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        GameObject panel = CreatePanel(canvas.transform, "BoosterSelectionPanel", new Color(0, 0, 0, 0.85f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject container = CreatePanel(panel.transform, "Container", new Color(0.12f, 0.12f, 0.18f, 1f));
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(500, 450);
        containerRect.anchoredPosition = Vector2.zero;

        float yPos = 180;

        CreateText(container.transform, "TitleText", "⚡ SELECT BOOSTERS", 26, yPos, FontStyles.Bold, Color.white);
        yPos -= 40;

        CreateText(container.transform, "SubtitleText", "Choose boosters for this game", 16, yPos, FontStyles.Normal, new Color(0.7f, 0.7f, 0.7f, 1f));
        yPos -= 50;

        CreateBoosterButton(container.transform, "ExtraTurnButton", "+1 TURN", "ExtraTurnCountText", "ExtraTurnSelected", new Vector2(-110, yPos), new Color(0.2f, 0.5f, 0.7f, 1f));
        CreateBoosterButton(container.transform, "ShieldButton", "🛡️ SHIELD", "ShieldCountText", "ShieldSelected", new Vector2(110, yPos), new Color(0.5f, 0.5f, 0.2f, 1f));
        yPos -= 100;

        CreateBoosterButton(container.transform, "DoubleCoinsButton", "x2 💰", "DoubleCoinsCountText", "DoubleCoinsSelected", new Vector2(-110, yPos), new Color(0.6f, 0.5f, 0.2f, 1f));
        CreateBoosterButton(container.transform, "DoubleXPButton", "x2 XP", "DoubleXPCountText", "DoubleXPSelected", new Vector2(110, yPos), new Color(0.5f, 0.2f, 0.6f, 1f));
        yPos -= 70;

        CreateButton(container.transform, "StartButton", "START COOKING", new Vector2(0, yPos), new Vector2(220, 55), new Color(0.2f, 0.7f, 0.2f, 1f));
        CreateButton(container.transform, "CloseButton", "✕", new Vector2(215, 190), new Vector2(45, 45), new Color(0.5f, 0.2f, 0.2f, 1f));

        panel.SetActive(false);

        Selection.activeGameObject = panel;
        Debug.Log("BoosterSelectionPanel UI created!");
    }

    static void CreateBoosterButton(Transform parent, string buttonName, string label, string countTextName, string selectedName, Vector2 position, Color color)
    {
        GameObject buttonObj = new GameObject(buttonName);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(180, 75);
        rect.anchoredPosition = position;

        Image image = buttonObj.AddComponent<Image>();
        image.color = color;

        buttonObj.AddComponent<Button>();

        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(buttonObj.transform, false);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = new Vector2(1, 0.65f);
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI labelTmp = labelObj.AddComponent<TextMeshProUGUI>();
        labelTmp.text = label;
        labelTmp.fontSize = 18;
        labelTmp.fontStyle = FontStyles.Bold;
        labelTmp.alignment = TextAlignmentOptions.Center;
        labelTmp.color = Color.white;

        GameObject countObj = new GameObject(countTextName);
        countObj.transform.SetParent(buttonObj.transform, false);
        RectTransform countRect = countObj.AddComponent<RectTransform>();
        countRect.anchorMin = new Vector2(0, 0);
        countRect.anchorMax = new Vector2(1, 0.4f);
        countRect.offsetMin = Vector2.zero;
        countRect.offsetMax = Vector2.zero;

        TextMeshProUGUI countTmp = countObj.AddComponent<TextMeshProUGUI>();
        countTmp.text = "x0";
        countTmp.fontSize = 14;
        countTmp.alignment = TextAlignmentOptions.Center;
        countTmp.color = new Color(0.8f, 0.8f, 0.8f, 1f);

        GameObject selectedObj = new GameObject(selectedName);
        selectedObj.transform.SetParent(buttonObj.transform, false);
        RectTransform selectedRect = selectedObj.AddComponent<RectTransform>();
        selectedRect.anchorMin = new Vector2(1, 1);
        selectedRect.anchorMax = new Vector2(1, 1);
        selectedRect.pivot = new Vector2(1, 1);
        selectedRect.sizeDelta = new Vector2(28, 28);
        selectedRect.anchoredPosition = new Vector2(5, 5);

        Image selectedImage = selectedObj.AddComponent<Image>();
        selectedImage.color = new Color(0.2f, 1f, 0.2f, 1f);

        GameObject checkObj = new GameObject("Check");
        checkObj.transform.SetParent(selectedObj.transform, false);
        RectTransform checkRect = checkObj.AddComponent<RectTransform>();
        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.offsetMin = Vector2.zero;
        checkRect.offsetMax = Vector2.zero;

        TextMeshProUGUI checkTmp = checkObj.AddComponent<TextMeshProUGUI>();
        checkTmp.text = "✓";
        checkTmp.fontSize = 18;
        checkTmp.fontStyle = FontStyles.Bold;
        checkTmp.alignment = TextAlignmentOptions.Center;
        checkTmp.color = Color.white;

        selectedObj.SetActive(false);
    }

    static GameObject CreatePanel(Transform parent, string name, Color color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        panel.AddComponent<RectTransform>();
        Image image = panel.AddComponent<Image>();
        image.color = color;
        return panel;
    }

    static GameObject CreateText(Transform parent, string name, string text, int fontSize, float yPos, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 40);
        rect.anchoredPosition = new Vector2(0, yPos);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;

        return textObj;
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

        buttonObj.AddComponent<Button>();

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
