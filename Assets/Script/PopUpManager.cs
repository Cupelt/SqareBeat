using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public enum popUpType { WARN, NOTICE, ERROR }
    private enum AniType { BG, BTN, IMAGE, MESSAGE }

    public AnimationCurve bgCurve;

    public List<AppearAnimation> CompList; 

    public List<GameObject> btn;
    public GameObject messageBar;
    public GameObject typeImage;
    public RectTransform bg;

    public string message;

    private List<AppearAnimation> AnimationList = new List<AppearAnimation>();

    public bool isAnimation = false;

    public static void sendMessage(Transform mainCanvas, popUpType type, popUpBtn[] btn, string msg)
    {
        GameObject btnPrefab = Resources.Load<GameObject>("prefabs/popUp/Button");
        GameObject popUpPrefab = Resources.Load<GameObject>("prefabs/popUp/PopUp");
        List<GameObject> btnList = new List<GameObject>();

        GameObject popUpClone = Instantiate(popUpPrefab);
        popUpClone.transform.parent = mainCanvas;
        popUpClone.transform.localScale = Vector3.one;

        Color imageColor;
        switch (type)
        {
            case popUpType.WARN:
                imageColor = new Color(1f, 0.75f, 0);
                break;
            case popUpType.ERROR:
                imageColor = Color.red;
                break;
            case popUpType.NOTICE:
                imageColor = Color.white;
                break;
            default:
                imageColor = Color.black;
                break;
        }

        popUpClone.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = imageColor; // ¿Ü°û¼±
        popUpClone.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = imageColor;

        imageColor.a = 0;

        popUpClone.transform.GetChild(1).GetComponent<Image>().color = imageColor;

        for (int i = 0; i < btn.Length; i++)
        {
            GameObject clone = Instantiate(btnPrefab);
            SimpleButton cloneBtn = clone.GetComponent<SimpleButton>();
            Text cloneText = clone.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            
            cloneText.text = btn[i].text;
            cloneBtn.onClick.AddListener(btn[i].action);
            cloneBtn.isPopUp = popUpClone;

            clone.transform.parent = popUpClone.transform;
            clone.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -170, 0);
            clone.SetActive(true);

            btnList.Add(clone);
        }

        popUpClone.GetComponent<PopUpManager>().message = msg;
        popUpClone.GetComponent<PopUpManager>().btn = btnList;

        popUpClone.SetActive(true);
    }

    private void Awake()
    {
        MenuManager.isPopUp = true;
        isAnimation = true;
        messageBar.GetComponent<Text>().text = message;

        AnimationList.Add(transform.GetChild(1).GetComponent<AppearAnimation>());
        AnimationList.Add(transform.GetChild(2).GetComponent<AppearAnimation>());

        StartCoroutine(openAnimation());

        int count = btn.Count / 2;
        int pos = 0;

        pos = (btn.Count - 1) * -150;

        for (int i = 0; i < btn.Count; i++)
        {
            Vector3 firstPos = btn[i].GetComponent<RectTransform>().anchoredPosition3D;
            firstPos.x = pos + i * 300f;
            btn[i].GetComponent<RectTransform>().anchoredPosition3D = firstPos;
            AnimationList.Add(btn[i].GetComponent<AppearAnimation>());
            btn[i].active = true;
        }
    }

    IEnumerator openAnimation()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            bg.localScale = new Vector3(1, bgCurve.Evaluate(time), 1);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        isAnimation = false;
    }

    public static void closePopUp(PopUpManager popUp)
    {
        popUp.isAnimation = true;
        popUp.StartCoroutine(popUp.closeAnimation());
        for (int i = 0; i < popUp.AnimationList.Count; i++)
        {
            popUp.AnimationList[i].fade = AppearAnimation.fadeType.fadeOut;
            popUp.AnimationList[i].movePos *= -1;
            popUp.AnimationList[i].offset -= 0.5f;
        }

        for (int i = 0; i < popUp.AnimationList.Count; i++)
        {
            GameObject child;
            if (i < 2)
                child = popUp.transform.GetChild(i + 1).gameObject;
            else
                child = popUp.btn[i - 2].gameObject;

            Util.CopyComponent(popUp.AnimationList[i], child);
        }
    }

    public IEnumerator closeAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            bg.localScale = new Vector3(1, 1 - bgCurve.Evaluate(time), 1);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        isAnimation = false;
        MenuManager.isPopUp = false;
        Destroy(gameObject);
    }

    [System.Serializable]
    public struct popUpBtn
    {
        public popUpBtn(string _text)
        {
            text = _text;
            action = () => { };
        }

        public popUpBtn(string _text, UnityAction _action)
        {
            text = _text;
            action = _action;
        }
        public UnityAction action { get; }
        public string text { get; }
    }
}
