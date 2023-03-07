using System.Collections;
using System.Collections.Generic;
using com.cupelt.sqarebeat;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    public static MusicSelector nowSel;
    public BeatMap map;
    void Update()
    {
        BeatMap nowMusic = AudioManager.Instance.nowMusic;
        RectTransform trans = transform.GetChild(2).GetComponent<RectTransform>();
        
        if (nowMusic.Equals(map))
        {
            trans.sizeDelta = Vector2.Lerp(trans.sizeDelta, new Vector2(800f, 40), Time.deltaTime * 8f);
            nowSel = this;
        }
        else
        {
            trans.sizeDelta = Vector2.Lerp(trans.sizeDelta, new Vector2(-40f, 40), Time.deltaTime * 8f);
        }
    }
}
