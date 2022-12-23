using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static bool CheckMousePos(Vector3 originVector, Vector3 range, bool isMiddle = false)
    {
        return CheckVector(originVector, getMousePos(isMiddle), range);
    }

    public static bool CheckVector(Vector3 originVector, Vector3 conditionVector, Vector3 range)
    {
        if (originVector.x + range.x > conditionVector.x && originVector.x - range.x < conditionVector.x)
            if (originVector.y + range.y > conditionVector.y && originVector.y - range.y < conditionVector.y)
                    return true;

        return false;
    }

    public static Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        bool active = destination.active;
        destination.SetActive(false);
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        destination.SetActive(active);
        return copy;
    }

    public static Vector3 getMousePos(bool isMiddle = false)
    {
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z));
        if (isMiddle)
            mousePos = new Vector3(mousePos.x * 1920 - 960, mousePos.y * 1080 - 540, 0);
        else
            mousePos = new Vector3(mousePos.x * 1920, mousePos.y * 1080, 0);
        return mousePos;
    }

    public static Vector3 getWorldPoint(RectTransform trans)
    {
        Transform parent = trans.parent;
        Vector3 fixedPos = trans.anchoredPosition3D;
        while (parent)
        {
            fixedPos += parent.GetComponent<RectTransform>().anchoredPosition3D;
            parent = parent.parent;
        }

        return fixedPos;
    }
}
