using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaletteStringEventTrigger : EventTrigger {

    private bool isPointerOnGameObject = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isDown", true);
        isPointerOnGameObject = true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isDown", true);
        isPointerOnGameObject = true;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isDown", false);
        isPointerOnGameObject = false;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if(isPointerOnGameObject && GetComponent<PaletteStringTextCtrl>().myText.text != "0")
        {
            int stringNum = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().StringObject2StringNumber(gameObject);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().InstantiateString(stringNum);
        }
        GetComponent<Animator>().SetBool("isDown", false);
    }
}
