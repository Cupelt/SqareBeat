using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncText : MonoBehaviour
{
    public Text text;
    void Update()
    {
        if (!GetComponent<Text>().text.Equals(text.text)) GetComponent<Text>().text = text.text;
    }
}
