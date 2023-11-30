using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkHelper : MonoBehaviour
{

    string subject = "Hey I am playing this awesome new game called 'Happy Me', Do give it try and enjoy.";
    string body = "Hey I am playing this awesome new game called 'Happy Me', Do give it try and enjoy. " + "https://play.google.com/store/apps/details?id=com.nicogames.happy_me";

    public GameObject panel;

    public void Openpanel()
    {
        panel.SetActive(true);
    }

    public void Closepanel()
    {
        panel.SetActive(false);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Nico+Game+Studio");
    }

    public void RateApp()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }

    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/gaming/nexsagames");
    }

    public void Instagram()
    {
        Application.OpenURL("https://www.facebook.com/gaming/nexsagames");
    }

    public void OnAndroidTextSharingClick()
    {
        //FindObjectOfType<AudioManager>().Play("Enter");
        StartCoroutine(ShareAndroidText());
    }

    IEnumerator ShareAndroidText()
    {
        yield return new WaitForEndOfFrame();
        //execute the below lines if being run on a Android device
        //Reference of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Reference of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "TITLE");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
    }
}
