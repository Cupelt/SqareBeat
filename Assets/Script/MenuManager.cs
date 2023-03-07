using System;
using System.Collections;
using System.Collections.Generic;
using com.cupelt.util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public MenuController menu;
    public CustomButton icon;
    public GameObject mainText;

    public RectTransform optionObject;
    public RectTransform playListObject;

    public ScrollRect mapsScrollRect;

    public readonly Stack<MenuState> nowState = new Stack<MenuState>();
    public readonly List<MenuState> readyQueue = new List<MenuState>();
    private bool _readyNextAnimation = true;

    private float _keyCool = 0;

    private void Awake()
    
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _readyNextAnimation && nowState.Count != 0 && icon.readyActive)
        {
            removeState();
            if (nowState.Count == 0)
            {
                icon.setActive(true);
                mainText.SetActive(true);
            }
        }

        if (nowState.Count > 0 && nowState.Peek() is SinglePlayer)
        {
            _keyCool += Time.deltaTime;
            
            AudioManager audioManager = AudioManager.Instance;
            
            Vector3 scrollPos = mapsScrollRect.content.anchoredPosition3D;
            float top = -scrollPos.y - 150f;
            float bottom = -scrollPos.y - mapsScrollRect.viewport.rect.height + 150;
            if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKey(KeyCode.UpArrow) && _keyCool > 0.1f))
            {
                audioManager.setTrack(audioManager.getTrackbyNowBeatMap(-1));
                _keyCool = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKey(KeyCode.DownArrow) && _keyCool > 0.1f))
            {
                audioManager.setTrack(audioManager.getTrackbyNowBeatMap(1));
                _keyCool = 0f;
            }
            
            if (MusicSelector.nowSel.GetComponent<RectTransform>().anchoredPosition3D.y > top && _keyCool < 1f && 
                0 < mapsScrollRect.content.anchoredPosition3D.y)
            {
                Vector3 anchoredPosition3D = mapsScrollRect.content.anchoredPosition3D;
                Vector3 fixedPos = anchoredPosition3D;
                fixedPos.y = -1 * (MusicSelector.nowSel.GetComponent<RectTransform>().anchoredPosition3D.y + 150);
                mapsScrollRect.content.anchoredPosition3D = Vector3.Lerp(anchoredPosition3D, fixedPos, Time.deltaTime * 8f);
            } 
            else if (MusicSelector.nowSel.GetComponent<RectTransform>().anchoredPosition3D.y < bottom && _keyCool < 1f && 
                     mapsScrollRect.content.rect.height - mapsScrollRect.viewport.rect.height > mapsScrollRect.content.anchoredPosition3D.y)
            {
                Vector3 anchoredPosition3D = mapsScrollRect.content.anchoredPosition3D;
                Vector3 fixedPos = anchoredPosition3D;
                fixedPos.y = -1 * (MusicSelector.nowSel.GetComponent<RectTransform>().anchoredPosition3D.y + mapsScrollRect.viewport.rect.height - 150);
                mapsScrollRect.content.anchoredPosition3D = Vector3.Lerp(anchoredPosition3D, fixedPos, Time.deltaTime * 8f);
            }
        }

        _readyNextAnimation = (readyQueue.Count == 0);
    }

    #region - preset Func -

    public void openMenu() { addState(new Menu()); }
    public void openOption() { addState(new OptionMenu()); }
    public void openSinglePlayer() { addState(new SinglePlayer()); }

    #endregion

    public void addState(MenuState state)
    {
        if (_readyNextAnimation && icon.readyActive)
        {
            if (nowState.Count == 0) icon.setActive(false);
            else StartCoroutine(nowState.Peek().resetState());
            
            nowState.Push(state);
            StartCoroutine(state.initState());
        }
    }
    
    private void removeState()
    {
        MenuState state = nowState.Pop();
        StartCoroutine(state.resetState());
        if (nowState.Count != 0) StartCoroutine(nowState.Peek().initState());
    }
}
