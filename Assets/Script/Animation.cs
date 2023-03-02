using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using com.cupelt.util;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Animation : MonoBehaviour
{
    [HideInInspector] public bool readyAnimation = true;
    [HideInInspector] public bool active = false;
    
    public Ease animaton;
    
    public float offset;
    public float delay;
    
    public Vector3 movePos;

    private Vector3 originPos;


    private void Awake()
    {
        originPos = GetComponent<RectTransform>().anchoredPosition3D;
    }

    public void setActive(bool _active)
    {
        if (active == _active || readyAnimation == false) return;

        active = _active;
        StartCoroutine(startAnimation(active));
    }
    
    public virtual void setAlpha(float alpha) { }

    IEnumerator startAnimation(bool _active)
    {
        readyAnimation = false;
        RectTransform trans = gameObject.GetComponent<RectTransform>();

        float time = -offset;
        
        float reverse = _active ? 1f : -1f;
        float min = _active ? 0f : 1f;

        while (time / delay < 1)
        {
            time += Time.deltaTime;
            float fixedTime = time / delay;
            fixedTime = Tweening.fixedTime(fixedTime);
            trans.anchoredPosition3D = originPos + ((min + animaton(fixedTime) * reverse) * movePos) - (movePos * (reverse + min * 2));
            setAlpha(min + animaton(fixedTime) * reverse);
            yield return null;
        }

        readyAnimation = true;
    }
}
