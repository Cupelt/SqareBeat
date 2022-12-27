using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppearAnimation : MonoBehaviour
{
    public enum fadeType { fadeIn, fadeOut }

    public AnimationCurve animaiton;

    public fadeType fade;
    public bool isFadeInOut = true;

    public Image[] img;
    public Text[] text;

    public float offset; // 시작 시간
    public float delay; // 움직이는 시간

    public Vector3 movePos; // 움직일 정도
    private Vector3 originPos;

    public bool isDone = true;

    private List<float> imgColor = new List<float>();
    private List<float> textColor = new List<float>();


    private void Awake()
    {
        for (int i = 0; i < img.Length; i++)
        {
            imgColor.Add(img[i].color.a);
            if (fade.Equals(fadeType.fadeIn))
            {
                Color fixedColor = img[i].color;
                fixedColor.a = 0;
                img[i].color = fixedColor;
            }
        }

        for (int i = 0; i < text.Length; i++)
        {
            textColor.Add(text[i].color.a);
            if (fade.Equals(fadeType.fadeIn))
            {
                Color fixedColor = text[i].color;
                fixedColor.a = 0;
                text[i].color = fixedColor;
            }
        }
        originPos = GetComponent<RectTransform>().anchoredPosition3D;
    }

    private void OnEnable()
    {
        StartCoroutine(Animation(fade, false));
    }

    public void onDisable()
    {
        fadeType reverse;
        if (fade.Equals(fadeType.fadeIn))
            reverse = fadeType.fadeOut;
        else
            reverse = fadeType.fadeIn;

        StartCoroutine(Animation(reverse, true));
    }

    IEnumerator Animation(fadeType _fade, bool setDisable)
    {
        if (!isDone)
            yield break;

        isDone = false;
        yield return new WaitForSeconds(offset);

        float time = 0f;
        float value;

        RectTransform trans = gameObject.GetComponent<RectTransform>();

        float reverse = 1f;
        float min = 0f;
        if (_fade.Equals(fadeType.fadeOut))
        {
            reverse = -1f;
            min = 1f;
        }

        while (time < 1)
        {
            time += Time.deltaTime / delay;
            trans.anchoredPosition3D = originPos + ((min + animaiton.Evaluate(time) * reverse) * movePos) - (movePos * (reverse + min * 2));
            if (isFadeInOut)
            {
                for (int i = 0; i < img.Length; i++)
                {
                    Color color = img[i].color;
                    color.a = (min + animaiton.Evaluate(time) * reverse) * imgColor[i];
                    img[i].color = color;
                }

                for (int i = 0; i < text.Length; i++)
                {
                    Color color = text[i].color;
                    color.a = (min + animaiton.Evaluate(time) * reverse) * textColor[i];
                    text[i].color = color;
                }
            }

            yield return null;
        }
        isDone = true;
        
        if (setDisable)
            gameObject.SetActive(false);
        yield break;
    }
}