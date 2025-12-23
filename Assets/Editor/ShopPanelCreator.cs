using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ShopPanelCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create ShopPanel UI (Boosters)")]
    public static void CreatePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in scene!");
            return;
        }

        GameObject panel = CreatePanel(canvas.transform, "ShopPanel", new Color(0, 0, 0, 0.9f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject container = CreatePanel(panel.transform, "Container", new Color(0.12f, 0.12f, 0.18f, 1f));
        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(550, 700);
        containerRect.anchoredPosition = Vector2.zero;

        float yPos = 310;

        CreateText(container.transform, "TitleText", "🛒 BOOSTER SHOP", 32, yPos, FontStyles.Bold, Color.white);
        yPos -= 50;

        CreateText(container.transform, "CoinsText", "💰 1000", 26, yPos, FontStyles.Bold, new Color(1f, 0.85f, 0.2f, 1f));
        yPos -= 40;

        CreateDivider(container.transform, yPos);
        yPos -= 30;

        GameObject scrollView = new GameObject("ScrollView");
        scrollView.transform.SetParent(container.transform, false);
        RectTransform scrollRect = scrollView.AddComponent<RectTransform>();
        scrollRect.anchorMin = new Vector2(0.5f, 0.5f);
        scrollRect.anchorMax = new Vector2(0.5f, 0.5f);
        scrollRect.sizeDelta = new Vector2(500, 450);
        scrollRect.anchoredPosition = new Vector2(0, yPos - 225);

        Image scrollBg = scrollView.AddComponent<Image>();
        scrollBg.color = new Color(0.08f, 0.08f, 0.12f, 1f);

        ScrollRect scroll = scrollView.AddComponent<ScrollRect>();
        scroll.horizontal = false;
        scroll.vertical = true;

        GameObject viewport = new GameObject("Viewport");
        viewport.transform.SetParent(scrollView.transform, false);
        RectTransform viewportRect = viewport.AddComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(5, 5);
        viewportRect.offsetMax = new Vector2(-5, -5);
        viewport.AddComponent<Image>().color = Color.clear;
        viewport.AddComponent<Mask>().showMaskGraphic = false;

        GameObject content = new GameObject("ItemsContainer");
        content.transform.SetParent(viewport.transform, false);
        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.sizeDelta = new Vector2(0, 600);
        contentRect.anchoredPosition = Vector2.zero;

        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 10;
        layout.padding = new RectOffset(10, 10, 10, 10);
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scroll.viewport = viewportRect;
        scroll.content = contentRect;

        string[] names = { "Extra Turn", "Shield", "Double Coins", "Double XP" };
        string[] descs = { "+1 turn this game", "Block one overflow", "2x coin reward", "2x XP reward" };
        int[] prices = { 200, 300, 400, 400 };

        for (int i = 0; i < 4; i++)
        {
            CreateShopItemSlot(content.transform, $"ItemSlot_{i}", names[i], descs[i], prices[i]);
        }

        CreateButton(container.transform, "CloseButton", "✕", new Vector2(240, 315), new Vector2(50, 50), new Color(0.6f, 0.2f, 0.2f, 1f));

        panel.SetActive(false);

        Selection.activeGameObject = panel;
        Debug.Log("ShopPanel UI (Boosters) created!");
    }

    static void CreateShopItemSlot(Transform parent, string name, string itemName, string desc, int price)
    {
        GameObject slot = CreatePanel(parent, name, new Color(0.18f, 0.18f, 0.25f, 1f));
        RectTransform slotRect = slot.GetComponent<RectTransform>();
        slotRect.sizeDelta = new Vector2(480, 100);

        LayoutElement layoutElement = slot.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = 100;
        layoutElement.flexibleWidth = 1;

        GameObject iconBg = CreatePanel(slot.transform, "IconBackground", new Color(0.25f, 0.25f, 0.35f, 1f));
        RectTransform iconBgRect = iconBg.GetComponent<RectTransform>();
        iconBgRect.anchorMin = new Vector2(0, 0.5f);
        iconBgRect.anchorMax = new Vector2(0, 0.5f);
        iconBgRect.pivot = new Vector2(0, 0.5f);
        iconBgRect.sizeDelta = new Vector2(70, 70);
        iconBgRect.anchoredPosition = new Vector2(15, 0);

        GameObject icon = new GameObject("IconImage");
        icon.transform.SetParent(iconBg.transform, false);
        RectTransform iconRect = icon.AddComponent<RectTransform>();
        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;
        iconRect.offsetMin = new Vector2(10, 10);
        iconRect.offsetMax = new Vector2(-10, -10);
        Image iconImg = icon.AddComponent<Image>();
        iconImg.color = Color.white;

        CreateSlotText(slot.transform, "NameText", itemName, 20, 25, FontStyles.Bold, Color.white, new Vector2(160, 0));
        CreateSlotText(slot.transform, "DescriptionText", desc, 14, 0, FontStyles.Normal, new Color(0.7f, 0.7f, 0.7f, 1f), new Vector2(160, 0));
        CreateSlotText(slot.transform, "PriceText", $"{price} 💰", 18, -25, FontStyles.Bold, new Color(1f, 0.8f, 0.2f, 1f), new Vector2(160, 0));

        CreateSlotButton(slot.transform, "BuyButton", "BUY", new Vector2(-50, 0), new Vector2(80, 45), new Color(0.2f, 0.6f, 0.2f, 1f));
    }

    static void CreateSlotText(Transform parent, string name, string text, int fontSize, float yOffset, FontStyles style, Color color, Vector2 xOffset)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.pivot = new Vector2(0, 0.5f);
        rect.sizeDelta = new Vector2(200, 30);
        rect.anchoredPosition = new Vector2(xOffset.x, yOffset);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Left;
        tmp.color = color;
    }

    static void CreateSlotButton(Transform parent, string name, string text, Vector2 position, Vector2 size, Color color)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);
        rect.pivot = new Vector2(1, 0.5f);
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
        tmp.fontSize = 16;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
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
        rect.sizeDelta = new Vector2(450, 40);
        rect.anchoredPosition = new Vector2(0, yPos);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
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
        rect.sizeDelta = new Vector2(480, 2);
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
