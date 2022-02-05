using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstScreenManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerPrf;
    public SceneChanger sceneChanger;

    //debug
    //public Text debugText;

    //Audio
    public Image mySoundBtn;
    public Sprite soundOn;
    public Sprite soundOff;
    private bool isSoundOn;

    //setting
    public Image settingImage;
    public Sprite normalImage;
    public Sprite clickedImage;
    public Animator settingAnim;
    private bool isOpen = false;

    //ClearAll
    public GameObject ttl;
    public GameObject thxData;
    public GameObject DY;
    public GameObject THX;

    private int myBall = 1;

    private void OnPreCull()
    {
        if (PlayerPrefs.GetInt("Stage") > 80)
        {
            DY.SetActive(false);
            THX.SetActive(true);
            thxData.SetActive(true);
            int count = 0;
            bool first = true;
            for(int k = 1; k <= 80; k++)
            {
                if(PlayerPrefs.GetInt(k.ToString()) == 0)
                {
                    count++;
                    if(k == 80 && first) PlayerPrefs.SetInt("Perfactly", Random.Range(1, 80));
                }
                else
                {
                    if (first) PlayerPrefs.SetInt("Perfactly", k);
                }
            }
            ttl.GetComponent<Text>().text = "TOTAL DDIYONG " + count.ToString();
        }
    }

    private void Awake()
    {
        //player.GetComponent<FirstSceneBallCtrl>().myVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * player.GetComponent<FirstSceneBallCtrl>().speed;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 0)).normalized * player.GetComponent<FirstSceneBallCtrl>().speed;
        player.GetComponent<FirstSceneBallCtrl>().oldVel = player.GetComponent<Rigidbody2D>().velocity;

        if (!PlayerPrefs.HasKey("FirstLoad"))
        {
            PlayerPrefs.SetString("FirstLoad", "true");
            GameData.stageNum = 1;
            PlayerPrefs.SetInt("Stage", 1);
            PlayerPrefs.SetString("Mute", "False");
            //sceneChanger.Fade2Scene(2);
        }

        if(PlayerPrefs.GetString("Mute") == "True")
        {
            mySoundBtn.sprite = soundOff;
            AudioListener.pause = true;
        }
    }
    private void Update()
    {
        //if (PlayerPrefs.HasKey("product")) debugText.text = PlayerPrefs.GetString("product");
    }
    public void InstantiateBall()
    {
        GameObject obj;
        if (myBall < 5)
        {
            obj = Instantiate(playerPrf, new Vector2(Random.Range(-9f, 9f), Random.Range(-17f, 17f)), Quaternion.identity);
            //obj.GetComponent<FirstSceneBallCtrl>().myVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * obj.GetComponent<FirstSceneBallCtrl>().speed;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * obj.GetComponent<FirstSceneBallCtrl>().speed;
            obj.GetComponent<FirstSceneBallCtrl>().oldVel = obj.GetComponent<Rigidbody2D>().velocity;
        }
        myBall++;
    }
    public void SoundBtn()
    {
        AudioListener.pause = !AudioListener.pause;
        PlayerPrefs.SetString("Mute", AudioListener.pause.ToString());
        print(AudioListener.pause);

        if (AudioListener.pause)
        {
            mySoundBtn.sprite = soundOff;
        }
        else
        {
            mySoundBtn.sprite = soundOn;
        }
    }

    public void SettingBtn()
    {
        isOpen = !isOpen;
        settingAnim.SetBool("Open", isOpen);
        settingImage.sprite = isOpen ? clickedImage : normalImage;
    }
    public void Instar()
    {
        Application.OpenURL("https://www.instagram.com/koreanthinker/");
    }
    public void FaceBook()
    {
        Application.OpenURL("https://www.facebook.com/KoreanThinker-408391183302145/");
    }
}