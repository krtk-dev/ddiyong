using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScaleCtrl : MonoBehaviour
{
    private const float targetWidth = 9.0f;
    private const float targetHeight = 16.0f;

    private void Start()
    {
        float myRatio = targetWidth / targetHeight;
        float windowRatio = (float)Screen.width / (float)Screen.height;
        float scale = windowRatio / myRatio;

        if (scale > 1f)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }
}
