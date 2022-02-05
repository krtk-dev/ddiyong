using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour {

    public static SceneData myData;

    private void Awake()
    {
        myData = this;
    }


}
