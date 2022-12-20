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
    public static AudioManager instance;
    public static Cycle nowCycle = Cycle.Cycle;

    public List<AudioClip> clips;
    public List<string> artist;
    public List<string> title;

    public static Stack<MusicInfo> MusicStack = new Stack<MusicInfo>();
    public static List<MusicInfo> musicList;

    public GameObject PlayBtn;
    public GameObject PauseBtn;

    public GameObject[] CycleBtns;

    public SimpleButton prevBtn;
    public Image prevBtnImage;

    public Text TitleFeild;

    public static MusicInfo nowMusic;

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
        int track = 9; //Random.Range(0, musicList.Count);
        instance.activePrevTrack(false);

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

    public void activePrevTrack(bool active)
    {
        prevBtn.active = active;
        Color color = prevBtnImage.color;
        if (active)
        {
            color.a = 1f;
        }
        else
        {
            color.a = 0.5f;
        }
        prevBtnImage.color = color;
    }

    public void changeCycle()
    {
        int length = System.Enum.GetValues(typeof(Cycle)).Length;
        if ((int)nowCycle + 1 >= length)
        {
            nowCycle = 0;
        }
        else
        {
            nowCycle = nowCycle + 1;
        }

        for (int i = 0; i < length; i++)
        {
            if (nowCycle.Equals((Cycle)i))
                CycleBtns[i].SetActive(true);
            else
                CycleBtns[i].SetActive(false);
        }
    }

    public static void changeTrack(int type)
    {
        int track = musicList.IndexOf(nowMusic);
        MusicInfo music;
        if (track == -1)
        {
            initMusic();
            return;
        }

        if (type == 1)
        {
            music = MusicStack.Pop();
            if (MusicStack.Count == 0)
            {
                instance.activePrevTrack(false);
            }
        }
        else
        {
            instance.activePrevTrack(true);
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
            music = musicList[track];
        }
        setTrack(music);
    }

    public static void setTrack(MusicInfo music)
    {
        nowMusic = music;

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
