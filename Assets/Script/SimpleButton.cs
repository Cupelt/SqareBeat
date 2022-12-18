using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    public UnityEvent onClick;

    public float clickScale = 1.05f;
    public float highlightScale = 1.1f;

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
            if (Input.GetMouseButton(0))
            {
                Vector3 smoothedPos = Vector3.Lerp(trans.localScale, Vector3.one * clickScale, 0.125f);
                trans.localScale = smoothedPos;
            }
            else
            {
                Vector3 smoothedPos = Vector3.Lerp(trans.localScale, Vector3.one * highlightScale, 0.125f);
                trans.localScale = smoothedPos;
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
        else 
        {
            Vector3 smoothedPos = Vector3.Lerp(trans.localScale, new Vector3(1.0f, 1.0f, 1.0f), 0.125f);
            trans.localScale = smoothedPos;
            isHighlighted = false;
        }
    }
}
