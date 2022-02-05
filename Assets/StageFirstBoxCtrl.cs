using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageFirstBoxCtrl : MonoBehaviour
{
    public Text texts;

    public void ChangeFirstBoxInfo(int scores)
    {
        if (scores == 20) texts.text = "CLEAR";
        else if (scores != -1) texts.text = "DDIYONG : " + scores.ToString();
        else texts.text = "";
    }
}
