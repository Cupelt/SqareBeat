using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppearAnimation : MonoBehaviour
{
    public enum fadeType { fadeIn, fadeOut, NONE }

    public AnimationCurve animaiton;

    public fadeType fade;

    public Image[] img;
    public Text[] text;

    public float offset; // 시작 시간
    public float delay; // 움직이는 시간

    public Vector3 movePos; // 움직일 정도

    public bool isDone = false;

    private void Awake()
    {
        StartCoroutine(Animation(gameObject));
    }

    IEnumerator Animation(GameObject obj)
    {
        yield return new WaitForSeconds(offset);

        float time = 0f;
        float value;

        RectTransform trans = gameObject.GetComponent<RectTransform>();
        Vector3 beforePos = trans.anchoredPosition3D;

        while (time < 1)
        {
            time += Time.deltaTime / delay;

            trans.anchoredPosition3D = beforePos + (animaiton.Evaluate(time) * movePos);
            if (!fade.Equals(fadeType.NONE))
            {
                float reverse = 1f;
                float min = 0f;
                if (fade.Equals(fadeType.fadeOut))
                {
                    reverse = -1f;
                    min = 1f;
                }

                Color color;
                for (int i = 0; i < img.Length; i++)
                {
                    color = img[i].color;
                    color.a = min + animaiton.Evaluate(time) * reverse;
                    img[i].color = color;
                }

                for (int i = 0; i < text.Length; i++)
                {
                    color = text[i].color;
                    color.a = min + animaiton.Evaluate(time) * reverse;
                    text[i].color = color;
                }
            }

            yield return null;
        }
        yield break;
    }
}