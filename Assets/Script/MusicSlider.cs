using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{

    public Image backGroundObject;
    public GameObject valueObject;
    public RectTransform test;

    [Range(0f, 1f)]
    public float value;

    private Image audioValue;
    private RectTransform valueTrans;
    public bool isClick = false;
    private bool firstPause;

    void Start()
    {
        audioValue = valueObject.GetComponent<Image>();
        valueTrans = valueObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        audioValue.fillAmount = AudioManager.audio.time / AudioManager.audio.clip.length;

        float scale = 20f;

        Vector3 valuePos = new Vector3(valueTrans.sizeDelta.x * (audioValue.fillAmount - 0.5f), 0, 0);

        if (Util.CheckMousePos(Vector3.down * 550, new Vector3(valueTrans.sizeDelta.x / 2f, scale, 0), true))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClick = true;
                firstPause = AudioManager.isPause;
                if (!firstPause)
                    AudioManager.isPause = true;
            }
        }

        if (isClick)
        {
            if (Input.GetMouseButtonUp(0))
            {
                AudioManager.isPause = firstPause;
                isClick = false;
            }

            if (Input.GetMouseButton(0))
            {
                float time = AudioManager.audio.clip.length * (Util.getMousePos(true).x / valueTrans.sizeDelta.x + 0.5f);
                if (time > AudioManager.audio.clip.length)
                    time = AudioManager.audio.clip.length - 1f;
                else if (time < 0)
                    time = 0f;
                AudioManager.audio.time = time;
            } 
            else
            {
                isClick = false;
            }
        }
    }
}