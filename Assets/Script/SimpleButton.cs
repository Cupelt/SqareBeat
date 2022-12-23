using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public UnityEvent onClick;

    public bool animation = true;

    public float clickScale = 1.05f;
    public float highlightScale = 1.1f;

    public float smoothSpeed = 8f;

    public GameObject isPopUp;
    public bool isHighlighted = false;

    public bool active = true;

    private void Update()
    {
        RectTransform trans = GetComponent<RectTransform>();

        if (!active)
        {
            return;
        }

        Vector3 WorldPoint = Util.getWorldPoint(trans);
        if (Util.CheckMousePos(new Vector3(WorldPoint.x, WorldPoint.y, 0), new Vector3(trans.sizeDelta.x / 2, trans.sizeDelta.y / 2, 0), true))
        {
            if (Input.GetMouseButton(0) && animation)
            {
                Vector3 smoothedPos = Vector3.Lerp(trans.localScale, Vector3.one * clickScale, smoothSpeed * Time.deltaTime);
                trans.localScale = smoothedPos;
            }
            else if (animation)
            {
                Vector3 smoothedPos = Vector3.Lerp(trans.localScale, Vector3.one * highlightScale, smoothSpeed * Time.deltaTime);
                trans.localScale = smoothedPos;
                isHighlighted = true;
            }
            else
            {
                isHighlighted = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isPopUp)
                {
                    if (!isPopUp.GetComponent<PopUpManager>().isAnimation)
                    {
                        PopUpManager.closePopUp(isPopUp.GetComponent<PopUpManager>());
                        onClick.Invoke();
                    }
                }
                else
                {
                    onClick.Invoke();
                }
            }
        } 
        else if (animation)
        {
            Vector3 smoothedPos = Vector3.Lerp(trans.localScale, new Vector3(1.0f, 1.0f, 1.0f), smoothSpeed * Time.deltaTime);
            trans.localScale = smoothedPos;
            isHighlighted = false;
        }
        else 
        {
            isHighlighted = false;
        }
    }
}
