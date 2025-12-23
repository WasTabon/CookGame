using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public static IAPManager Instance { get; private set; }
    
    [Header("Product IDs")]
    public string gemPackProductId = "com.yourgame.gems50";
    
    [Header("Gem Pack Settings")]
    public int gemsPerPurchase = 50;
    
    private IStoreController storeController;
    private IExtensionProvider extensionProvider;
    
    public bool IsInitialized => storeController != null && extensionProvider != null;
    
    public event Action<int> OnPurchaseComplete;
    public event Action<string> PurchaseFailed;
    public event Action OnRestoreComplete;
    public event Action<string> OnRestoreFailed;
    
    void Awake()
    {
        Debug.Log("[IAPManager] Awake() called");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePurchasing();
            Debug.Log("[IAPManager] ✅ Singleton created");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializePurchasing()
    {
        if (IsInitialized)
        {
            Debug.Log("[IAPManager] Already initialized");
            return;
        }
        
        Debug.Log("[IAPManager] Initializing Unity IAP...");
        
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        builder.AddProduct(gemPackProductId, ProductType.Consumable);
        
        UnityPurchasing.Initialize(this, builder);
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("[IAPManager] ✅ Unity IAP initialized successfully");
        
        storeController = controller;
        extensionProvider = extensions;
        
        LogProductInfo();
    }
    
    void LogProductInfo()
    {
        if (storeController == null) return;
        
        var product = storeController.products.WithID(gemPackProductId);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"[IAPManager] Product: {product.metadata.localizedTitle}");
            Debug.Log($"[IAPManager] Price: {product.metadata.localizedPriceString}");
            Debug.Log($"[IAPManager] Description: {product.metadata.localizedDescription}");
        }
        else
        {
            Debug.LogWarning($"[IAPManager] ⚠️ Product {gemPackProductId} not available");
        }
    }
    
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"[IAPManager] ❌ Initialization failed: {error}");
    }
    
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"[IAPManager] ❌ Initialization failed: {error} - {message}");
    }
    
    public void BuyGemPack()
    {
        BuyProduct(gemPackProductId);
    }
    
    void BuyProduct(string productId)
    {
        if (!IsInitialized)
        {
            Debug.LogError("[IAPManager] ❌ Not initialized!");
            PurchaseFailed?.Invoke("Store not initialized");
            return;
        }
        
        var product = storeController.products.WithID(productId);
        
        if (product == null)
        {
            Debug.LogError($"[IAPManager] ❌ Product {productId} not found");
            PurchaseFailed?.Invoke("Product not found");
            return;
        }
        
        if (!product.availableToPurchase)
        {
            Debug.LogError($"[IAPManager] ❌ Product {productId} not available");
            PurchaseFailed?.Invoke("Product not available");
            return;
        }
        
        Debug.Log($"[IAPManager] 🛒 Purchasing: {productId}");
        storeController.InitiatePurchase(product);
    }
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        
        Debug.Log($"[IAPManager] ✅ Purchase successful: {productId}");
        
        if (productId == gemPackProductId)
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.AddGems(gemsPerPurchase);
                Debug.Log($"[IAPManager] 💎 Added {gemsPerPurchase} gems");
            }
            
            OnPurchaseComplete?.Invoke(gemsPerPurchase);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCoinCollect();
            }
            
            HapticController.Success();
        }
        
        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"[IAPManager] ❌ Purchase failed: {product.definition.id} - {failureReason}");
        PurchaseFailed?.Invoke(failureReason.ToString());
        
        HapticController.Error();
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"[IAPManager] ❌ Purchase failed: {product.definition.id} - {failureDescription.reason}: {failureDescription.message}");
        PurchaseFailed?.Invoke(failureDescription.message);
        
        HapticController.Error();
    }
    
    public void RestorePurchases()
    {
        if (!IsInitialized)
        {
            Debug.LogError("[IAPManager] ❌ Not initialized!");
            OnRestoreFailed?.Invoke("Store not initialized");
            return;
        }
        
        Debug.Log("[IAPManager] 🔄 Restoring purchases...");
        
#if UNITY_IOS
        var apple = extensionProvider.GetExtension<IAppleExtensions>();
        apple.RestoreTransactions((result, error) =>
        {
            if (result)
            {
                Debug.Log("[IAPManager] ✅ Restore complete");
                OnRestoreComplete?.Invoke();
            }
            else
            {
                Debug.LogError($"[IAPManager] ❌ Restore failed: {error}");
                OnRestoreFailed?.Invoke(error);
            }
        });
#else
        Debug.Log("[IAPManager] Restore not needed on this platform");
        OnRestoreComplete?.Invoke();
#endif
    }
    
    public string GetLocalizedPrice()
    {
        if (!IsInitialized) return "---";
        
        var product = storeController.products.WithID(gemPackProductId);
        if (product != null && product.availableToPurchase)
        {
            return product.metadata.localizedPriceString;
        }
        
        return "---";
    }
    
    public bool CanPurchase()
    {
        if (!IsInitialized) return false;
        
        var product = storeController.products.WithID(gemPackProductId);
        return product != null && product.availableToPurchase;
    }
}
