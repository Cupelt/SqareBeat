using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.cupelt.util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : CheckMouseCursor, IPointerClickHandler
{
    [HideInInspector] public bool readyActive = true;
    
    public float highlightedScale = 1.1f;
    public float clickScale = 1.05f;

    public float smoothSpeed = 8f;

    public bool nonAnimation = false; 

    public ColorObjects colorObjects;
    public UnityEvent onClick;

    public void OnPointerClick(PointerEventData PointerEventData)
    {
        if (!getActive()) return;
        
        onClick.Invoke();
    }

    public override IEnumerator onChangeActive()
    {
        readyActive = false;
        
        float toAlpha = getActive() ? toAlpha = 1f : toAlpha = 0.025f;
        float fromAlpha = getActive() ? fromAlpha = 0.025f : fromAlpha = 1f;

        float time = 0;
        while (time < 1)
        {
            fromAlpha = Mathf.Lerp(fromAlpha, toAlpha, Tweening.OutQuart(time));

            foreach (Image img in colorObjects.images)
            {
                Color color = img.color;
                color.a = fromAlpha;
                img.color = color;
            }
            
            foreach (Text text in colorObjects.texts)
            {
                Color color = text.color;
                color.a = fromAlpha;
                text.color = color;
            }

            time += Time.deltaTime;
            yield return null;
        }

        readyActive = true;
    }

    public override void onIdle(bool active)
    {
        if (!active || nonAnimation) return;
        
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * smoothSpeed);
    }

    public override void onHighlight(bool active)
    {
        if (!active || nonAnimation) return;
        float scale = Input.GetMouseButton(0) ? clickScale : highlightedScale;
    
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scale, Time.deltaTime * smoothSpeed);
    }

    [System.Serializable]
    public struct ColorObjects
    {
        public Image[] images;
        public Text[] texts;
    }
}
