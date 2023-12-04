using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color MainColor;
    public int price;

    public void ButtonClick()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            if (price > Shop.instance.balance)
            {
                //Do what you want
                return;
            }
            else
            {
                Shop.instance.BuyItem("color", price, GetMyID());
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetString("color", ColorToHexadecimal(MainColor));
            Transform parent = transform.parent;

            parent.GetChild(Shop.instance.shopdata.selectedColor).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            Shop.instance.shopdata.selectedColor = GetMyID();

            Shop.instance.SaveData();
        }
    }

    private int GetMyID()
    {
        Transform parent = transform.parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child != transform)
            {
                continue;
            }
            else
            {
                return i;
            }
        }

        return -1;
    }

    string ColorToHexadecimal(Color color)
    {
        Color32 color32 = (Color32)color; // Convert to Color32 to get byte values

        string hexColor = "#" +
            color32.r.ToString("X2") +
            color32.g.ToString("X2") +
            color32.b.ToString("X2");

        return hexColor;
    }
}
