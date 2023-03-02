using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using com.cupelt.util;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CustomSelection : MonoBehaviour
{
    public RectTransform mask;

    public bool syncValue = true;
    [HideInInspector] public int selected = -1;
    [HideInInspector] public string value = "";
    
    private Type syncType;
    private object valueObject;
    private bool movingTextActive = false;
    
    void Update()
    {
        if (syncValue)
        {
            selected = (int)syncType.GetField(value).GetValue(valueObject);
        }

        mask.anchoredPosition3D = Vector3.right * Mathf.Lerp(mask.anchoredPosition3D.x, selected * -520f, Time.deltaTime * 8f);
    }

    public static GameObject buildSelection(Transform parent, Vector3 pos, string localeKey, Type valueType, object valueObject, string value, string[] contents, UnityAction prev, UnityAction next, UnityAction commonFunc = null)
    {
        GameObject uiSelectionPrefab = Resources.Load<GameObject>("prefabs/ui/selection/selectionPrefab");
        GameObject contentPrefab = Resources.Load<GameObject>("prefabs/ui/selection/selectionContentPrefab");
        GameObject selection = Instantiate(uiSelectionPrefab, parent);

        selection.GetComponent<RectTransform>().anchoredPosition3D = pos;
        selection.GetComponent<LocaleString>().setKey(localeKey);
        selection.GetComponent<CustomSelection>().value = value;
        selection.GetComponent<CustomSelection>().syncType = valueType;
        selection.GetComponent<CustomSelection>().valueObject = valueObject;

        //description add

        Transform container = selection.transform.GetChild(0);
        Transform selectList = container.GetChild(0).GetChild(0);
        int count = 0;
        foreach (string selKey in contents)
        {
            RectTransform selectionText = Instantiate(contentPrefab, selectList).GetComponent<RectTransform>();
            selectionText.anchoredPosition3D = Vector3.right * 520 * count;

            selectionText.GetComponent<LocaleString>().setKey(selKey);
            count++;
        }

        for (int i = 1; i < 3; i++)
        {
            UnityEvent func = container.GetChild(i).GetComponent<CustomButton>().onClick;

            if (i == 1) func.AddListener(prev);
            if (i == 2) func.AddListener(next);
            
            if (commonFunc != null) func.AddListener(commonFunc);
        }

        return selection;
    }
}
