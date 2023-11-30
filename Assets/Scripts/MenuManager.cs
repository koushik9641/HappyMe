using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Material WaterColor;
    public Image BackGround;
    public GameObject droplet;
    public Button btnAudio;
    public Sprite audioOn, audioOff;
    // Use this for initialization
    private void Awake()
    {
        if (PlayerPrefs.HasKey("color"))
        {
            WaterColor.color = ColorFromHex(PlayerPrefs.GetString("color"));
        }
        else
        {
            WaterColor.color = ColorFromHex("#28BEF3");
        }
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
            BackGround.sprite = newSprite;
        }
    }
    void Start () {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("audio", 1) == 1)
        {
            btnAudio.GetComponent<Image>().sprite = audioOn;
            AudioListener.volume = 1;
        }
        else
        {
            btnAudio.GetComponent<Image>().sprite = audioOff;
            AudioListener.volume = 0;
        }
        
        SceneTransition.Instance.Out();
        StartCoroutine(waterFall());

        
    }
    IEnumerator waterFall()
    {
        for (int i = 0; i < 35; i++)
        {
            GameObject obj = Instantiate(droplet,new Vector3(-0.5f,-0.5f,0), Quaternion.identity, transform);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 0.3f)*5);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void AudioClick()
    {
        PlayerPrefs.SetInt("audio", -PlayerPrefs.GetInt("audio", 1));
        if (PlayerPrefs.GetInt("audio", 1) == 1)
        {
            btnAudio.GetComponent<Image>().sprite = audioOn;
            AudioListener.volume = 1;
        }
        else
        {
            btnAudio.GetComponent<Image>().sprite = audioOff;
            AudioListener.volume = 0;
        }
    }
    public void StartClick()
    {
        //SceneManager.LoadScene("MainGame");
        
        SceneTransition.Instance.LoadScene("MainGame",TransitionType.WaterLogo);
    }
    public void MoreApps()
    {
        //SceneManager.LoadScene("MainGame");

        //SceneTransition.Instance.LoadScene("MainGame", TransitionType.WaterLogo);
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Nexsa+Studio");
    }


    public void ChooseGlassClick()
    {
        SceneTransition.Instance.LoadScene("Choose", TransitionType.WaterLogo);
    }

    public void ClearAllData()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);
        PlayerPrefs.DeleteAll();
    }

    Color ColorFromHex(string hex)
    {
        Color color = Color.white; // Default color

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning("Invalid hex color code. It should be in the format #RRGGBB or #AARRGGBB.");
            return Color.white;
        }
    }
}
