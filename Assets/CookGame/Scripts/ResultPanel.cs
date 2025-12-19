using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultPanel : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text resultTitleText;
    public TMP_Text resultMessageText;
    
    [Header("Button References")]
    public Button continueButton;
    
    [Header("Visual")]
    public Image backgroundImage;
    public Color victoryColor = new Color(0.2f, 0.8f, 0.2f, 0.9f);
    public Color failColor = new Color(0.8f, 0.2f, 0.2f, 0.9f);
    
    void Awake()
    {
        Debug.Log("[ResultPanel] Awake() called");
    }
    
    void Start()
    {
        Debug.Log("[ResultPanel] Start() called");
        ValidateReferences();
        SetupButtons();
    }
    
    void ValidateReferences()
    {
        if (resultTitleText == null)
            Debug.LogWarning("[ResultPanel] ⚠️ Result Title Text is NULL");
        if (continueButton == null)
            Debug.LogError("[ResultPanel] ❌ Continue Button is NULL!");
    }
    
    void SetupButtons()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
            Debug.Log("[ResultPanel] ✅ Continue button listener added");
        }
    }
    
    public void DisplayResult(bool victory)
    {
        Debug.Log($"[ResultPanel] DisplayResult: Victory = {victory}");
        
        if (resultTitleText != null)
        {
            resultTitleText.text = victory ? "SUCCESS!" : "FAILED!";
            resultTitleText.color = victory ? Color.green : Color.red;
        }
        
        if (resultMessageText != null)
        {
            resultMessageText.text = victory 
                ? "All meters are in target range!\nPerfect cooking!" 
                : "One or more meters are out of range.\nTry again!";
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.color = victory ? victoryColor : failColor;
        }
        
        Debug.Log("[ResultPanel] ✅ Result displayed");
    }
    
    void OnContinueClicked()
    {
        Debug.Log("[ResultPanel] Continue button clicked!");
        
        continueButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5);
        
        DOVirtual.DelayedCall(0.3f, () => {
            Hide();
            GameManager.Instance.ReturnToMenu();
        });
    }
    
    public void Show()
    {
        Debug.Log("[ResultPanel] Show()");
        
        gameObject.SetActive(true);
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 0.5f;
        
        Sequence showSequence = DOTween.Sequence();
        showSequence.Append(canvasGroup.DOFade(1f, 0.4f));
        showSequence.Join(transform.DOScale(1f, 0.5f).SetEase(Ease.OutElastic));
        
        if (resultTitleText != null)
        {
            resultTitleText.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 5).SetDelay(0.3f);
        }
        
        Debug.Log("[ResultPanel] ✅ Show animation started");
    }
    
    public void Hide()
    {
        Debug.Log("[ResultPanel] Hide()");
        
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Append(canvasGroup.DOFade(0f, 0.2f));
        hideSequence.Join(transform.DOScale(0.8f, 0.2f).SetEase(Ease.InBack));
        hideSequence.OnComplete(() => gameObject.SetActive(false));
        
        Debug.Log("[ResultPanel] ✅ Hide animation started");
    }
}
