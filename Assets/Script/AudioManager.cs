using System.Collections;
using System.Collections.Generic;
using com.cupelt.sqarebeat;
using com.cupelt.util;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public enum Cycle { Cycle, Repeat, Random }
    
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    public AudioSource audio;
    
    public BeatMap nowMusic;
    public List<BeatMap> musicList = new List<BeatMap>();
    public Stack<BeatMap> musicStack = new Stack<BeatMap>();

    //option
    public bool isPause = false;
    public bool isMusicEnd;
    public Cycle nowCycle = Cycle.Cycle;

    void Awake()
    {
        instance = this;
        audio = GetComponent<AudioSource>();
        
        LoadMusicList();
        initMusic();
    }

    private void Update()
    {
        audio = GetComponent<AudioSource>();

        if (isPause)
            audio.Pause();
        else
            audio.UnPause();
        
        if (audio.time >= audio.clip.length - 0.1f)
        {
            isMusicEnd = true;
        }
    }

    public void LoadMusicList()
    {
        //@todo load music methods
        
        musicList.Sort((x, y) => x.Artist.CompareTo(y.Artist));
    }

    public void initMusic()
    {
        int track = Random.Range(0, musicList.Count);

        nowMusic = musicList[track];
        audio.clip = nowMusic.Clip;
        audio.Play();
    }

    public BeatMap changeTrack(int increase)
    {
        int track = musicList.IndexOf(nowMusic);
        if (track == -1)
        {
            return musicList[0];
        }

        if (increase < 0)
        {
            return musicStack.Pop();
        }
        
        switch (nowCycle)
        {
            case Cycle.Cycle:
                musicStack.Push(musicList[track]);
                track = Util.changeCycleValue(musicList.Count, track, increase);
                break;
            case Cycle.Random:
                musicStack.Push(musicList[track]);
                if (musicList.Count > 2)
                {
                    int rand = track;
                    while (track == rand)
                    {
                        rand = Random.Range(0, musicList.Count);
                    }
                    track = rand;
                }
                break;
        }
        
        return musicList[track];
    }

    public void setTrack(BeatMap music)
    {
        isMusicEnd = false;
        nowMusic = music;
        audio.clip = nowMusic.Clip;
        audio.time = 0;
        audio.Play();
    }
}
