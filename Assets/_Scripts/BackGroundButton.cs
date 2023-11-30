using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundButton : MonoBehaviour
{
    Texture2D texture;
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
                Shop.instance.BuyItem("background", price, GetMyID());
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else
        {
            texture = GetComponent<Image>().sprite.texture;
            // Encode the texture as a PNG
            byte[] textureBytes = texture.EncodeToPNG();

            // Convert the byte array to a Base64 string
            string textureBase64 = System.Convert.ToBase64String(textureBytes);

            PlayerPrefs.SetString("background", textureBase64);
            Transform parent = transform.parent;

            parent.GetChild(Shop.instance.shopdata.selectedBackGround).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            Shop.instance.shopdata.selectedBackGround = GetMyID();

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
}
