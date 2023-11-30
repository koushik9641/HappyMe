using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour {

    public PolygonCollider2D PolygonInsideCup;
    public ContactFilter2D LiquidFilter;
    Collider2D[] res = new Collider2D[1000];

    private void Awake()
    {
        Texture2D newTexture;
        //Texture2D texture = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite.texture;

        if (PlayerPrefs.HasKey("glass"))
        {
            string textureBase64 = PlayerPrefs.GetString("glass"); // Replace with your actual Base64 string

            // Convert the Base64 string to a byte array
            byte[] textureBytes = System.Convert.FromBase64String(textureBase64);

            // Create a new texture and load the image from the byte array
            newTexture = new Texture2D(2, 2); // You might need to set the correct dimensions
            newTexture.LoadImage(textureBytes);

            // Create a new Sprite using the newTexture
            Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

            // Assign the new Sprite to the SpriteRenderer
            GetComponent<SpriteRenderer>().sprite = newSprite;

            newTexture = LoadImage("Happy.png");
            if (newTexture)
            {

                // Create a new Sprite using the newTexture
                newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

                // Assign the new Sprite to the SpriteRenderer
                transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            newTexture = LoadImage("Sad.png");
            if (newTexture)
            {
                // Create a new Sprite using the newTexture
                newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

                // Assign the new Sprite to the SpriteRenderer
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = newSprite;
            }
            newTexture = LoadImage("Surprize.png");
            if (newTexture)
            {
                // Create a new Sprite using the newTexture
                newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

                // Assign the new Sprite to the SpriteRenderer
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
        


    }
    void Update () {
        if (GameManager.GameStatus==GameStatus.PLAYING)
            GetComponent<Rigidbody2D>().isKinematic = false;

        int count= Physics2D.OverlapCollider(PolygonInsideCup, LiquidFilter, res);        
        if (count > 0 && transform.GetChild(0).gameObject.active)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (count >= 30 && transform.GetChild(1).gameObject.active)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().DayNuoc(transform.position);
        }
    }

    private Texture2D LoadImage(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (System.IO.File.Exists(filePath))
        {
            byte[] textureBytes = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(textureBytes);
            return texture;
        }
        return null;
    }
}
