using UnityEngine;
using System;

[System.Serializable]
public class PlayerProgressData
{
    public int level = 1;
    public int currentXP = 0;
    public int totalXPEarned = 0;
    public int ordersCompleted = 0;
    public int ordersFailed = 0;
    public int perfectOrders = 0;
    public int jackpotsTriggered = 0;
    public int highestStreak = 0;
    public int currentStreak = 0;
    public string lastPlayDate = "";
}

public class PlayerProgressManager : MonoBehaviour
{
    public static PlayerProgressManager Instance { get; private set; }
    
    private const string SAVE_KEY = "PlayerProgress";
    
    [Header("Level Settings")]
    public int baseXPPerLevel = 100;
    public float xpScalingFactor = 1.5f;
    public int maxLevel = 50;
    
    [Header("XP Rewards")]
    public int xpPerOrder = 10;
    public int xpBonusPerfect = 25;
    public int xpBonusGood = 15;
    public int xpBonusOkay = 5;
    public int xpBonusStreak = 5;
    
    public PlayerProgressData Data { get; private set; }
    
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnStreakChanged;
    
    void Awake()
    {
        Debug.Log("[PlayerProgressManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
            Debug.Log("[PlayerProgressManager] ✅ Singleton instance created");
        }
        else
        {
            Debug.LogWarning("[PlayerProgressManager] ⚠️ Duplicate destroyed");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateLastPlayDate();
    }
    
    public int GetXPForLevel(int level)
    {
        return Mathf.RoundToInt(baseXPPerLevel * Mathf.Pow(xpScalingFactor, level - 1));
    }
    
    public int GetXPForNextLevel()
    {
        return GetXPForLevel(Data.level);
    }
    
    public float GetLevelProgress()
    {
        int xpNeeded = GetXPForNextLevel();
        return xpNeeded > 0 ? (float)Data.currentXP / xpNeeded : 0f;
    }
    
    public void AddXP(int amount)
    {
        if (Data.level >= maxLevel)
        {
            Debug.Log("[PlayerProgressManager] Max level reached");
            return;
        }
        
        int oldXP = Data.currentXP;
        Data.currentXP += amount;
        Data.totalXPEarned += amount;
        
        Debug.Log($"[PlayerProgressManager] ⭐ XP: {oldXP} + {amount} = {Data.currentXP}");
        
        CheckLevelUp();
        
        OnXPChanged?.Invoke(Data.currentXP, GetXPForNextLevel());
        SaveProgress();
    }
    
    void CheckLevelUp()
    {
        while (Data.currentXP >= GetXPForNextLevel() && Data.level < maxLevel)
        {
            Data.currentXP -= GetXPForNextLevel();
            Data.level++;
            
            Debug.Log($"[PlayerProgressManager] 🎉 LEVEL UP! Now level {Data.level}");
            
            OnLevelUp?.Invoke(Data.level);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayVictory();
            }
            
            if (VFXController.Instance != null)
            {
                VFXController.Instance.FlashVictory();
                VFXController.Instance.ShakeHeavy();
            }
            
            HapticController.Success();
        }
    }
    
    public void RecordOrderComplete(string grade, int reward)
    {
        Data.ordersCompleted++;
        
        int xpEarned = xpPerOrder;
        
        switch (grade)
        {
            case "PERFECT":
                Data.perfectOrders++;
                xpEarned += xpBonusPerfect;
                Data.currentStreak++;
                break;
            case "GOOD":
                xpEarned += xpBonusGood;
                Data.currentStreak++;
                break;
            case "OKAY":
                xpEarned += xpBonusOkay;
                Data.currentStreak = 0;
                break;
            default:
                Data.currentStreak = 0;
                break;
        }
        
        if (Data.currentStreak > 1)
        {
            int streakBonus = xpBonusStreak * (Data.currentStreak - 1);
            xpEarned += streakBonus;
            Debug.Log($"[PlayerProgressManager] 🔥 Streak bonus: +{streakBonus} XP (streak: {Data.currentStreak})");
        }
        
        if (Data.currentStreak > Data.highestStreak)
        {
            Data.highestStreak = Data.currentStreak;
            Debug.Log($"[PlayerProgressManager] 🏆 New highest streak: {Data.highestStreak}");
        }
        
        OnStreakChanged?.Invoke(Data.currentStreak);
        
        Debug.Log($"[PlayerProgressManager] ✅ Order complete: {grade}, +{xpEarned} XP");
        AddXP(xpEarned);
    }
    
    public void RecordOrderFailed()
    {
        Data.ordersFailed++;
        Data.currentStreak = 0;
        
        OnStreakChanged?.Invoke(Data.currentStreak);
        
        Debug.Log($"[PlayerProgressManager] ❌ Order failed. Streak reset.");
        SaveProgress();
    }
    
    public void RecordJackpot()
    {
        Data.jackpotsTriggered++;
        Debug.Log($"[PlayerProgressManager] 🎰 Jackpots triggered: {Data.jackpotsTriggered}");
        SaveProgress();
    }
    
    void UpdateLastPlayDate()
    {
        Data.lastPlayDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SaveProgress();
    }
    
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("[PlayerProgressManager] 💾 Progress saved");
    }
    
    void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            Data = JsonUtility.FromJson<PlayerProgressData>(json);
            Debug.Log($"[PlayerProgressManager] 📂 Progress loaded - Level {Data.level}, XP {Data.currentXP}");
        }
        else
        {
            Data = new PlayerProgressData();
            Debug.Log("[PlayerProgressManager] 📂 New progress created");
        }
    }
    
    public void ResetProgress()
    {
        Debug.Log("[PlayerProgressManager] ⚠️ Resetting all progress!");
        Data = new PlayerProgressData();
        SaveProgress();
        
        OnXPChanged?.Invoke(Data.currentXP, GetXPForNextLevel());
        OnStreakChanged?.Invoke(Data.currentStreak);
    }
    
    public int GetTotalOrders()
    {
        return Data.ordersCompleted + Data.ordersFailed;
    }
    
    public float GetSuccessRate()
    {
        int total = GetTotalOrders();
        return total > 0 ? (float)Data.ordersCompleted / total * 100f : 0f;
    }
}
