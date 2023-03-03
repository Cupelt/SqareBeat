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
    public void addHeader(string Key, bool IsSmall = false)
    {
        ui.Add(new Header(Key, IsSmall));
    }
    #endregion

    #region - Selection Functions
    public void addSelection(string Key, bool AllowDisable, UnityAction PrevFunc, UnityAction NextFunc, string ValueName)
    {
        string[] sel;
        if (AllowDisable) sel = new string[] {
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

        addSelection($"{Key}.title", $"{Key}.description", sel, PrevFunc, NextFunc, ValueName);
    }

    public void addSelection(string Key, string[] SelectionKey, UnityAction PrevFunc, UnityAction NextFunc, string ValueName)
    {
        for (int i = 0; i < SelectionKey.Length; i++) SelectionKey[i] = $"{Key}.selection.{SelectionKey[i]}";
        addSelection($"{Key}.title", $"{Key}.description", SelectionKey, PrevFunc, NextFunc, ValueName);
    }

    public void addSelection(string Key, string DescriptionKey, string[] SelectionKey, UnityAction PrevFunc, UnityAction NextFunc, string ValueName)
    {
        ui.Add(new SelectionUI(Key, DescriptionKey, SelectionKey, PrevFunc, NextFunc, ValueName));
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
        public string key;
        public bool isSmall;

        public Header(string Key, bool IsSmall)
        {
            this.key = Key;
            this.isSmall = IsSmall;
        }

        public UiType getStructType()
        {
            return UiType.Header;
        }
    }
    public struct SelectionUI : OptionUIStyle
    {
        public string key;
        public string descriptionKey;

        public string[] selectionKeys;

        public UnityAction prev;
        public UnityAction next;

        public string value;

        public SelectionUI(string Key, string DescriptionKey, string[] SelectionKey, UnityAction PrevFunc, UnityAction NextFunc, string ValueName)
        {
            this.key = Key;
            this.descriptionKey = DescriptionKey;

            selectionKeys = SelectionKey;

            prev = PrevFunc;
            next = NextFunc;

            value = ValueName;
    }

        public UiType getStructType()
        {
            return UiType.Selection;
        }
    }
    public struct ToggleUI : OptionUIStyle
    {
        public string key;
        public string descriptionKey;

        public UnityAction<bool> setActiveFunc;

        public string value;

        public ToggleUI(string Key, string DescriptionKey, UnityAction<bool> SetActiveFunc, string ValueName)
        {
            this.key = Key;
            this.descriptionKey = DescriptionKey;

            this.setActiveFunc = SetActiveFunc;

            value = ValueName;
        }

        public UiType getStructType()
        {
            return UiType.Toggle;
        }
    }
    public struct ButtonUI : OptionUIStyle
    {

        public ButtonObject[] buttons;

        public ButtonUI(ButtonObject[] Buttons)
        {

            this.buttons = Buttons;
        }

        public UiType getStructType()
        {
            return UiType.Button;
        }

        public struct ButtonObject
        {
            public UnityAction action;
            public string text;

            public ButtonObject(string Text, UnityAction Action)
            {
                text = Text;
                action = Action;
            }
        }
    }
}
