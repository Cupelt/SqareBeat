using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectBar : MonoBehaviour
{
    public int nowSel = 0;
    private float runtime = 0f;
    private RectTransform SelectBar;

    private Vector2[] SelectChildPos = new Vector2[7];

    private void Awake()
    {
        SelectBar = GetComponent<RectTransform>();
        for (int i = 0; i < SelectChildPos.Length; i++)
        {
            SelectChildPos[i] = transform.GetChild(i).position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isPopUp = MenuManager.isPopUp;
        runtime += Time.deltaTime;

        if (Util.CheckMousePos(SelectBar.anchoredPosition3D, new Vector3(SelectBar.sizeDelta.x / 2, SelectBar.sizeDelta.y / 2, 0), true) && runtime > 1)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 clickPos = new Vector3(-650 - 50, SelectBar.anchoredPosition3D.y, SelectBar.anchoredPosition3D.z);
                Vector3 smoothedPos = Vector3.Lerp(SelectBar.anchoredPosition3D, clickPos, 0.125f);
                SelectBar.anchoredPosition3D = smoothedPos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!isPopUp)
                {
                    List<PopUpManager.popUpBtn> btnList = new List<PopUpManager.popUpBtn>();
                    switch (nowSel)
                    {
                        case 6:
                            PopUpManager.popUpBtn yesBtn = new PopUpManager.popUpBtn("Yes", () => { Debug.Log("Yes"); });
                            btnList.Add(yesBtn);

                            PopUpManager.popUpBtn noBtn = new PopUpManager.popUpBtn("No");
                            btnList.Add(noBtn);

                            PopUpManager.sendMessage(MenuManager.mainCanvas, PopUpManager.popUpType.WARN, btnList.ToArray(), "Are You Sure Exit this game?");
                            break;
                        default:
                            btnList = new List<PopUpManager.popUpBtn>();

                            PopUpManager.popUpBtn OKBtn = new PopUpManager.popUpBtn("Ok");
                            btnList.Add(OKBtn);

                            PopUpManager.sendMessage(MenuManager.mainCanvas, PopUpManager.popUpType.NOTICE, btnList.ToArray(), "This action is not support yet");
                            break;
                    }
                }
            }
        }

        if (!Input.GetMouseButton(0) && runtime > 1)
        {
            Vector3 clickPos = new Vector3(-650, SelectBar.anchoredPosition3D.y, SelectBar.anchoredPosition3D.z);
            Vector3 smoothedPos = Vector3.Lerp(SelectBar.anchoredPosition3D, clickPos, 0.125f);
            SelectBar.anchoredPosition3D = smoothedPos;
        }

        Vector3 afterPos = new Vector3(SelectBar.anchoredPosition3D.x, 225f - 100f * nowSel, SelectBar.anchoredPosition3D.z);
        Vector3 smoothedPosition = Vector3.Lerp(SelectBar.anchoredPosition3D, afterPos, 0.125f);
        SelectBar.anchoredPosition3D = smoothedPosition;

        for (int i = 0; i < SelectChildPos.Length; i++)
        {
            transform.GetChild(i).position = SelectChildPos[i];
        }
    }
}
