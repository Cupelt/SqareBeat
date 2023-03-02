using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CheckMouseCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isActive = true;
    public bool isCurserOnObject = false;

    private void Update()
    {
        if (isCurserOnObject) { onHighlight(isActive); }
        else { onIdle(isActive); }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isCurserOnObject = true;
    }
    
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isCurserOnObject = false;
    }

    public void setActive(bool active)
    {
        isActive = active;
        StartCoroutine(onChangeActive());
    }

    public bool getActive()
    {
        return isActive;
    }
    

    public virtual IEnumerator onChangeActive() { yield return null; }
    public virtual void onHighlight(bool active) { }
    public virtual void onIdle(bool active) { }
}