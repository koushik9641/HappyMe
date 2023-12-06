using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerManager : MonoBehaviour
{
    public GameObject Board, StarBust, Confetti,confettiparticle;
    public Text txtTotalStar;
    public Text txtLevel;
    public int bounce = 10;
    public static int numStar = 0;
    bool clicked = false;

    private int adAfterGameCount = 2;


    private void Awake()
    {
        Transform parent = transform.parent;
        parent.GetChild(0).gameObject.SetActive(false);
        parent.GetChild(1).gameObject.SetActive(false);
        parent.GetChild(2).gameObject.SetActive(false);
        parent.GetChild(3).gameObject.SetActive(false);
        parent.GetChild(4).gameObject.SetActive(false);
        parent.GetChild(5).gameObject.SetActive(false);
        parent.GetChild(6).gameObject.SetActive(false);
        parent.GetChild(7).gameObject.SetActive(false);
        parent.GetChild(8).gameObject.SetActive(false);
        parent.GetChild(9).gameObject.SetActive(false);
        parent.GetChild(10).gameObject.SetActive(false);
        parent.GetChild(11).gameObject.SetActive(false);
        parent.GetChild(12).gameObject.SetActive(false);
    }

    IEnumerator Start()
    {
        Debug.Log("start...");
        txtTotalStar.text = (PlayerPrefs.GetInt("totalStar", 0) + numStar * bounce).ToString();
        PlayerPrefs.SetInt("totalStar", PlayerPrefs.GetInt("totalStar", 0) + numStar * bounce);
        if (Random.Range(0, 100) < 90)
        {
            //AdsManager.Instance.ShowInterstitial();
        }
        txtLevel.text = PlayerPrefs.GetInt("curLevel", 1).ToString();
        for (int i = 0; i < numStar; i++)
        {
            Debug.Log("explosion...");
            StartCoroutine(StarExplosion(i));
            yield return new WaitForSeconds(0.3f);
        }

        int gameCount = PlayerPrefs.GetInt("GamePlayed", 0);
        gameCount++;

        if (adAfterGameCount <= gameCount)
        {
            gameCount = 0;
            AdsManager.Instance.ShowInterstitial();
            Debug.Log("ShowInterstitial..................................");
        }
        PlayerPrefs.SetInt("GamePlayed", gameCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HomeClick();
        }
    }

    public void MoreAppsTwo()
    {
        //SceneManager.LoadScene("MainGame");

        //SceneTransition.Instance.LoadScene("MainGame", TransitionType.WaterLogo);
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Nexsa+studio");
    }

    public void PuzzleAds()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.nexsastudio.block_puzzle");
    }

    public void WordAds()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.nexsastudio.word_search");
    }

    public void ConnectAds()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.nexsastudio.word_connect");
    }

    IEnumerator StarExplosion(int i)
    {
        yield return new WaitForSeconds(2);
        //AudioManager.Instance.Play("star_explosion");
        GetComponents<AudioSource>()[0].Play();
        Debug.Log("explosion..." + Board.transform.GetChild(i).GetChild(0).gameObject);
        Board.transform.GetChild(i).GetChild(0).DOScale(3, 0.8f).SetEase(Ease.OutBounce);
        Destroy(Instantiate(StarBust, Board.transform.GetChild(i).position, Quaternion.identity), 3);
        if (i == 2)
        {
            yield return new WaitForSeconds(0.3f);
            //AudioManager.Instance.Play("congratulation");
            GetComponents<AudioSource>()[1].Play();
            Destroy(Instantiate(Confetti, Board.transform.position + Vector3.up * 1.9f, Quaternion.identity), 3);

        }
    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) < GameManager.totalLevel && !clicked)
        {
            PlayerPrefs.SetInt("curLevel", PlayerPrefs.GetInt("curLevel", 1) + 1);
            PlayAgain();
            clicked = true;
        }
        else if (!clicked)
        {
            ChooseLevel();
        }
    }

    public void HomeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
    public void PlayAgain()
    {
        SceneTransition.Instance.LoadScene("MainGame", TransitionType.FadeToBlack);
    }
    public void ChooseLevel()
    {
        SceneTransition.Instance.LoadScene("ChooseLevel", TransitionType.WaterLogo);
    }
}
