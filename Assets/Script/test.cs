using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{

    public Transform Canvas;

    private void Start()
    {
        Vector3 one = new Vector3(0f, 0f, 0f);
        Vector3 two = new Vector3(1f, 0f, 0f);

        //StartCoroutine(popUp());
    }

    private void Update()
    {
        Debug.Log(Util.getWorldPoint(GetComponent<RectTransform>()));
    }
}
