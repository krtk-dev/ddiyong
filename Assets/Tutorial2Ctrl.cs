using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial2Ctrl : MonoBehaviour
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
        myAnimNum++;
        myAnim.SetInteger("Num", myAnimNum);
        if (myAnimNum == 1) GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().BarAnimCtrl(1);
        else if(myAnimNum == 2) GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().BarAnimCtrl(0);
    }
    public void ChangeText(string _text)
    {
        content.text = _text;
    }
}
