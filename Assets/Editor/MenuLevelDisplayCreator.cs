using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class MenuLevelDisplayCreator : EditorWindow
{
    [MenuItem("Probability Kitchen/Create Menu Level Display UI")]
    public static void CreateDisplay()
    {
        GameObject menuPanel = Selection.activeGameObject;
        if (menuPanel == null)
        {
            Debug.LogError("Select MenuPanel GameObject first!");
            return;
        }

        GameObject levelDisplay = new GameObject("LevelDisplay");
        levelDisplay.transform.SetParent(menuPanel.transform, false);

        RectTransform displayRect = levelDisplay.AddComponent<RectTransform>();
        displayRect.anchorMin = new Vector2(0, 1);
        displayRect.anchorMax = new Vector2(0, 1);
        displayRect.pivot = new Vector2(0, 1);
        displayRect.sizeDelta = new Vector2(300, 100);
        displayRect.anchoredPosition = new Vector2(20, -20);

        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(levelDisplay.transform, false);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f);

        GameObject levelText = new GameObject("LevelText");
        levelText.transform.SetParent(levelDisplay.transform, false);
        RectTransform levelRect = levelText.AddComponent<RectTransform>();
        levelRect.anchorMin = new Vector2(0, 1);
        levelRect.anchorMax = new Vector2(0, 1);
        levelRect.pivot = new Vector2(0, 1);
        levelRect.sizeDelta = new Vector2(100, 40);
        levelRect.anchoredPosition = new Vector2(10, -10);

        TextMeshProUGUI levelTmp = levelText.AddComponent<TextMeshProUGUI>();
        levelTmp.text = "Lv.1";
        levelTmp.fontSize = 28;
        levelTmp.fontStyle = FontStyles.Bold;
        levelTmp.alignment = TextAlignmentOptions.Left;
        levelTmp.color = Color.white;

        GameObject streakText = new GameObject("StreakText");
        streakText.transform.SetParent(levelDisplay.transform, false);
        RectTransform streakRect = streakText.AddComponent<RectTransform>();
        streakRect.anchorMin = new Vector2(1, 1);
        streakRect.anchorMax = new Vector2(1, 1);
        streakRect.pivot = new Vector2(1, 1);
        streakRect.sizeDelta = new Vector2(80, 40);
        streakRect.anchoredPosition = new Vector2(-10, -10);

        TextMeshProUGUI streakTmp = streakText.AddComponent<TextMeshProUGUI>();
        streakTmp.text = "🔥 3";
        streakTmp.fontSize = 24;
        streakTmp.fontStyle = FontStyles.Bold;
        streakTmp.alignment = TextAlignmentOptions.Right;
        streakTmp.color = new Color(1f, 0.6f, 0.2f, 1f);

        GameObject xpSlider = new GameObject("XPSlider");
        xpSlider.transform.SetParent(levelDisplay.transform, false);

        RectTransform sliderRect = xpSlider.AddComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0, 0);
        sliderRect.anchorMax = new Vector2(1, 0);
        sliderRect.pivot = new Vector2(0.5f, 0);
        sliderRect.sizeDelta = new Vector2(-20, 16);
        sliderRect.anchoredPosition = new Vector2(0, 30);

        Slider slider = xpSlider.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0.4f;
        slider.interactable = false;

        GameObject sliderBg = new GameObject("Background");
        sliderBg.transform.SetParent(xpSlider.transform, false);
        RectTransform sliderBgRect = sliderBg.AddComponent<RectTransform>();
        sliderBgRect.anchorMin = Vector2.zero;
        sliderBgRect.anchorMax = Vector2.one;
        sliderBgRect.offsetMin = Vector2.zero;
        sliderBgRect.offsetMax = Vector2.zero;
        Image sliderBgImage = sliderBg.AddComponent<Image>();
        sliderBgImage.color = new Color(0.2f, 0.2f, 0.25f, 1f);

        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(xpSlider.transform, false);
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
        fillImage.color = new Color(0.4f, 0.8f, 1f, 1f);

        slider.fillRect = fillRect;

        GameObject xpText = new GameObject("XPText");
        xpText.transform.SetParent(levelDisplay.transform, false);
        RectTransform xpRect = xpText.AddComponent<RectTransform>();
        xpRect.anchorMin = new Vector2(0.5f, 0);
        xpRect.anchorMax = new Vector2(0.5f, 0);
        xpRect.pivot = new Vector2(0.5f, 0);
        xpRect.sizeDelta = new Vector2(200, 25);
        xpRect.anchoredPosition = new Vector2(0, 5);

        TextMeshProUGUI xpTmp = xpText.AddComponent<TextMeshProUGUI>();
        xpTmp.text = "40 / 100 XP";
        xpTmp.fontSize = 16;
        xpTmp.alignment = TextAlignmentOptions.Center;
        xpTmp.color = new Color(0.7f, 0.7f, 0.7f, 1f);

        Selection.activeGameObject = levelDisplay;
        Debug.Log("Menu Level Display UI created!");
    }
}
