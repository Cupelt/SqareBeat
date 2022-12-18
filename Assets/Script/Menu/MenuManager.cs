using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static Transform mainCanvas;
    public Transform canvas;

    public MenuSelectBar SelectBar;

    public SideBar sideBar;
    public SideBar songList;

    public static int widith = 1920;
    public static int height = 1080;

    public static bool isPopUp = false;
    public bool isListEnable = false;

    void Update()
    {
        mainCanvas = canvas;

        Vector3 mousePos = Util.getMousePos();

        if (Util.CheckMousePos(new Vector3(325f, 470f, 0), new Vector3(325f, 330f, 0)))
        {
            SelectBar.nowSel = 7 - (int)(mousePos.y - 15) / 100;
        }

        if (!Util.CheckMousePos(Vector3.zero, new Vector3(widith * 2f, 400f, 0), true) || (isListEnable && songList.isSlected))
        {
            sideBar.fadeInSideBar();
        }
        else 
        {
            sideBar.fadeOutSideBar();
            if (isListEnable)
                isListEnable = false; 
        }
    
        if (isListEnable)
            songList.fadeInSideBar();
        else
            songList.fadeOutSideBar();
    }

    public void ToggleSongList()
    {
        isListEnable = !isListEnable;
    }
}
