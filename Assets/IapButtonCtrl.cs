using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IapButtonCtrl : MonoBehaviour
{
    public void RemoveAds()
    {
        PlayerPrefs.SetString("ADS", "true");
    }

}
