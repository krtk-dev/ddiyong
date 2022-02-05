using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StringEventTrigger : EventTrigger {

    public override void OnPointerDown(PointerEventData data)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().StringDown();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().StringClick(gameObject);
    }

}
