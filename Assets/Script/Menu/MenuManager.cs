using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static Transform mainCanvas;
    public Transform canvas;

    public MenuSelectBar SelectBar;

    public SideBar sideBar;
    public SideBar songList;

    public GameObject icon;
    public GameObject mainText;

    public static int widith = 1920;
    public static int height = 1080;

    public static bool isPopUp = false;
    public bool isListEnable = false;

    private List<GameObject> musicObjectList;
    public GameObject musicObjects;
    public Transform musicListParent;
    public GameObject menuBtns;

    private bool isScrolled = false;
    private float scrollSelectTime = 0f;
    private int scrollValue = 0;
    private bool isSetSong = false;

    private bool selectMenu = false;
    private float selectMenuTime = 0;

    private void Awake()
    {
        musicObjectList = new List<GameObject>();
        for (int i = 0; i < AudioManager.musicList.Count; i++)
        {
            GameObject clone = Instantiate(musicObjects, musicListParent);
            AudioManager.MusicInfo info = AudioManager.musicList[i];
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

        if (Util.CheckMousePos(new Vector3(960f, 470f, 0), new Vector3(325f, 330f, 0)))
        {
            if (SelectBar.nowSel != 7 - ((int)(mousePos.y - 15) / 100))
                selectMenuTime = 0;

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

        icon.GetComponent<SimpleButton>().active = !sideBar.isOpend;

        Color fixedColor = icon.GetComponent<Image>().color;
        if (selectMenu)
        {
            fixedColor.a = Mathf.Lerp(fixedColor.a, 0.025f, Time.deltaTime * 8f);
            mainText.SetActive(false);
            menuBtns.GetComponent<AppearAniParents>().onEnableAll();
            SelectBar.gameObject.SetActive(true);
            selectMenuTime += Time.deltaTime;
            if (selectMenuTime > 30f || Input.GetKeyDown(KeyCode.Escape))
            {
                setSelectMenu(false);
            }
        }
        else
        {
            fixedColor.a = Mathf.Lerp(fixedColor.a, 1f, Time.deltaTime * 8f);
            mainText.SetActive(true);
            menuBtns.GetComponent<AppearAniParents>().onDisableAll();
            SelectBar.gameObject.SetActive(false);
            selectMenuTime = 0;
        }
        icon.GetComponent<Image>().color = fixedColor;


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

        if (isScrolled || isSetSong)
        {
            int selectedMusic = moveMusicListObject(scrollValue);
            scrollSelectTime += Time.deltaTime;
            if (scrollSelectTime > 1)
            {
                isScrolled = false;
                scrollSelectTime = 0;
                scrollValue = 0;
                isSetSong = false;

                if (!AudioManager.musicList[selectedMusic].Equals(AudioManager.nowMusic))
                {
                    AudioManager.setTrack(AudioManager.musicList[selectedMusic]);
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

    public void setSelectMenu(bool isOpen)
    {
        selectMenu = isOpen;
    }

    public void setTrack(AudioManager.MusicInfo info)
    {
        isSetSong = true;
        scrollValue = AudioManager.musicList.Count - AudioManager.musicList.IndexOf(AudioManager.nowMusic) + AudioManager.musicList.IndexOf(info);
        scrollSelectTime = 0;
    }

    public int moveMusicListObject(int offset)
    {
        int track = AudioManager.musicList.IndexOf(AudioManager.nowMusic);
        if (track == -1)
        {
            AudioManager.initMusic();
            return -1;
        }

        track += offset;
        track = track % AudioManager.musicList.Count;
        if (track >= AudioManager.musicList.Count)
            track = 0;
        else if (track < 0)
            track = AudioManager.musicList.Count - 1;

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
