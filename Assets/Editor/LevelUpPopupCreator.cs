using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class LevelUpPopupCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create LevelUpPopup UI")]
    public static void CreatePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        GameObject panel = CreatePanel(canvas.transform, "LevelUpPopup", new Color(0, 0, 0, 0.85f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject popup = CreatePanel(panel.transform, "PopupPanel", new Color(0.1f, 0.1f, 0.15f, 1f));
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchorMin = new Vector2(0.5f, 0.5f);
        popupRect.anchorMax = new Vector2(0.5f, 0.5f);
        popupRect.sizeDelta = new Vector2(500, 400);
        popupRect.anchoredPosition = Vector2.zero;

        GameObject border = CreatePanel(popup.transform, "Border", new Color(1f, 0.84f, 0f, 1f));
        RectTransform borderRect = border.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = new Vector2(-4, -4);
        borderRect.offsetMax = new Vector2(4, 4);
        border.transform.SetAsFirstSibling();

        GameObject inner = CreatePanel(popup.transform, "InnerPanel", new Color(0.12f, 0.12f, 0.18f, 1f));
        RectTransform innerRect = inner.GetComponent<RectTransform>();
        innerRect.anchorMin = Vector2.zero;
        innerRect.anchorMax = Vector2.one;
        innerRect.offsetMin = new Vector2(4, 4);
        innerRect.offsetMax = new Vector2(-4, -4);

        CreateText(inner.transform, "CongratsText", "LEVEL UP!", 32, 130, FontStyles.Bold, new Color(1f, 0.84f, 0f, 1f));

        CreateText(inner.transform, "LevelText", "LEVEL 5", 56, 60, FontStyles.Bold, Color.white);

        GameObject starsRow = new GameObject("Stars");
        starsRow.transform.SetParent(inner.transform, false);
        RectTransform starsRect = starsRow.AddComponent<RectTransform>();
        starsRect.anchorMin = new Vector2(0.5f, 0.5f);
        starsRect.anchorMax = new Vector2(0.5f, 0.5f);
        starsRect.sizeDelta = new Vector2(200, 40);
        starsRect.anchoredPosition = new Vector2(0, 0);

        TextMeshProUGUI starsTmp = starsRow.AddComponent<TextMeshProUGUI>();
        starsTmp.text = "★ ★ ★";
        starsTmp.fontSize = 36;
        starsTmp.alignment = TextAlignmentOptions.Center;
        starsTmp.color = new Color(1f, 0.84f, 0f, 1f);

        CreateText(inner.transform, "UnlocksText", "NEW RECIPES UNLOCKED:\n<color=#7FFF7F>Magic Stew</color>\n<color=#7FFF7F>Fire Soup</color>", 20, -60, FontStyles.Normal, Color.white);

        CreateButton(inner.transform, "ContinueButton", "CONTINUE", new Vector2(0, -140), new Vector2(200, 50), new Color(0.2f, 0.6f, 0.2f, 1f));

        panel.SetActive(false);

        Selection.activeGameObject = panel;
        Debug.Log("LevelUpPopup UI created!");
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

    static GameObject CreateText(Transform parent, string name, string text, int fontSize, float yPos, FontStyles style, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(450, 100);
        rect.anchoredPosition = new Vector2(0, yPos);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.richText = true;

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

        Button button = buttonObj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 22;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return buttonObj;
    }
}
