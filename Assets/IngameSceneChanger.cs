using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameSceneChanger : MonoBehaviour {

    public Animator animator;
    public BannerCtrl banner;

    private int sceneNumber;

    public void Fade2Scene(int index)
    {   
        if (index <= 83) sceneNumber = index;
        else sceneNumber = 0;
        animator.SetTrigger("FadeOut");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().ChangeTheme(0);
        banner.DeleteBanner();
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void OnFadeInComplete()//수정
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().SceneChanger(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().stageIndex + 3); //싱글톤 값
    }
}
