using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomToggleButton : CustomButton
{
    [Header("- ToggleButton -")]
    public bool toggleActive = false;
    public GameObject toggleObject;
    public UnityEvent<bool> onValueChanged;

    void OnEnable()
    {
        toggleObject.SetActive(toggleActive);
    }

    public void toggle()
    {
        toggleActive = !toggleActive;
        toggleObject.SetActive(toggleActive);
        onValueChanged.Invoke(toggleActive);
    }
}
