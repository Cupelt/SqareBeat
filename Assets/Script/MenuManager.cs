using System;
using System.Collections;
using System.Collections.Generic;
using com.cupelt.util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public readonly Stack<MenuState> nowState = new Stack<MenuState>();
    public readonly List<MenuState> readyQueue = new List<MenuState>();
    private bool _readyNextAnimation = true;

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

        _readyNextAnimation = (readyQueue.Count == 0);
    }

    #region - preset Func -

    public void openMenu() { addState(new Menu()); }
    public void openOption() { addState(new OptionMenu()); }
    public void openSinglePlayer() { addState(new SinglePlayer()); }

    #endregion

    public void addState(MenuState State)
    {
        if (_readyNextAnimation && icon.readyActive)
        {
            if (nowState.Count == 0) icon.setActive(false);
            else StartCoroutine(nowState.Peek().resetState());
            
            nowState.Push(State);
            StartCoroutine(State.initState());
        }
    }
    
    private void removeState()
    {
        MenuState state = nowState.Pop();
        StartCoroutine(state.resetState());
        if (nowState.Count != 0) StartCoroutine(nowState.Peek().initState());
    }
}
