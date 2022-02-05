using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringBarCtrl : MonoBehaviour {

    public Animator anim;
    public Animator paletteAnim;

	public void StringOpenComplete()
    {
        paletteAnim.SetBool("isOpen", true);
    }
    public void StringCloseStart()
    {
        paletteAnim.SetBool("isOpen", false);
    }
}
