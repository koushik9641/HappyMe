using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassButton : MonoBehaviour
{
    Texture2D texture;
    public int price;

    public void ButtonClick()
    {
        Debug.Log("Pressed button click:");
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            Debug.Log("Balanace money:" + Shop.instance.balance);
            if (price > Shop.instance.balance)
            {
                Debug.Log("not Possible buy:");
                //Do what you want
                return;

            }
            else
            {
                Debug.Log("Possible buy:");
                Shop.instance.BuyItem("glass", price, GetMyID());
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(6).gameObject.SetActive(false);

            }
        }
        else
        {

            texture = GetComponent<Image>().sprite.texture;
            // Encode the texture as a PNG
            byte[] textureBytes = texture.EncodeToPNG();

            // Convert the byte array to a Base64 string
            string textureBase64 = System.Convert.ToBase64String(textureBytes);

            PlayerPrefs.SetString("glass", textureBase64);

            SaveTextureToFile(transform.GetChild(2).GetComponent<Image>().sprite.texture, "Happy.png");
            SaveTextureToFile(transform.GetChild(3).GetComponent<Image>().sprite.texture, "Sad.png");
            SaveTextureToFile(transform.GetChild(4).GetComponent<Image>().sprite.texture, "Surprize.png");

            Transform parent = transform.parent;

            parent.GetChild(Shop.instance.shopdata.selectedGlass).GetChild(1).gameObject.SetActive(true);

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(6).gameObject.SetActive(false);
            Debug.Log("object name:" + parent.GetChild(Shop.instance.shopdata.selectedGlass).GetChild(1).gameObject);
            Shop.instance.shopdata.selectedGlass = GetMyID();

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
    public void SaveTextureToFile(Texture2D textureToSave, string fileName)
    {
        if (textureToSave == null)
        {
            Debug.LogError("No texture to save.");
            return;
        }

        // Encode the texture as a PNG
        byte[] textureBytes = textureToSave.EncodeToPNG();

        // Get the full file path using Application.persistentDataPath
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            // Write the bytes to the file
            System.IO.File.WriteAllBytes(filePath, textureBytes);
            Debug.Log("Saved texture to: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving texture: " + e.Message);
        }
    }
}

