using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SideBar : MonoBehaviour
{
    private enum State { fadeIn, fadeOut }
    public Vector3 hidePos = new Vector3(0f, 600f, 0f);
    public Vector3 appearPos = new Vector3(0f, 500f, 0f);

    public RectTransform[] rectTrans;
    public int[] mulitple;

    public bool isOpend = false;
    public bool isSlected = false;
    public float delay = 0.125f;

    private void Update()
    {
        if (!isOpend)
            return;

        bool check = false;
        for (int i = 0; i < rectTrans.Length; i++)
        {
            RectTransform trans = rectTrans[i];
            Vector3 WorldPoint = Util.getWorldPoint(trans);
            if (Util.CheckMousePos(new Vector3(WorldPoint.x, WorldPoint.y, 0), new Vector3(trans.sizeDelta.x / 2, trans.sizeDelta.y / 2, 0), true))
            {
                check = true;
                break;
            }
        }
            
        if (check)
        {
            isSlected = true;
        }
        else
        {
            isSlected = false;
        }
    }

    public void fadeInSideBar()
    {
        isOpend = true;

        for (int i = 0; i < rectTrans.Length; i++)
        {
            Vector3 smoothedPos = Vector3.Lerp(rectTrans[i].anchoredPosition3D, appearPos * mulitple[i], delay);
            rectTrans[i].anchoredPosition3D = smoothedPos;
        }
    }


    public void fadeOutSideBar()
    {
        isOpend = false;

        for (int i = 0; i < rectTrans.Length; i++)
        {
            Vector3 smoothedPos = Vector3.Lerp(rectTrans[i].anchoredPosition3D, hidePos * mulitple[i], delay);
            rectTrans[i].anchoredPosition3D = smoothedPos;
        }
    }
}