using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Spine.Unity;

[System.Serializable]
public class LevelInfo: MonoBehaviour {
    public float ThreeStarLength;

    //public SkeletonAnimation spine;
    //public GameObject imgWatermelon;

    private void Start()
    {
    }

    //public void ShowAnimTree()
    //{
    //    imgWatermelon.SetActive(false);
    //    spine.gameObject.SetActive(true);
    //    spine.AnimationName = "Appear";
    //    spine.loop = false;

    //    var myAnimation = spine.Skeleton.Data.FindAnimation("Appear");
    //    float duration = myAnimation.Duration;
    //    Invoke("IdleSpine", duration);
    //}

    //void IdleSpine()
    //{
    //    spine.AnimationName = "Idle";
    //    spine.loop = true;
    //    spine.Initialize(true);
    //}
}
