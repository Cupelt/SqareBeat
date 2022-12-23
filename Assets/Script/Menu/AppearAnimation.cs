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
    public bool isfadeInOut = true;

    public Image[] img;
    public Text[] text;

    public float offset; // ���� �ð�
    public float delay; // �����̴� �ð�

    public Vector3 movePos; // ������ ����

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
    }

    private void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition3D = GetComponent<RectTransform>().anchoredPosition3D - movePos;
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
        Vector3 beforePos = trans.anchoredPosition3D;

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

            trans.anchoredPosition3D = beforePos + ((min + animaiton.Evaluate(time) * reverse) * movePos);
            if (isfadeInOut)
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