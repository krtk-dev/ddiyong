using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryBoxCtrl : MonoBehaviour
{
    public GameObject image;
    public Animator anim;

    public void ChangeScale(float scale)
    {
        image.transform.localScale = Vector3.one * scale;
    }
    public void AnimOnOff(bool isOpen)
    {
        anim.SetBool("On", isOpen);
    }
}
