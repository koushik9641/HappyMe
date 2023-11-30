using UnityEngine;
using UnityEngine.UI;

//22/02/2021

public class AlertVar : MonoBehaviour
{
    public GameObject panelAlert;
    public RectTransform alertBox;
    public Text textTitle;
    public Text textDescription;
    public GameObject buttonHolder;

    public Alert alert;
    [HideInInspector] public AlertDirection alertOpenDirection;
    [HideInInspector] public float alertWidth;
    [HideInInspector] public float alertHeight;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Alert.alertStack[Alert.alertStack.Count - 1] == alert)
            alert.OnButtonClick(alert.backButtonDelegate);
    }
}
