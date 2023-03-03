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
    
    private Type _syncType;
    private object _valueObject;
    private bool _movingTextActive = false;
    
    void Update()
    {
        if (syncValue)
        {
            selected = (int)_syncType.GetField(value).GetValue(_valueObject);
        }

        mask.anchoredPosition3D = Vector3.right * Mathf.Lerp(mask.anchoredPosition3D.x, selected * -520f, Time.deltaTime * 8f);
    }

    public static GameObject buildSelection(Transform Parent, Vector3 Pos, string LocaleKey, Type ValueType, object ValueObject, string Value, string[] Contents, UnityAction Prev, UnityAction Next, UnityAction CommonFunc = null)
    {
        GameObject uiSelectionPrefab = Resources.Load<GameObject>("prefabs/ui/selection/selectionPrefab");
        GameObject contentPrefab = Resources.Load<GameObject>("prefabs/ui/selection/selectionContentPrefab");
        GameObject selection = Instantiate(uiSelectionPrefab, Parent);

        selection.GetComponent<RectTransform>().anchoredPosition3D = Pos;
        selection.GetComponent<LocaleString>().setKey(LocaleKey);
        selection.GetComponent<CustomSelection>().value = Value;
        selection.GetComponent<CustomSelection>()._syncType = ValueType;
        selection.GetComponent<CustomSelection>()._valueObject = ValueObject;

        //description add

        Transform container = selection.transform.GetChild(0);
        Transform selectList = container.GetChild(0).GetChild(0);
        int count = 0;
        foreach (string selKey in Contents)
        {
            RectTransform selectionText = Instantiate(contentPrefab, selectList).GetComponent<RectTransform>();
            selectionText.anchoredPosition3D = Vector3.right * 520 * count;

            selectionText.GetComponent<LocaleString>().setKey(selKey);
            count++;
        }

        for (int i = 1; i < 3; i++)
        {
            UnityEvent func = container.GetChild(i).GetComponent<CustomButton>().onClick;

            if (i == 1) func.AddListener(Prev);
            if (i == 2) func.AddListener(Next);
            
            if (CommonFunc != null) func.AddListener(CommonFunc);
        }

        return selection;
    }
}
