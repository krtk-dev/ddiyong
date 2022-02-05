using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteStringTextCtrl : MonoBehaviour {

    public Text myText;

    public void ChangeValue(int n)
    {
        myText.text = n.ToString();
    }

}
