using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongInfo : MonoBehaviour
{
    public Vector3 movePos;
    public SimpleButton btn;

    public float smoothValue;

    private Vector3 originPos;

    private void Awake()
    {
        originPos = GetComponent<RectTransform>().anchoredPosition3D;
    }

    void Update()
    {
        RectTransform trans = GetComponent<RectTransform>();
        if (btn.isHighlighted)
        {
            Vector3 smoothedPos = Vector3.Lerp(trans.anchoredPosition3D, originPos + movePos, smoothValue);
            trans.anchoredPosition3D = smoothedPos;
        }
        else
        {
            Vector3 smoothedPos = Vector3.Lerp(trans.anchoredPosition3D, originPos, smoothValue);
            trans.anchoredPosition3D = smoothedPos;
        }
    }
}