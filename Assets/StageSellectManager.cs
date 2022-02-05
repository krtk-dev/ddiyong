using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSellectManager : MonoBehaviour
{
    public CategoryCtrl category;
    public StageScrollCtrl stage;
    public SceneChanger sceneChanger;

    private void Awake()
    {
        PlayerPrefs.SetInt("LastScene", 2);
    }
    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape)) sceneChanger.Fade2Scene(0);
#endif
    }
    public void ChangeTargetStage()
    {
        if (stage.stageNum == 0) GameData.stageNum = (category.categoryNum - 1) * 20 + 1;
        else GameData.stageNum = (category.categoryNum - 1) * 20 + stage.stageNum;
    }
}
