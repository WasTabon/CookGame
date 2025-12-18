using UnityEngine;
using TMPro;
using DG.Tweening;

public class ResultPanel : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI messageText;
    
    void Awake()
    {
        Debug.Log("[ResultPanel] Awake() called");
    }
    
    public void ShowResult(bool victory)
    {
        Debug.Log($"[ResultPanel] ShowResult: {(victory ? "VICTORY" : "DEFEAT")}");
        
        if (resultText != null)
        {
            resultText.text = victory ? "SUCCESS!" : "FAILED!";
            resultText.color = victory ? Color.green : Color.red;
        }
        
        if (messageText != null)
        {
            messageText.text = victory ? 
                "All meters in target range!" : 
                "Meters outside target or overflow!";
        }
        
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        
        if (resultText != null)
        {
            resultText.alpha = 0;
            resultText.DOFade(1f, 0.5f).SetDelay(0.2f);
        }
        
        if (messageText != null)
        {
            messageText.alpha = 0;
            messageText.DOFade(1f, 0.5f).SetDelay(0.4f);
        }
    }
    
    public void OnContinueButtonPressed()
    {
        Debug.Log("[ResultPanel] Continue button pressed - returning to menu");
        GameManager.Instance.uiManager.ShowMenuPanel();
    }
}
