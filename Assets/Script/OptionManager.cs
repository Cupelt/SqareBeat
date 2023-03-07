using System;
using com.cupelt.option;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using com.cupelt.util;

public class OptionManager : MonoBehaviour
{
    //Objects
    public Transform parents;
    public GameObject frameRateObject;

    //value
    private static OptionManager instance;
    public static OptionManager Instance => instance;
    public Option option;

    //Other Value
    private float fps = 60f;

    private void Awake()
    {
        instance = this;
        option = Option.load();

        OptionUIBuilder builder = GetComponent<OptionUIBuilder>();

        option.applyOption();
        builder.drawOptionUI(builder.createOptionUI());
    }
}