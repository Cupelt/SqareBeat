using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorFlicker : MonoBehaviour
{
    public AnimationCurve flikeringCurve;

    private float time = 0.0f;
    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<Text>().color;
        time += Time.deltaTime / 4;

        color.a = flikeringCurve.Evaluate(time - (int)time);

        GetComponent<Text>().color = color;
    }
}
