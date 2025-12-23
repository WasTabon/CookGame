using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class GemShopPanelCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create GemShopPanel UI")]
    public static void CreatePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        GameObject panel = CreatePanel(canvas.transform, "GemShopPanel", new Color(0, 0, 0, 0.9f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject container = CreatePanel(panel.transform, "Container", new Color(0.12f, 0.12f, 0.18f, 1f));
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(500, 650);
        containerRect.anchoredPosition = Vector2.zero;

        float yPos = 280;

        CreateText(container.transform, "TitleText", "💎 GEM SHOP", 36, yPos, FontStyles.Bold, Color.white);
        yPos -= 60;

        GameObject currencyRow = new GameObject("CurrencyRow");
        currencyRow.transform.SetParent(container.transform, false);
        RectTransform currencyRect = currencyRow.AddComponent<RectTransform>();
        currencyRect.anchorMin = new Vector2(0.5f, 0.5f);
        currencyRect.anchorMax = new Vector2(0.5f, 0.5f);
        currencyRect.sizeDelta = new Vector2(400, 50);
        currencyRect.anchoredPosition = new Vector2(0, yPos);

        CreateText(currencyRow.transform, "GemsText", "💎 50", 28, 0, FontStyles.Bold, new Color(0.6f, 0.3f, 1f, 1f), TextAlignmentOptions.Left, new Vector2(-100, 0));
        CreateText(currencyRow.transform, "CoinsText", "💰 1000", 28, 0, FontStyles.Bold, new Color(1f, 0.85f, 0.2f, 1f), TextAlignmentOptions.Right, new Vector2(100, 0));
        yPos -= 50;

        CreateDivider(container.transform, yPos);
        yPos -= 30;

        CreateText(container.transform, "BuyGemsLabel", "BUY GEMS", 22, yPos, FontStyles.Bold, new Color(0.8f, 0.8f, 0.8f, 1f));
        yPos -= 50;

        GameObject buyButton = CreateButton(container.transform, "BuyGemsButton", "BUY 50 💎\n$0.99", new Vector2(0, yPos), new Vector2(250, 80), new Color(0.5f, 0.2f, 0.8f, 1f));
        yPos -= 30;

        CreateText(container.transform, "GemPackInfoText", "Get 50 gems instantly!", 16, yPos, FontStyles.Normal, new Color(0.6f, 0.6f, 0.6f, 1f));
        yPos -= 40;

        CreateButton(container.transform, "RestoreButton", "Restore Purchases", new Vector2(0, yPos), new Vector2(200, 40), new Color(0.3f, 0.3f, 0.4f, 1f));
        yPos -= 40;

        CreateDivider(container.transform, yPos);
        yPos -= 30;

        CreateText(container.transform, "ExchangeLabel", "EXCHANGE GEMS → COINS", 22, yPos, FontStyles.Bold, new Color(0.8f, 0.8f, 0.8f, 1f));
        yPos -= 50;

        CreateButton(container.transform, "Exchange1Button", "1 💎 → 50 💰", new Vector2(0, yPos), new Vector2(200, 50), new Color(0.3f, 0.5f, 0.3f, 1f));
        yPos -= 60;

        CreateButton(container.transform, "Exchange5Button", "5 💎 → 250 💰", new Vector2(0, yPos), new Vector2(200, 50), new Color(0.3f, 0.5f, 0.3f, 1f));
        yPos -= 60;

        CreateButton(container.transform, "Exchange10Button", "10 💎 → 500 💰", new Vector2(0, yPos), new Vector2(200, 50), new Color(0.3f, 0.5f, 0.3f, 1f));
        yPos -= 50;

        CreateText(container.transform, "StatusText", "", 18, yPos, FontStyles.Normal, Color.green);
        
        GameObject statusObj = container.transform.Find("StatusText").gameObject;
        statusObj.SetActive(false);

        CreateButton(container.transform, "CloseButton", "✕", new Vector2(215, 290), new Vector2(50, 50), new Color(0.6f, 0.2f, 0.2f, 1f));

        panel.SetActive(false);

        Selection.activeGameObject = panel;
        Debug.Log("GemShopPanel UI created!");
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

    static GameObject CreateText(Transform parent, string name, string text, int fontSize, float yPos, FontStyles style, Color color, TextAlignmentOptions alignment = TextAlignmentOptions.Center, Vector2 offset = default)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 50);
        rect.anchoredPosition = new Vector2(offset.x, yPos + offset.y);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = alignment;
        tmp.color = color;

        return textObj;
    }

    static void CreateDivider(Transform parent, float yPos)
    {
        GameObject divider = new GameObject("Divider");
        divider.transform.SetParent(parent, false);

        RectTransform rect = divider.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(420, 2);
        rect.anchoredPosition = new Vector2(0, yPos);

        Image image = divider.AddComponent<Image>();
        image.color = new Color(0.3f, 0.3f, 0.4f, 1f);
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
        tmp.fontSize = 18;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return buttonObj;
    }
}
