using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartOptionTrigger : EventTrigger {

    public override void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().PlayButton();
    }
}
