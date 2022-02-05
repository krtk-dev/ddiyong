using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class StageBoxCtrl : MonoBehaviour
{
    public GameObject background;
    public Image image;
    public ProceduralImage backgroundImage;
    public ProceduralImage border;
    public Text text;
    public Color gray;

    public void ChangeScale(float scale)
    {
        background.transform.localScale = Vector3.one * scale;
    }
    public void ChangeInfo(Sprite sprite, Color color, string _text)
    {
        backgroundImage.color = color;
        border.color = color;
        image.sprite = sprite;
        text.text = _text;
        text.color = color;
        if (_text == "???") image.color = gray;
        else image.color = Color.white;
    }
}
