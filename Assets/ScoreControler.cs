using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Advertisements;

public class ScoreControler : MonoBehaviour {

    public Animator myAnim;
    public Text stage;
    public Text score;
    public Text bounce;
    public Text record;
    public GameObject newRecord;
    public BannerCtrl banner;
    public AudioSource myAudio;
    public AudioClip DY;
    public AudioClip over;
    public Color yellow;
    public Color red;

    private int myBounce = 11;
    private bool isNextScene;
    private bool isAds;
    private int myScore;

	public void FadeOutDone()
    {
        myBounce = GameObject.FindGameObjectWithTag("Player").GetComponent<BallControler>().myBounce;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().EndScene();
        if (PlayerPrefs.GetInt("Stage") <= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex) PlayerPrefs.SetInt("Stage", GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex + 1);

        if (myBounce <= GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().bounceLimit.x) myScore = 0;
        else if (myBounce <= GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().bounceLimit.y) myScore = 1;
        else if (myBounce <= GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().bounceLimit.z) myScore = 2;
        else myScore = 3;
        newRecord.SetActive(false);
        if (PlayerPrefs.HasKey(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex.ToString()))
        {
            if (myScore < PlayerPrefs.GetInt(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex.ToString()))
            {
                PlayerPrefs.SetInt(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex.ToString(), myScore);
                if (myScore == 0)
                {
                    newRecord.SetActive(true);
                    record.text = "NEW RECORD";
                    record.color = yellow;
                }
            }
            else if(myScore != 0)
            {
                newRecord.SetActive(true);
                record.text = "TRY AGAIN";
                record.color = Color.red;
            }
        }
        else
        {
            PlayerPrefs.SetInt(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex.ToString(), myScore);
            if (myScore == 0)
            {
                newRecord.SetActive(true);
                record.text = "NEW RECORD";
                record.color = yellow;
            }
            else
            {
                newRecord.SetActive(true);
                record.text = "TRY AGAIN";
                record.color = Color.red;
            }
        }

        stage.text = "LEVEL " + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex;
        bounce.text = ": " + myBounce;

        if (myScore == 0) score.color = Color.black;
        else score.color = Color.red;
        score.text = Score2String(myScore);

        banner.BannerLoad();
    }
    public string Score2String(int score)
    {
        switch (score)
        {
            case 0: return "DDIYONG";
            //case 1: return "GREAT";
            //case 2: return "NOT BAD";
            default: return "OVER BOUNCE";
        }

    }
    public void FadeInDone()
    {
        if (!isNextScene)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().ResetButton();
        }
        else if (isAds) GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().PlayAds2Next();
        else GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().NextScene();
    }

    public void NextSceneButton()
    {
        banner.DestroyBanner();
        isNextScene = true;
        myAnim.SetBool("isOpen", false);
        isAds = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageManager>().isAds;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().UnLoadScene();
    }
    public void RestartButton()
    {
        banner.DestroyBanner();
        isNextScene = false;
        myAnim.SetBool("isOpen", false);
        GameObject.FindGameObjectWithTag("Popup").GetComponent<Animator>().SetBool("isClear", false);
    }
    public void MenuButton()
    {
        banner.DestroyBanner();
    }
    public void AudioPlay()
    {
        if (myScore == 0) myAudio.clip = DY;
        else myAudio.clip = over;
        myAudio.Play();
    }
}
