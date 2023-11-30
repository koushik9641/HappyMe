using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//22/02/2021

public enum AlertDirection { Left, Right, Top, Down }

public class Alert
{
    public float openAnimationTime = 0.5f;
    public float closeAnimationTime = 0.3f;
    public static bool isAlertShown = false;

    public delegate void CallbackDelegate(bool isPause);
    CallbackDelegate theDelegate;

    public delegate void ButtonDelegate();

    public ButtonDelegate backButtonDelegate;

    private AlertVar alertVar;

    public static List<Alert> alertStack = new List<Alert>();

    public Alert(string title, string description, int backButtonCloseIndex = 1, CallbackDelegate commonCallback = null, Dictionary<string, ButtonDelegate> buttons = null, AlertDirection alertOpenDirection = AlertDirection.Left)
    {
        isAlertShown = true;
        alertStack.Add(this);

        this.theDelegate = commonCallback;

        GameObject prefab = Resources.Load("Alert") as GameObject;

        GameObject alert = MonoBehaviour.Instantiate(prefab);
        alert.transform.SetAsLastSibling();

        alertVar = alert.GetComponent<AlertVar>();
        alertVar.alert = this;
        alertVar.textTitle.text = title;
        alertVar.textDescription.text = description;
        alertVar.alertWidth = alertVar.alertBox.rect.width;
        alertVar.alertHeight = alertVar.alertBox.rect.height;

        ResetPosition(alertOpenDirection);

        if (buttons.Count > 0)
        {
            int buttonAdded = alertVar.buttonHolder.transform.childCount;
            if (buttonAdded == 0)
            {
                DConsole.LogError("Button Holder should have at least a Button to use Button Delegate!");
            }

            int i = 0;
            foreach (var button in buttons)
            {
                if (i == 2)
                {
                    DConsole.LogWarning("Only Two buttons can be added in a Alert!");
                    break;
                }

                GameObject gb;
                if (i < buttonAdded)
                {
                    gb = alertVar.buttonHolder.transform.GetChild(i).gameObject;
                }
                else
                {
                    /*gb = new GameObject();
                    gb.AddComponent<RectTransform>();
                    gb.AddComponent<Image>();
                    gb.AddComponent<Button>();

                    GameObject gb2 = new GameObject();
                    gb2.AddComponent<Text>();
                    gb2.transform.SetParent(gb.transform);*/

                    gb = MonoBehaviour.Instantiate(alertVar.buttonHolder.transform.GetChild(0).gameObject);
                }
                
                Button newButton = gb.GetComponent<Button>();
                newButton.onClick.RemoveAllListeners();
                newButton.onClick.AddListener(() => OnButtonClick(button.Value));
                newButton.GetComponentInChildren<Text>().text = button.Key;
                gb.transform.SetParent(alertVar.buttonHolder.transform, false);

                if (backButtonCloseIndex == i)
                    backButtonDelegate = button.Value;

                i++;
            }
        }

        Callback(true);
        AudioManager.Click();
        alertVar.panelAlert.SetActive(true);

        alertVar.alertBox.DOLocalMove(Vector3.zero, openAnimationTime);
    }

    public void OnButtonClick(ButtonDelegate newDelegate)
    {
        if (alertStack.Count > 0)
            alertStack.RemoveAt(alertStack.Count - 1);

        if (alertStack.Count <= 0)
        {
            isAlertShown = false;

            /*if (Config.isDebug)
                DConsole.Log("All alert has been closed!");*/
        }

        backButtonDelegate = null;

        AudioManager.Click();

        Vector2 pos = new Vector2(0f, alertVar.alertHeight);
        if (alertVar.alertOpenDirection == AlertDirection.Left)
            pos = new Vector2(alertVar.alertWidth, 0f);
        else if (alertVar.alertOpenDirection == AlertDirection.Right)
            pos = new Vector2(alertVar.alertWidth * -1, 0f);
        else if (alertVar.alertOpenDirection == AlertDirection.Top)
            pos = new Vector2(0f, alertVar.alertHeight * -1);

        alertVar.alertBox.DOLocalMove(pos, closeAnimationTime).OnComplete(() => {
            alertVar.panelAlert.SetActive(false);
            newDelegate?.Invoke();
            Callback(false);
            MonoBehaviour.Destroy(alertVar.gameObject);
        });
    }

    public void Callback(bool flag)
    {
        theDelegate?.Invoke(flag);
    }

    // Copied from Popup.cs
    private void ResetPosition(AlertDirection alertDirection)
    {
        if (alertDirection == AlertDirection.Left)
            alertVar.alertBox.DOLocalMove(new Vector2(alertVar.alertWidth * -1, 0f), 0f);
        else if (alertDirection == AlertDirection.Right)
            alertVar.alertBox.DOLocalMove(new Vector2(alertVar.alertWidth, 0f), 0f);
        else if (alertDirection == AlertDirection.Top)
            alertVar.alertBox.DOLocalMove(new Vector2(0f, alertVar.alertHeight), 0f);
        else if (alertDirection == AlertDirection.Down)
            alertVar.alertBox.DOLocalMove(new Vector2(0f, alertVar.alertHeight * -1), 0f);
    }
}