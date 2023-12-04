using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{

    public Image backGround;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("background"))
        {
            string textureBase64 = PlayerPrefs.GetString("background"); // Replace with your actual Base64 string

            // Convert the Base64 string to a byte array
            byte[] textureBytes = System.Convert.FromBase64String(textureBase64);

            // Create a new texture and load the image from the byte array
            Texture2D newTexture = new Texture2D(2, 2); // You might need to set the correct dimensions
            newTexture.LoadImage(textureBytes);

            // Create a new Sprite using the newTexture
            Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

            // Assign the new Sprite to the SpriteRenderer
            backGround.sprite = newSprite;
        }
    }
}
