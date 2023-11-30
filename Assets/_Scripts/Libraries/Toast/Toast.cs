using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public enum Position { top, bottom};
    //static GameObject clone;

    public static void ShowMessage(string message, Position position = Position.top, float duration = 3)
    {
        GameObject toast = Resources.Load("Message") as GameObject;

        Transform container = toast.transform.GetChild(0);
        container.GetChild(0).gameObject.GetComponent<Text>().text = message;

        GameObject clone = Instantiate(toast);

        clone.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -200, 0), 0.5f);
        //Toast.Instance.StartCoroutine(Toast.Instance.SlideUp(duration));
        
        
        /*tween.SetDelay(5).OnComplete(()=> {
            DConsole.Log("Complete");
        });*/

            //.OnComplete(() =>
        //{
        //    //SetDelay(duration - 1);
        //    //.clone.transform.GetChild(0).GetComponent<RectTransform>()..DOAnchorPosY(200f, 0.5f);
        //});


        Destroy(clone, duration);
    }

    //IEnumerator SlideUp(float duration)
    //{
    //    yield return new WaitForSeconds(duration - 1);

    //    clone.transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 200, 0), 0.5f);
    //}
}
