using System;
using System.Collections;
using System.Collections.Generic;
using com.cupelt.util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance
    {
        get
        {
            return instance;
        }
    }

    public MenuController menu;
    public CustomButton icon;
    public GameObject mainText;

    public RectTransform optionObject;
    public RectTransform playListObject;

    public Stack<MenuState> nowState = new Stack<MenuState>();
    public List<MenuState> readyQueue = new List<MenuState>();
    private bool readyNextAnimation = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && readyNextAnimation && nowState.Count != 0 && icon.readyActive)
        {
            removeState();
            if (nowState.Count == 0)
            {
                icon.setActive(true);
                mainText.SetActive(true);
            }
        }

        readyNextAnimation = (readyQueue.Count == 0);
    }

    #region - preset Func -

    public void openMenu() { addState(new Menu()); }
    public void openOption() { addState(new OptionMenu()); }
    public void openSinglePlayer() { addState(new SinglePlayer()); }

    #endregion

    public void addState(MenuState state)
    {
        if (readyNextAnimation && icon.readyActive)
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
