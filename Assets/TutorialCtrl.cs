using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialCtrl : MonoBehaviour
{
    public Text content;

    private Animator myAnim;
    private int myAnimNum;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        myAnimNum = 0;
    }

    public void Clicked()
    {
        if(myAnimNum != 5 && myAnimNum != 7)
        {
            myAnimNum++;
            myAnim.SetInteger("Num", myAnimNum);
        }
    }
    public void PointClicked()
    {
        myAnimNum++;
        myAnim.SetInteger("Num", myAnimNum);
        if (myAnimNum == 6) GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().InstantiateString(0);
        else if (myAnimNum == 8) GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().PlayButton();
    }
    public void ReStart()
    {
        myAnimNum = 0;
        myAnim.SetInteger("Num", myAnimNum);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeText(string _text)
    {
        content.text = _text;
    }
}
