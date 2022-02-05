using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainCtrl : MonoBehaviour
{
    public CategoryCtrl category;
    public StageScrollCtrl stage;
    
    public void StartFadeIn()
    {
        stage.ChangeChepter(category.categoryNum);
    }
    public void StartFadein2()
    {
        int chepter = ((PlayerPrefs.GetInt("Stage") - 1) / 20) + 1;
        int stageNum = ((PlayerPrefs.GetInt("Stage") - 1) % 20) + 1;
        stage.ChangeChepter(chepter, stageNum);
    }
}
