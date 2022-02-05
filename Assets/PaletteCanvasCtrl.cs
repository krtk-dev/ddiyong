using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteCanvasCtrl : MonoBehaviour {

    public PaletteCtrl pc;

    public void AnimDone()
    {
        pc.StartMode2Anim();
    }
}
