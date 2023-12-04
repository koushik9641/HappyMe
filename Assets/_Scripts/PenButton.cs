using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenButton : MonoBehaviour
{
    Texture2D texture;
    public int price;

    public void ButtonClick()
    {
        Debug.Log("Pressed button click:");
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            if (price > Shop.instance.balance)
            {
                //Do what you want
                return;
            }
            else
            {
                //Debug.Log("object name:" + transform.GetChild(2).GetChild(0).gameObject);
                Debug.Log("Possible buy:");
                Shop.instance.BuyItem("pen", price, GetMyID());
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);

                transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
          

            }
        }
        else
        {
            texture = GetComponent<Image>().sprite.texture;
            // Encode the texture as a PNG
            byte[] textureBytes = texture.EncodeToPNG();

            // Convert the byte array to a Base64 string
            string textureBase64 = System.Convert.ToBase64String(textureBytes);

            PlayerPrefs.SetString("pen", textureBase64);
            Transform parent = transform.parent;

            parent.GetChild(Shop.instance.shopdata.selectedPen).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            Shop.instance.shopdata.selectedPen = GetMyID();

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
