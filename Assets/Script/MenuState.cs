using System;
using System.Collections;
using System.Collections.Generic;
using com.cupelt.util;
using UnityEngine;

public class MenuState
{
    protected readonly MenuManager manager;
    private Guid _uuid;

    protected MenuState()
    {
        this.manager = MenuManager.Instance;
        this._uuid = Guid.NewGuid();
    }

    public virtual IEnumerator resetState() { yield return null; }
    public virtual IEnumerator initState() { yield return null; }

    protected void unpreparedNextAnimation() { manager.readyQueue.Add(this); }
    protected void readyNextAnimation() { manager.readyQueue.Remove(this); }
}

public class Menu : MenuState
{
    public override IEnumerator initState()
    {
        unpreparedNextAnimation();
        
        manager.mainText.SetActive(false);
        manager.menu.transform.GetChild(0).gameObject.SetActive(true);
        manager.menu.active = true;
        manager.menu.freeMove = false;

        foreach (Animation m in manager.menu.animations)
        {
            m.setActive(true);
        }

        while (true)
        {
            bool isEnd = true;
            foreach (Animation m in manager.menu.animations)
            {
                if (!m.readyAnimation) isEnd = false;
            }

            if (isEnd) break;
            yield return null;
        }

        readyNextAnimation();
    }
    
    public override IEnumerator resetState()
    {
        unpreparedNextAnimation();
        
        manager.menu.active = false;
        if (manager.nowState.Count == 0) manager.menu.freeMove = false;
        else manager.menu.freeMove = true;
        
        foreach (Animation m in manager.menu.animations)
        {
            m.setActive(false);
        }

        while (true)
        {
            bool isEnd = true;
            foreach (Animation m in manager.menu.animations)
            {
                if (!m.readyAnimation) isEnd = false;
            }

            if (isEnd) break;
            yield return null;
        }
        
        manager.menu.transform.GetChild(0).gameObject.SetActive(false);

        readyNextAnimation();
    }
}

public class SinglePlayer : MenuState
{
    public override IEnumerator initState()
    {
        unpreparedNextAnimation();

        manager.playListObject.anchoredPosition3D = Vector3.up * 1080f;
        manager.playListObject.gameObject.SetActive(true);

        RectTransform trans = manager.playListObject;
        RectTransform selectBar = manager.menu.selectBar;

        Vector2 fixedSize = new Vector2(OptionManager.Instance.option.getResolution().width, 80f);
        
        Vector3 playListPos = trans.anchoredPosition3D;
        Vector3 selectPos = selectBar.anchoredPosition3D;
        Vector3 selectSizeDelta = selectBar.sizeDelta;
        
        float time = 0;
        while (time < 1)
        {
            selectBar.sizeDelta = Vector2.Lerp(selectSizeDelta, fixedSize, Tweening.OutQuart(time));
            
            time += Time.deltaTime / 0.5f;
            yield return null;
        }

        time = 0;
        while (time < 1)
        {
            manager.playListObject.anchoredPosition3D = Util.lerpVector(playListPos, Vector3.zero, Tweening.OutQuart(time));
            selectBar.anchoredPosition3D = Util.lerpVector(selectPos, Vector3.up * 515f, Tweening.OutQuart(time));

            time += Time.deltaTime / 0.8f;
            yield return null;
        }

        readyNextAnimation();
    }

    public override IEnumerator resetState()
    {
        unpreparedNextAnimation();

        Vector3 playListObject = manager.playListObject.anchoredPosition3D;
        Vector3 selectSizeDelta = manager.menu.selectBar.sizeDelta;

        float time = 0;
        while (time < 1)
        {
            manager.playListObject.anchoredPosition3D = Util.lerpVector(playListObject, Vector3.up * -1080f, Tweening.OutQuart(time));
            manager.menu.selectBar.sizeDelta = Util.lerpVector(selectSizeDelta, new Vector2(500f, 80f), Tweening.OutQuart(time));
            
            time += Time.deltaTime / 0.8f;
            yield return null;
        }
        manager.playListObject.gameObject.SetActive(false);

        readyNextAnimation();
    }
}


public class OptionMenu : MenuState
{
    public override IEnumerator initState()
    {
        unpreparedNextAnimation();

        manager.optionObject.anchoredPosition3D = Vector3.up * 1080f;
        manager.optionObject.gameObject.SetActive(true);

        RectTransform trans = manager.optionObject;

        Vector2 fixedSize = new Vector2(OptionManager.Instance.option.getResolution().width, 80f);
        
        Vector3 optionPos = trans.anchoredPosition3D;
        Vector3 selectPos = manager.menu.selectBar.anchoredPosition3D;
        Vector2 selectSizeDelta = manager.menu.selectBar.sizeDelta;
        
        float time = 0;
        while (time < 1)
        {
            manager.menu.selectBar.sizeDelta = Vector2.Lerp(selectSizeDelta, fixedSize, Tweening.OutQuart(time));
            
            time += Time.deltaTime / 0.5f;
            yield return null;
        }

        time = 0;
        while (time < 1)
        {
            manager.optionObject.anchoredPosition3D = Util.lerpVector(optionPos, Vector3.zero, Tweening.OutQuart(time));
            manager.menu.selectBar.anchoredPosition3D = Util.lerpVector(selectPos, Vector3.up * 515f, Tweening.OutQuart(time));

            time += Time.deltaTime / 0.8f;
            yield return null;
        }

        readyNextAnimation();
    }

    public override IEnumerator resetState()
    {
        unpreparedNextAnimation();

        Vector3 optionObject = manager.optionObject.anchoredPosition3D;
        Vector3 selectSizeDelta = manager.menu.selectBar.sizeDelta;

        float time = 0;
        while (time < 1)
        {
            manager.optionObject.anchoredPosition3D = Vector3.Lerp(optionObject, Vector3.up * -1040f, Tweening.OutQuart(time));
            manager.menu.selectBar.sizeDelta = Vector2.Lerp(selectSizeDelta, new Vector2(500f, 80f), Tweening.OutQuart(time));
            
            time += Time.deltaTime / 0.8f;
            yield return null;
        }
        manager.optionObject.gameObject.SetActive(false);

        readyNextAnimation();
    }
}
