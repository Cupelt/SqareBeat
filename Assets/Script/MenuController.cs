using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using com.cupelt.util;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MenuController : MonoBehaviour
{
    public string mainTextKey;
    public Contents[] contents;

    public Animation[] animations;
    public CustomButton[] buttons;

    [HideInInspector] public bool active = false;
    [HideInInspector] public bool freeMove = false;
    [HideInInspector] public int nowSelect = -1;
    [HideInInspector] public RectTransform selectBar;

    private void Awake()
    {
        float top = (contents.Length * 30f + 30f) / 2 + 120f;
        List<Animation> ani = new List<Animation>();
        List<CustomButton> btns = new List<CustomButton>();
        List<CheckMouseCursor> sels = new List<CheckMouseCursor>();

        Transform menuParents = new GameObject("Menu", typeof(RectTransform)).transform;
        menuParents.gameObject.SetActive(false);
        menuParents.transform.SetParent(transform);

        GameObject dummy = new GameObject("dummy", typeof(RectTransform));
        Transform bar = Instantiate(Resources.Load<GameObject>("prefabs/Menu/selectBar"), transform).transform;
        selectBar = bar.GetComponent<RectTransform>();

        bar = bar.GetChild(1);

        GameObject title = Instantiate(dummy, menuParents.transform);
        Text titleText = title.AddComponent<Text>();
        AnimationText titleAnimation = title.AddComponent<AnimationText>();
        RectTransform titleTrans = title.GetComponent<RectTransform>();
        LocaleString titleLocale = title.AddComponent<LocaleString>();

        title.name = mainTextKey;
        
        Vector2 titleSize = new Vector2(700f, 225f);
        titleTrans.sizeDelta = titleSize;
        titleTrans.anchoredPosition3D = Vector3.up * top;

        titleAnimation.animaton = Tweening.OutQuart;
        titleAnimation.delay = 0.5f;
        titleAnimation.offset = 0.25f;
        titleAnimation.movePos = Vector3.up * -350;
        
        titleLocale.text = titleText;
        titleLocale.setKey(mainTextKey);
        
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.font = Resources.Load<Font>("Font/Towards");
        titleText.fontSize = 85;

        ani.Add(titleAnimation);
        Destroy(dummy);

        int count = 0;
        foreach (Contents con in contents)
        {
            GameObject content = Instantiate(title, menuParents.transform);
            Text text = content.GetComponent<Text>();
            AnimationText animation = content.GetComponent<AnimationText>();
            RectTransform trans = content.GetComponent<RectTransform>();
            LocaleString locale = content.GetComponent<LocaleString>();
            CustomButton btn = content.AddComponent<CustomButton>();

            content.name = con.textKey;

            Vector2 size = new Vector2(550f, 75f);
            trans.sizeDelta = size;
            trans.anchoredPosition3D = Vector3.up * (top - 120 - count * 60f);

            animation.animaton = Tweening.OutQuart;
            animation.offset = count * 0.1f;
            animation.movePos = Vector3.up * 50;
            
            btn.nonAnimation = true;
            btn.onClick = con.action;
            
            locale.setKey(con.textKey);
            
            text.font = Resources.Load<Font>("Font/NotoSansKR-Bold");
            text.fontSize = 40;

            GameObject maskedObject = Instantiate(content, bar.transform);
            RectTransform maskedTrans = maskedObject.GetComponent<RectTransform>();
            Text maskedText = maskedObject.GetComponent<Text>();

            maskedTrans.anchoredPosition3D = new Vector3(maskedTrans.anchoredPosition3D.x,
                maskedTrans.anchoredPosition3D.y - selectBar.anchoredPosition3D.y, maskedTrans.anchoredPosition3D.z);

            maskedText.color = Color.black;
            maskedText.raycastTarget = false;
            maskedObject.AddComponent<Fixpos>();

            Destroy(maskedObject.GetComponent<CustomButton>());
            Destroy(maskedObject.GetComponent<AnimationText>());

            btns.Add(btn);
            ani.Add(animation);
            count++;
        }

        menuParents.transform.SetParent(transform);
        title.transform.SetParent(menuParents);
        selectBar.SetAsLastSibling();

        buttons = btns.ToArray();
        animations = ani.ToArray();
        menuParents.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (active && !freeMove)
        {
            if (nowSelect == -1) nowSelect = 0;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].isCurserOnObject)
                    nowSelect = i;
            }

            selectBar.anchoredPosition3D = Vector3.Lerp(selectBar.anchoredPosition3D,
                buttons[nowSelect].gameObject.GetComponent<RectTransform>().anchoredPosition3D, Time.deltaTime * 8);
        }
        else if (!freeMove)
        {
            nowSelect = -1;
            selectBar.anchoredPosition3D = Vector3.up * 580f;
        }

        Transform texts = transform.GetChild(1).GetChild(1);
        LocaleString movingText = transform.GetChild(1).GetChild(2).GetComponent<LocaleString>();
        
        if (freeMove) movingText.setKey(buttons[nowSelect].GetComponent<Text>().text);
        movingText.gameObject.SetActive(freeMove);

        for (int i = 0; i < texts.childCount; i++)
        {
            texts.GetChild(i).gameObject.SetActive(!freeMove);
        }
    }

    [System.Serializable]
    public struct Contents
    {
        public string textKey;
        public UnityEvent action;
    }
}
