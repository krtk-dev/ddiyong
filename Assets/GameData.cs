using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public static int stageNum = 0;
    //Ads
#if UNITY_IOS
    public const string gameID = "3012328";
#elif UNITY_ANDROID
    public const string gameID = "3012329";
#endif
    //public const string gameID = "1111111";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("ADS")) PlayerPrefs.SetString("ADS", "false");
        Advertisement.Initialize(gameID);
        instance = this;
        PlayerPrefs.SetInt("LastScene", 0);
    }
    public void PlayBtn()
    {
        if(PlayerPrefs.HasKey("Stage"))
        {
            if(PlayerPrefs.GetInt("Stage") > 80)
            {
                stageNum = PlayerPrefs.GetInt("Perfactly");
            }
            else stageNum = PlayerPrefs.GetInt("Stage");
        }
        else
        {
            stageNum = 1;

            PlayerPrefs.SetInt("Stage", 1);
        }
    }
    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayBtn();
    }
}
