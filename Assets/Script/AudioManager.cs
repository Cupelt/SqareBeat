using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public enum Track { Next = 0, Prev = 1 }
    public enum Cycle { Cycle, Repeat, Random }

    public static bool isPause = false;
    public static AudioSource audio;
    private static AudioManager instance;
    public static Cycle nowCycle = Cycle.Cycle;

    public List<AudioClip> clips;
    public List<string> artist;
    public List<string> title;

    public static Stack<MusicInfo> MusicStack = new Stack<MusicInfo>();
    public static List<MusicInfo> musicList;

    public GameObject PlayBtn;
    public GameObject PauseBtn;

    public SimpleButton prevBtn;
    public Image prevBtnImage;

    public Text TitleFeild;

    private static MusicInfo nowMusic;

    void Awake()
    {
        audio = GetComponent<AudioSource>();

        instance = this;

        LoadMusicList();
        initMusic();
    }

    private void Update()
    {
        LoadMusicList(); //develop

        audio = GetComponent<AudioSource>();
        instance = this; //develop

        PlayBtn.SetActive(isPause);
        PauseBtn.SetActive(!isPause);

        if (isPause)
            audio.Pause();
        else
            audio.UnPause();

        if (audio.time >= audio.clip.length - 0.1f)
        {
            changeTrack((int)Track.Next);
        }

        TitleFeild.text = nowMusic.Artist + " - " + nowMusic.Title;
    }

    //юсюг
    public void LoadMusicList()
    {
        musicList = new List<MusicInfo>();

        for (int i = 0; i < clips.Count; i++)
        {
            MusicInfo music = new MusicInfo(artist[i], title[i], clips[i]);
            musicList.Add(music);
        }

        musicList.Sort((x, y) => x.Artist.CompareTo(y.Artist));
    }

    public static void initMusic()
    {
        int track;

        track = Random.Range(0, musicList.Count);
        instance.disablePrevTrack();

        nowMusic = musicList[track];
        audio.clip = nowMusic.Clip;
        audio.Play();
    }

    public static void Pause()
    {
        if (isPause)
            audio.Play();
        isPause = !isPause;
    }

    public static void Stop()
    {
        audio.Stop();
        audio.time = 0;
        isPause = true;
    }

    public void disablePrevTrack()
    {
        prevBtn.active = false;
        Color color = prevBtnImage.color;
        color.a = 0.25f;
        prevBtnImage.color = color;
    }
    public void enablePrevTrack()
    {
        prevBtn.active = true;
        Color color = prevBtnImage.color;
        color.a = 1f;
        prevBtnImage.color = color;
    }

    public static void changeTrack(int type)
    {
        int track = musicList.IndexOf(nowMusic);
        if (track == -1)
        {
            initMusic();
            return;
        }

        if (type == 1)
        {
            nowMusic = MusicStack.Pop();
            if (MusicStack.Count == 0)
            {
                instance.disablePrevTrack();
            }
        }
        else
        {
            instance.enablePrevTrack();
            switch (nowCycle)
            {
                case Cycle.Cycle:
                    MusicStack.Push(musicList[track]);
                    track += 1;
                    if (track >= musicList.Count)
                        track = 0;
                    break;
                case Cycle.Random:
                    MusicStack.Push(musicList[track]);
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
            nowMusic = musicList[track];
        }

        audio.clip = nowMusic.Clip;
        audio.time = 0;
        audio.Play();
    }

    public struct MusicInfo
    {
        public MusicInfo(string _atrist, string _title, AudioClip _clip)
        {
            Artist = _atrist;
            Title = _title;
            Clip = _clip;
        }

        public string Artist { get; }
        public string Title { get; }
        public AudioClip Clip { get; }
    }
}
