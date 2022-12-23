using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAniParents : MonoBehaviour
{
    public List<AppearAnimation> animations;

    public void onEnableAll()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            animations[i].gameObject.SetActive(true);
        }
    }

    public void onDisableAll()
    {
        for (int i = 0; i < animations.Count; i++)
        {
            if (animations[i].gameObject.active)
                animations[i].onDisable();
        }
    }
}
