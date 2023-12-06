using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Gley.AllPlatformsSave;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    

    public Text txtTotalStar;

    public GameObject[] Glasses;

    public GameObject[] Pencils;

    public GameObject[] Colors;

    public GameObject[] Backgrounds;

    public Transform GlassesContent;
    public Transform PencilsContent;
    public Transform ColorsContent;
    public Transform BackgroundsContent;

   
    public int balance = 5000;

    public Transform parent;

    public ShopData shopdata;
    // Use this for initialization

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        API.Load<ShopData>("ShopData", DataWasLoaded, true);

        if (shopdata != null)
        {
            foreach(Item item in shopdata.PurchasedItems)
            {
                if (item.type == "glass")
                {
                    GlassesContent.GetChild(item.index).GetChild(0).gameObject.SetActive(false);
                    GlassesContent.GetChild(item.index).GetChild(1).gameObject.SetActive(true);
                    GlassesContent.GetChild(item.index).GetChild(6).gameObject.SetActive(false);
                }
                else if (item.type == "pen")
                {
                    PencilsContent.GetChild(item.index).GetChild(0).gameObject.SetActive(false);
                    PencilsContent.GetChild(item.index).GetChild(1).gameObject.SetActive(true);
                    //Debug.Log("object name:" + PencilsContent.GetChild(item.index).GetChild(2).GetChild(0).gameObject);
                    PencilsContent.GetChild(item.index).GetChild(2).GetChild(0).gameObject.SetActive(false);
                }
                else if (item.type == "color")
                {
                    ColorsContent.GetChild(item.index).GetChild(0).gameObject.SetActive(false);
                    ColorsContent.GetChild(item.index).GetChild(1).gameObject.SetActive(true);
                    ColorsContent.GetChild(item.index).GetChild(2).GetChild(0).gameObject.SetActive(false);
                }
                else if (item.type == "background")
                {
                    BackgroundsContent.GetChild(item.index).GetChild(0).gameObject.SetActive(false);
                    BackgroundsContent.GetChild(item.index).GetChild(1).gameObject.SetActive(true);
                    BackgroundsContent.GetChild(item.index).GetChild(2).GetChild(0).gameObject.SetActive(false);
                }
            }
            int numOpenedItems = PlayerPrefs.GetInt("curLevel", 1) / 10 + 1;
            for (int i = 0; i < GlassesContent.childCount & i < numOpenedItems; i++)
            {
              //  GlassesContent.GetChild(i).GetChild(5).GetChild(0).gameObject.SetActive(false);
                GlassesContent.GetChild(i).GetComponent<Button>().interactable = true;
            }
            for (int i = 0; i < ColorsContent.childCount & i < numOpenedItems; i++)
            {
                ColorsContent.GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                ColorsContent.GetChild(i).GetComponent<Button>().interactable = true;
            }
            for (int i = 0; i < PencilsContent.childCount & i < numOpenedItems; i++)
            {               
                PencilsContent.GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                PencilsContent.GetChild(i).GetComponent<Button>().interactable = true;
            }
            for (int i = 0; i < BackgroundsContent.childCount & i < numOpenedItems; i++)
            {
                BackgroundsContent.GetChild(i).GetChild(2).GetChild(0).gameObject.SetActive(false);
                BackgroundsContent.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }
        
        GlassesContent.GetChild(shopdata.selectedGlass).GetChild(1).gameObject.SetActive(false);
        PencilsContent.GetChild(shopdata.selectedPen).GetChild(1).gameObject.SetActive(false);
        ColorsContent.GetChild(shopdata.selectedColor).GetChild(1).gameObject.SetActive(false);
        BackgroundsContent.GetChild(shopdata.selectedBackGround).GetChild(1).gameObject.SetActive(false);
    }
    void Start()
    {

        //this method will be called after load process is done
        //data -> actual loaded data.
        //result -> Succes/Error
        //mesage -> error message
        balance = PlayerPrefs.GetInt("totalStar", 5000);
        txtTotalStar.text = balance.ToString();
        SceneTransition.Instance.Out();

    }

    public void SaveData()
    {
        API.Save(shopdata, "ShopData", DataWasSaved, true);
    }
    //this method will be called when save process is complete
    private void DataWasSaved(SaveResult result, string message)
    {
        if (result == SaveResult.Error)
        {
            //Saving error 
            Debug.Log(message);
        }
    }

    private void DataWasLoaded(ShopData data, SaveResult result, string message)
    {
        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            //No Data File Found -> Creating new data...";
            shopdata = new ShopData();
        }

        if (result == SaveResult.Success)
        {
            //store the loading date to use it later in your app
            shopdata = data;
        }
    }

    public void homeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.WaterLogo);
    }

    public void BuyItem(string type, int price, int index)
    {
        Item item = new Item
        {
            type = type,
            index = index
        };
        shopdata.PurchasedItems.Add(item);
        balance -= price;
        PlayerPrefs.SetInt("totalStar", balance);
        Debug.Log("balance:" + balance);
        Debug.Log("balance in pref:" + PlayerPrefs.GetInt("totalStar").ToString());
        txtTotalStar.text = balance.ToString();
        SaveData();
    }
}

[System.Serializable]
public class Item
{
    public string type; //glass , pen , color or background
    public int index = -1;
}
[System.Serializable]
public class ShopData
{
    public List<Item> PurchasedItems = new List<Item>();

    public int selectedGlass = 0;
    public int selectedPen = 0;
    public int selectedColor = 0;
    public int selectedBackGround = 0;

    public ShopData()
    {
        // Initialize the PurchasedItems list in the constructor
        PurchasedItems.Add(new Item { type = "glass", index = 0 });
        PurchasedItems.Add(new Item { type = "pen", index = 0 });
        PurchasedItems.Add(new Item { type = "color", index = 0 });
        PurchasedItems.Add(new Item { type = "background", index = 0 });
    }
}



