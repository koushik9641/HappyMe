using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerScript : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    private int resolutionX;
    private int resolutionY;

    void Start()
    {
        //CanvasScaler c = GetComponent<CanvasScaler>();
        //c.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //c.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        ChangeCanvasScaler();

#if UNITY_EDITOR
        resolutionX = Screen.width;
        resolutionY = Screen.height;
#endif

        /* Device list upto 6/4/2020
         * 
         * List<VirtualScreen> screens = new List<VirtualScreen>();
        screens.Add(new VirtualScreen(1080, 1920, 480));
        screens.Add(new VirtualScreen(1080, 1920, 420));
        screens.Add(new VirtualScreen(720, 1280, 280));
        screens.Add(new VirtualScreen(720, 1280, 320));

        screens.Add(new VirtualScreen(1080, 2340, 420));
        screens.Add(new VirtualScreen(1080, 2340, 440));
        screens.Add(new VirtualScreen(1080, 2340, 480));
        screens.Add(new VirtualScreen(540, 960, 240));
        screens.Add(new VirtualScreen(720, 1560, 280));
        screens.Add(new VirtualScreen(1440, 2880, 560));
        screens.Add(new VirtualScreen(720, 1600, 320));
        screens.Add(new VirtualScreen(720, 1520, 320));
        screens.Add(new VirtualScreen(1080, 2160, 420));
        screens.Add(new VirtualScreen(1080, 2160, 480));
        screens.Add(new VirtualScreen(480, 854, 240));
        screens.Add(new VirtualScreen(720, 1440, 320));
        screens.Add(new VirtualScreen(1280, 720, 320));
        screens.Add(new VirtualScreen(1080, 2400, 420));
        screens.Add(new VirtualScreen(480, 800, 240));
        screens.Add(new VirtualScreen(720, 1339, 320));
        screens.Add(new VirtualScreen(720, 1339, 280));
        screens.Add(new VirtualScreen(1080, 2280, 440));
        screens.Add(new VirtualScreen(1080, 2280, 480));
        screens.Add(new VirtualScreen(1080, 1980, 480));
        screens.Add(new VirtualScreen(720, 1320, 320));
        screens.Add(new VirtualScreen(720, 1280, 320)); //phone

        //Tabs:
        screens.Add(new VirtualScreen(1280, 800, 213));
        screens.Add(new VirtualScreen(1200, 1920, 320));
        screens.Add(new VirtualScreen(800, 1280, 160));
        screens.Add(new VirtualScreen(768, 1024, 160));

        int i = 0;
        foreach (var item in screens)
        {
            if (item.width > item.height)
                DConsole.LogError("Width: "+ item.width +" is Greater");

            if (IsTablet(item.width, item.height, item.dpi))
                DConsole.LogWarning(i+" Tablet: "+item.width+" x "+item.height+" : "+item.dpi);
            else
                DConsole.Log(i +" Phone: " + item.height);

            i++;
        }*/
    }

    public void ChangeCanvasScaler()
    {
        if (IsTablet())
        {
            canvasScaler.matchWidthOrHeight = 1f;
            //DConsole.Log("Tablet");
        }
        else { 
            canvasScaler.matchWidthOrHeight = 0f;
            //DConsole.Log("Phone");
        }
    }

    public static bool IsTablet(int width = -1, int height = -1, float dpi = -1)
    {
        if (-1 == width)
            width = Screen.width;

        if (-1 == height)
            height = Screen.height;

        if (-1 == dpi)
            dpi = Screen.dpi;

        float ssw;
        if (width > height) { ssw = width; } else { ssw = height; }

        if (ssw < 800)
        {
            //DConsole.Log("Smaller than 800");
            return false;
        }

        //if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer){
        float screenWidth = width / dpi;
        float screenHeight = height / dpi;
        float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        if (size >= 6.5f)
        {
            //DConsole.Log("Greater than 6.5");
            return true;

        }
        //}

        return false;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (resolutionX == Screen.width && resolutionY == Screen.height) return;

        ChangeCanvasScaler();

        resolutionX = Screen.width;
        resolutionY = Screen.height;
    }
#endif
}

/*
class VirtualScreen {
    public int width;
    public int height;
    public int dpi;

    public VirtualScreen(int width, int height, int dpi)
    {
        this.width = width;
        this.height = height;
        this.dpi = dpi;
    }
}*/