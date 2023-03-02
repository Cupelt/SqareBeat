using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Localization : MonoBehaviour
{
    public static string locale { get; set; } = "en_us";
    public static JObject translations { get; set; }

    [HideInInspector]
    public string key = "";

    public List<int> index = new List<int>();

    public static void LoadLocale()
    {
        TextAsset Localejson = Resources.Load<TextAsset>("lang/"+locale);
        if (Localejson == null) Localejson = Resources.Load<TextAsset>("lang/en_us");

        translations = JObject.Parse(Localejson.text);
        applyLocale();
    }

    public string getTranslation()
    {
        return getTranslationFormKey(key);
    }

    public static string getTranslationFormKey(string key)
    {
        string[] Keys = key.Split('.');
        JObject trans = translations;

        foreach (string _key in Keys)
        {
            try { trans = trans[_key].ToObject<JObject>(); }
            catch { try { return trans[_key].ToString(); } catch { } }
        }
        return key;
    }

    public void setKey(string trnaslationKey)
    {
        JObject trans = translations;
        if (trans == null)
        {
            LoadLocale();
            trans = translations;
        }

        string[] keys = trnaslationKey.Split('.');
        for (int i = 0; i < keys.Length; i++)
        {
            int count = 0;
            foreach (var type in trans)
            {
                if (type.Key == keys[i])
                {
                    try
                    {
                        trans = trans[type.Key].ToObject<JObject>();
                    }
                    catch { }
                    break;
                }
                else count++;
            }
            index.Insert(i, count);
        }
        key = trnaslationKey;
    }
    
    public string fixedText(string text, string[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            text = text.Replace("{value[" + i + "]}", value[i]);
        }
        return text;
    }

    void OnEnable()
    {
        changeLocale();
    }

    public static void applyLocale()
    {
        Localization[] locals = GameObject.FindObjectsOfType<Localization>() as Localization[];
        foreach (Localization loc in locals)
        {
            loc.changeLocale();
        }
    }
    public virtual void changeLocale() { }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(Localization), true)]
public class LocalizationGUI : Editor
{
    Localization localization;
    public bool isOpenOption = false;
    private string key = "";

    private void OnEnable()
    {
        localization = target as Localization;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        int i_height = 1;
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

        List<int> index = localization.index;
        int maxLength = index.Count;

        Localization.LoadLocale();
        JObject trans = Localization.translations;

        GUILayout.Label($"Key Value : \"{key}\" / {localization.getTranslation()}", EditorStyles.boldLabel);
        key = "";

        EditorGUILayout.BeginHorizontal();

        int count = 0;
        while (true)
        {
            List<string> keyList = new List<string>();

            foreach (var type in trans)
                keyList.Add(type.Key);


            if (count >= maxLength)
            {
                index.Add(0);
                maxLength++;
            }

            index[count] = EditorGUILayout.Popup(index[count], keyList.ToArray());
            if (index[count] >= keyList.Count)
            {
                for (int i = count; i < index.Count; i++)
                    index[i] = 0;
            }

            try
            {
                trans = trans[keyList[index[count]]].ToObject<JObject>();
                if (count == 0) key += keyList[index[count]];
                else key += "." + keyList[index[count]];
            }
            catch
            {
                key += "." + keyList[index[count]];
                break;
            }

            count++;
        }
        localization.key = key;
        EditorGUILayout.EndHorizontal();
    }
}
#endif