using System;
using System.Collections;
using System.Collections.Generic;
using com.cupelt.sqarebeat;
using com.cupelt.util;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelector : MonoBehaviour
{
    public enum SelectType { OFFICIAL, GLOBAL }

    public SelectType type = SelectType.OFFICIAL;
    
    public GameObject prefab;
    public int scroll = 0;

    private List<GameObject> playList = new List<GameObject>();
    private int nowMusic;

    private AudioManager manager => AudioManager.Instance;
    private float delay = 0f;

    private void Start()
    {
        createMusicObject();
        nowMusic = manager.musicList.IndexOf(manager.nowMusic);
    }

    private void Update()
    {
        delay += Time.deltaTime;
        if (Input.mouseScrollDelta.y != 0)
        {
            delay = 0;
            scroll += (int)(Input.mouseScrollDelta.y);
        }

        int result = Util.changeCycleValue(manager.musicList.Count, nowMusic, scroll);

        if (delay > 2f && result != nowMusic)
        {
            manager.setTrack(manager.musicList[result]);
            scroll = 0;
            nowMusic = result;
        }

        int count = 0;
        foreach (GameObject ob in playList)
        {
            int num = count - result;
            
            RectTransform trans = ob.GetComponent<RectTransform>();
            Vector3 fixedPos = Vector3.up * (300 * num);
            Vector3 fixedSize = Vector3.one * (1f - Mathf.Abs(num) * 0.25f);

            trans.anchoredPosition3D = Vector3.Lerp(trans.anchoredPosition3D, fixedPos, Time.deltaTime * 8f);

            ob.GetComponent<CustomButton>().isActive = (Mathf.Abs(num) == 0);
            
            if (!ob.GetComponent<CustomButton>().isActive) trans.localScale = Vector3.Lerp(trans.localScale, fixedSize, Time.deltaTime * 8f);

            count++;
        }
    }

    private void createMusicObject()
    {
        foreach (BeatMap music in manager.musicList)
        {
            GameObject clone = Instantiate(prefab, transform);
            clone.name = $"{music.Artist} - {music.Title}";
            clone.transform.GetChild(0).GetComponent<Text>().text = clone.name;

            //난이도 -
            //clone.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = star / 15.0f;
            
            //시작 -
            /*
            clone.GetComponent<CustomButton>().onClick.AddListener(() =>
            {
                
            });
            */
            
            playList.Add(clone);
        }
    }
}
