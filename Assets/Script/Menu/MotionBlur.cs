using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 8f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition = target.GetComponent<RectTransform>().anchoredPosition;
        Vector3 desiredPosition = target.localScale + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.localScale, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.localScale = smoothedPosition;
    }
}
