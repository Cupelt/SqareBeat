using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixpos : MonoBehaviour
{
    Vector3 pos;

    // Start is called before the first frame update
    void OnEnable()
    {
        pos = transform.position;
    }

    private void Update()
    {
        transform.position = pos;
    }
}
