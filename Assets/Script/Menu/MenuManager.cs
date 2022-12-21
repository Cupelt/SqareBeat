using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AudioManager;

public class MenuManager : MonoBehaviour
{
    public static Transform mainCanvas;
    public Transform canvas;

    public MenuSelectBar SelectBar;

    public SideBar sideBar;
    public SideBar songList;

    public static int widith = 1920;
    public static int height = 1080;

    public static bool isPopUp = false;
    public bool isListEnable = false;

    private List<GameObject> musicObjectList;
    public GameObject musicObjects;
    public Transform musicListParent;

    private bool isScrolled = false;
    private float scrollSelectTime = 0f;
    private int scrollValue = 0;

    private void Awake()
    {
        musicObjectList = new List<GameObject>();
        for (int i = 0; i < musicList.Count; i++)
        {
            GameObject clone = Instantiate(musicObjects, musicListParent);
            MusicInfo info = musicList[i];
            string musicInfo = info.Artist + " - " + info.Title;
            clone.name = musicInfo;
            clone.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(50f, -100 * i, 0);
            clone.transform.GetChild(0).GetComponent<Text>().text = musicInfo;
            clone.transform.GetChild(0).GetComponent<SimpleButton>().onClick.AddListener(() => { setTrack(info); });
            musicObjectList.Add(clone);
        }
    }

    void Update()
    {
        mainCanvas = canvas;

        Vector3 mousePos = Util.getMousePos();

        if (Util.CheckMousePos(new Vector3(325f, 470f, 0), new Vector3(325f, 330f, 0)))
        {
            SelectBar.nowSel = 7 - (int)(mousePos.y - 15) / 100;
        }

        if (!Util.CheckMousePos(Vector3.zero, new Vector3(widith * 2f, 475f, 0), true) || (isListEnable && songList.isSlected))
        {
            sideBar.fadeInSideBar();
        }
        else 
        {
            sideBar.fadeOutSideBar();
            if (isListEnable)
                isListEnable = false; 
        }
    
        if (isListEnable)
        {
            songList.fadeInSideBar();
        }
        else
        {
            songList.fadeOutSideBar();
        }

        InputCheck();

        if (isListEnable && songList.isSlected)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel") * -10;
            if (wheelInput != 0)
            {
                isScrolled = true;
                scrollValue += (int)wheelInput;
                scrollSelectTime = 0;
            }
        }

        if (isScrolled)
        {
            int selectedMusic = moveMusicListObject(scrollValue);
            scrollSelectTime += Time.deltaTime;
            if (scrollSelectTime > 1)
            {
                isScrolled = false;
                scrollSelectTime = 0;
                scrollValue = 0;

                if (!musicList[selectedMusic].Equals(nowMusic))
                {
                    setTrack(musicList[selectedMusic]);
                }
            }
        }
        else
        {
            moveMusicListObject(0);
        }
    }

    public void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && sideBar.isOpend)
        {
            AudioManager.Pause();
        }
    }

    public void SelectMenu()
    {

    }

    public int moveMusicListObject(int offset)
    {
        int track = musicList.IndexOf(nowMusic);
        if (track == -1)
        {
            initMusic();
            return -1;
        }

        track += offset;
        track = track % musicList.Count;
        if (track >= musicList.Count)
            track = 0;
        else if (track < 0)
            track = musicList.Count - 1;

        for (int i = 0; i < musicObjectList.Count; i++)
        {
            List<GameObject> list = musicObjectList;
            RectTransform trans = list[i].GetComponent<RectTransform>();

            Vector3 pos = new Vector3(50f, -100 * (i - track), 0);
            if (i == track)
            {
                list[i].transform.GetChild(0).GetComponent<SimpleButton>().active = false;
                pos.x = 0f;
            }
            else
            {
                list[i].transform.GetChild(0).GetComponent<SimpleButton>().active = true;
            }
            Vector3 smoothedPos = Vector3.Lerp(trans.anchoredPosition3D, pos, 8f * Time.deltaTime);
            trans.anchoredPosition3D = smoothedPos;
        }

        return track;
    }

    public void ToggleSongList()
    {
        isListEnable = !isListEnable;
    }
}
