using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationText : Animation
{
    public override void setAlpha(float alpha)
    {
        Color color = GetComponent<Text>().color;
        color.a = alpha;
        GetComponent<Text>().color = color;
    }
}
