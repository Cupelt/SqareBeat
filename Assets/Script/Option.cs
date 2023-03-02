using System.Collections;
using System.Collections.Generic;
using System.IO;
using com.cupelt.util;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace com.cupelt.option
{
    //Language Option -
    public enum languageEnum        { en_us }

    //Graphic Option -
    //Diaply Option
    public enum displayModeEnum     { fullScreen, windowd, borderlessWindowed }
    public enum displayFrameEnum    { Synchronization, Unlimited, Custom }

    //Graphic Option
    public enum presetQualityEnum   { Low, Middle, high, Custom } // ?

    public enum TextureQualityEnum  { Low, Middle, high }
    public enum AntiAliasingEnum    { Disable, MSAA, FXAA, SMAA }
    public enum BloomEnum           { Disable, Low, Middle, High }
    //Other Setting
    
    [System.Serializable]
    public class Option
    {
        public languageEnum lang = languageEnum.en_us;

        public displayModeEnum displayMode = displayModeEnum.fullScreen;
        public displayFrameEnum displayFrame = displayFrameEnum.Synchronization;
        public int resolution = 0;
        public int width;
        public int height;
        public int maxFrameRate = -1;
        public bool vsync = false;

        public TextureQualityEnum textureQuality = TextureQualityEnum.high;
        public AntiAliasingEnum antiAliasing = AntiAliasingEnum.FXAA;
        public BloomEnum bloom = BloomEnum.Middle;

        public bool isShowFrameRate = false;
        public bool isShowTime = false;

        public void save()
        {
            string path = Application.persistentDataPath + "/Option.json";
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public static Option load()
        {
            string path = Application.persistentDataPath + "/Option.json";
            
            if (File.Exists(path))
            {
                new Option().save();
            }

            string json = File.ReadAllText(path);

            Option o = JsonUtility.FromJson<Option>(json);
            o.resolution = 0;

            Resolution[] resolutions = Util.getFixedResolutions();

            for (int res = 0; res < resolutions.Length; res++)
                if (o.width.Equals(resolutions[res].width) && o.height.Equals(resolutions[res].height))
                {
                    o.resolution = res;
                    break;
                }

            return o;
        }
        
        public void applyResolution()
        {
            Resolution[] resolutions = Util.getFixedResolutions();
            switch (displayMode)
            {
                case displayModeEnum.fullScreen: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.ExclusiveFullScreen); break;
                case displayModeEnum.windowd: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.Windowed); break;
                case displayModeEnum.borderlessWindowed: Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, FullScreenMode.FullScreenWindow); break;
            }
            width = resolutions[resolution].width;
            height = resolutions[resolution].height;
            save();
        }

        public void applyAudio()
        {
            
        }
        
        public void applyOption()
        {
            //Language Option -
            Localization.locale = lang.ToString();
            Localization.LoadLocale();
            Localization.applyLocale();

            //Graphic Option -
            //Diaply Option

            switch (displayFrame)
            {
                case displayFrameEnum.Synchronization: Application.targetFrameRate = Screen.currentResolution.refreshRate; break;
                case displayFrameEnum.Unlimited: Application.targetFrameRate = 3000; break;
                case displayFrameEnum.Custom: Application.targetFrameRate = maxFrameRate; break;
            }

            //Graphic
            switch (textureQuality)
            {
                case TextureQualityEnum.high: QualitySettings.masterTextureLimit = 0; break;
                case TextureQualityEnum.Middle: QualitySettings.masterTextureLimit = 1; break;
                case TextureQualityEnum.Low: QualitySettings.masterTextureLimit = 2; break;
            }

            PostProcessLayer.Antialiasing antiMode = PostProcessLayer.Antialiasing.None;
            switch (antiAliasing)
            {
                case AntiAliasingEnum.FXAA: antiMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing; break;
                case AntiAliasingEnum.SMAA: antiMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing; break;
            }
            if (antiAliasing.Equals(AntiAliasingEnum.MSAA))
            {
                QualitySettings.antiAliasing = 4;
            }
            else
            {
                QualitySettings.antiAliasing = 0;
                foreach (Camera cam in Camera.allCameras)
                {
                    PostProcessLayer postProcess = cam.gameObject.GetComponent<PostProcessLayer>();
                    if (postProcess)
                    {
                        postProcess.antialiasingMode = antiMode;
                    }
                }
            }

            if (vsync) QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;


            foreach (Camera cam in Camera.allCameras)
            {
                Bloom postBloom;
                cam.gameObject.GetComponent<PostProcessVolume>().profile.TryGetSettings(out postBloom);
                if (postBloom)
                {
                    float intensity = 0f;

                    switch (bloom)
                    {
                        case BloomEnum.Low: intensity = 1f; break;
                        case BloomEnum.Middle: intensity = 2.5f; break;
                        case BloomEnum.High: intensity = 5f; break;
                    }
                    postBloom.intensity.value = intensity;
                }
            }

            //Other Option

            save();
        }
    }
}