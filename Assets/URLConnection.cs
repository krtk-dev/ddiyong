using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLConnection : MonoBehaviour
{
    public void Connect2URL(string url)
    {
        Application.OpenURL(url);
    }
}
