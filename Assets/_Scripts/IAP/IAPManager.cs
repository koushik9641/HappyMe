using System;
using UnityEngine;
using UnityEngine.Purchasing;
//using UnityEditor;

//Dependencies DConsole, Ad, Toast
//22/02/2021
//01/03/2022
//18/03/2021
//25/12/2021

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    [SerializeField] private bool _showLocalPrice = false;
    [SerializeField] private string _currencySymbol = "$";

    public IAPProducts[] products;

    public delegate void IAPCallback(string productID);
    public IAPCallback callbackAfterPurchase;

    private string _error = "";
    public string error { get { return _error; } }

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	void Start()
	{
		if (m_StoreController == null)
		{
            DConsole.Log("Initializing Purchase");
			InitializePurchasing();
		}

        RegisterProducts();
    }

    #region IAP Callbacks

    void InitializePurchasing()
    {
        if (IsInitialized())
        {
            DConsole.Log("Already Initialized");
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var item in products)
        {
            builder.AddProduct(item.productID, item.productType);
        }

        DConsole.Log("Do Initializing...");
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        DConsole.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        UpdateStorePrice();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        DConsole.Log("OnInitializeFailed InitializationFailureReason:" + error);

            if (error == InitializationFailureReason.PurchasingUnavailable)
                _error = "Billing is disabled in your device settings!";
            /*else
                _error = "Shop unavailable please mail at " + Config.supportEmail + "!";*/

        /*if (iapProducts.Length > 0)
        {
            foreach (var pro in iapProducts)
                pro.button.interactable = false;
        }*/
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        DConsole.Log("Processing Purchase...");
        bool flag = false;
        foreach (var pro in products)
        {
            if (String.Equals(args.purchasedProduct.definition.id, pro.productID, StringComparison.Ordinal))
            {
                flag = true;

                if (pro.removeAd == IsNoAd.Yes)
                {
                    RemoveAd(pro.productID);
                }
                else
                {
                    //@ AdManager.Instance.AddRewardAmount(pro.itemQuantity);

                    Toast.ShowMessage("Product purchased successfully!");
                }

                callbackAfterPurchase?.Invoke(pro.productID);
            }
        }

        if (!flag)
        {
            DConsole.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        DConsole.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

        string error;
        if (failureReason == PurchaseFailureReason.DuplicateTransaction)
            error = "Product already purchased!";
        else if (failureReason == PurchaseFailureReason.UserCancelled)
            error = "User Cancelled!";
        else if (failureReason == PurchaseFailureReason.PaymentDeclined)
            error = "Payment Declined!";
        else if (failureReason == PurchaseFailureReason.ExistingPurchasePending)
            error = "Existing Purchase Pending! Please wait.";
        else
            error = failureReason.ToString();

        Toast.ShowMessage("Failed! " + error);
    }

    #endregion

    public IAPProducts GetProductByID(string productID)
    {
        foreach (var item in products)
        {
            if (item.productID == productID)
                return item;
        }

        return null;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    private void RegisterProducts()
    {
        foreach (var pro in products)
        {
            if (pro.shopProductItem != null)
            {
                pro.shopProductItem.textPrice.text = _currencySymbol + pro.price.ToString();
                pro.shopProductItem.button.onClick.RemoveAllListeners();
                pro.shopProductItem.button.onClick.AddListener(() => BuyProductID(pro.productID));
            }
            else
                DConsole.LogError("Product Item not specified for Product ID: " + pro.productID);
        }

        // Called again to slove multiple scene product regestering
        if (IsInitialized())
            UpdateStorePrice();

        DConsole.Log("Products Registered");
    }

    private void UpdateStorePrice()
    {
        DConsole.Log("Update Store Price");

        if (m_StoreController != null && products != null)
        {
            if (products.Length > 0)
            {
                foreach (var pro in products)
                    if (pro.productType == ProductType.NonConsumable && (IsPurchased(pro.productID) || m_StoreController.products.WithID(pro.productID).hasReceipt))
                    {
                        if (pro.shopProductItem != null)
                        {
                            pro.shopProductItem.textPrice.text = "(Purchased)";
                            //pro.productItem.textPrice.fontSize = 30;
                            pro.shopProductItem.button.interactable = false;
                        }
                    }
                    else if(pro.shopProductItem != null)
                    {
                        if (_showLocalPrice)
                            pro.shopProductItem.textPrice.text = m_StoreController.products.WithID(pro.productID).metadata.localizedPriceString;
                        else
                            pro.shopProductItem.textPrice.text = _currencySymbol + pro.price.ToString();
                    }
            }
            else
                DConsole.LogWarning("No product found!");
        }
    }

    private static bool IsPurchased(string productID)
    {
        return 1 == PlayerPrefs.GetInt(productID + "Purchased", 0);
    }

    private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
            if (product == null)
            {
                Toast.ShowMessage("Failed! Product not found.");
            }
            else if (!product.availableToPurchase)
            {
                Toast.ShowMessage("Failed! Product is not available for purchase.");
            }
            else
            {
                DConsole.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
		}
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.

            Toast.ShowMessage("Failed! Make sure you have an active Internet connection or try again after some time.");
        }
	}

    private void RemoveAd(string productID)
    {
        DConsole.Log("Remove Ad");

        PlayerPrefs.SetInt(productID + "Purchased", 1);
        Config.isAdRemoved = true;

        //@ AdManager.Instance.DestroyBannerAd();
        Toast.ShowMessage("Ad removed successfully!");

        foreach (var pro in products)
        {
            if (pro.productType == ProductType.NonConsumable && (PlayerPrefs.GetInt(pro.productID + "Purchased", 0) == 1 || m_StoreController.products.WithID(pro.productID).hasReceipt))
            {
                if (pro.shopProductItem != null)
                {
                    pro.shopProductItem.textPrice.text = "(Purchased)";
                    //pro.productItem.textPrice.fontSize = 30;
                    pro.shopProductItem.button.interactable = false;
                }
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}

public enum IsNoAd
{
    No = 0,
    Yes = 1
}

[Serializable]
public class IAPProducts
{
    public string productID;
    public int quantity;
    public float price;
    public ProductType productType;
    public IsNoAd removeAd;
    public ShopProductItem shopProductItem;
}

/*[CustomEditor(typeof(IAPManager))]
public class IAPManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IAPManager iapManager = (IAPManager)target;

        DrawDefaultInspector();

        if (iapManager.products.Length == 0)
        {
            iapManager.products = new IAPProducts[] { 
                new IAPProducts { productID = "remove_ads", quantity = 0, productType = ProductType.NonConsumable, removeAd = IsNoAd.Yes },
                new IAPProducts { productID = "coins_1", quantity = 0, productType = ProductType.Consumable, removeAd = IsNoAd.No },
                new IAPProducts { productID = "coins_2", quantity = 0, productType = ProductType.Consumable, removeAd = IsNoAd.No },
                new IAPProducts { productID = "coins_3", quantity = 0, productType = ProductType.Consumable, removeAd = IsNoAd.No },
                new IAPProducts { productID = "coins_4", quantity = 0, productType = ProductType.Consumable, removeAd = IsNoAd.No },
            };
        }
    }
}*/