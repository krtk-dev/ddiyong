using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public Animator animator;
    private int sceneNumber;

    public void Fade2Scene(int index)
    {
        if (index <= 83) sceneNumber = index;
        else sceneNumber = 0;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
