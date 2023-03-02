using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionUI
{
    public List<OptionUIStyle> ui;
    public enum UiType{ Header, Selection, Toggle, Button }

    public OptionUI()
    {
        this.ui = new List<OptionUIStyle>();
    }

    #region - Header Functions
    public void AddHeader(string key, bool isSmall = false)
    {
        ui.Add(new Header(key, isSmall));
    }
    #endregion

    #region - Selection Functions
    public void AddSelection(string key, bool allowDisable, UnityAction prevFunc, UnityAction nextFunc, string valueName)
    {
        string[] sel;
        if (allowDisable) sel = new string[] {
            "general.active.disable",
            "general.quality.low",
            "general.quality.middle",
            "general.quality.high",
        };
        else sel = new string[] {
            "general.quality.low",
            "general.quality.middle",
            "general.quality.high",
        };

        AddSelection($"{key}.title", $"{key}.description", sel, prevFunc, nextFunc, valueName);
    }

    public void AddSelection(string key, string[] selectionKey, UnityAction prevFunc, UnityAction nextFunc, string valueName)
    {
        for (int i = 0; i < selectionKey.Length; i++) selectionKey[i] = $"{key}.selection.{selectionKey[i]}";
        AddSelection($"{key}.title", $"{key}.description", selectionKey, prevFunc, nextFunc, valueName);
    }

    public void AddSelection(string key, string descriptionKey, string[] selectionKey, UnityAction prevFunc, UnityAction nextFunc, string valueName)
    {
        ui.Add(new SelectionUI(key, descriptionKey, selectionKey, prevFunc, nextFunc, valueName));
    }
    #endregion

    #region - Toggle Functions
    public void AddToggle(string key, string descriptionKey, UnityAction<bool> setActiveFunc, string valueName)
    {
        ui.Add(new ToggleUI(key, descriptionKey, setActiveFunc, valueName));
    }
    public void AddToggle(string key, UnityAction<bool> setActiveFunc, string valueName)
    {
        ui.Add(new ToggleUI($"{key}.title", $"{key}.description", setActiveFunc, valueName));
    }
    #endregion

    #region - Button Functions
    public void AddButton(ButtonUI.ButtonObject[] buttons) => ui.Add(new ButtonUI(buttons));
    public void AddButton(string key, ButtonUI.ButtonObject[] buttons) => ui.Add(new ButtonUI(buttons));
    #endregion

    public interface OptionUIStyle 
    {
        UiType getStructType();
    }

    public struct Header : OptionUIStyle
    {
        public string _key;
        public bool _isSmall;

        public Header(string key, bool isSmall)
        {
            _key = key;
            _isSmall = isSmall;
        }

        public UiType getStructType()
        {
            return UiType.Header;
        }
    }
    public struct SelectionUI : OptionUIStyle
    {
        public string _key;
        public string _descriptionKey;

        public string[] selectionKeys;

        public UnityAction prev;
        public UnityAction next;

        public string value;

        public SelectionUI(string key, string descriptionKey, string[] selectionKey, UnityAction prevFunc, UnityAction nextFunc, string valueName)
        {
            _key = key;
            _descriptionKey = descriptionKey;

            selectionKeys = selectionKey;

            prev = prevFunc;
            next = nextFunc;

            value = valueName;
    }

        public UiType getStructType()
        {
            return UiType.Selection;
        }
    }
    public struct ToggleUI : OptionUIStyle
    {
        public string _key;
        public string _descriptionKey;

        public UnityAction<bool> _setActiveFunc;

        public string value;

        public ToggleUI(string key, string descriptionKey, UnityAction<bool> setActiveFunc, string valueName)
        {
            _key = key;
            _descriptionKey = descriptionKey;

            _setActiveFunc = setActiveFunc;

            value = valueName;
        }

        public UiType getStructType()
        {
            return UiType.Toggle;
        }
    }
    public struct ButtonUI : OptionUIStyle
    {

        public ButtonObject[] _buttons;

        public ButtonUI(ButtonObject[] buttons)
        {

            _buttons = buttons;
        }

        public UiType getStructType()
        {
            return UiType.Button;
        }

        public struct ButtonObject
        {
            public UnityAction action;
            public string text;

            public ButtonObject(string _text, UnityAction _action)
            {
                text = _text;
                action = _action;
            }
        }
    }
}
