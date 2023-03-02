using System.Collections;
using System.Collections.Generic;
using com.cupelt.option;
using com.cupelt.util;
using UnityEngine;

public class OptionUIBuilder : MonoBehaviour
{
    private Option option => GetComponent<OptionManager>().option;

    private OptionManager manager => GetComponent<OptionManager>();

    public OptionUI createOptionUI()
    {
        OptionUI ui = new OptionUI();

        string optionStr = "option.language_option";

        ui.AddHeader($"{optionStr}.title");
        ui.AddSelection("option.language_option.language",
            new string[] {
                "en_us",
                "ko_kr"
            },
            () => { option.lang = (languageEnum)Util.changeEnum(typeof(languageEnum), (int)option.lang, -1); },
            () => { option.lang = (languageEnum)Util.changeEnum(typeof(languageEnum), (int)option.lang, 1); },
            nameof(option.lang));

        optionStr = "option.video_option";
        ui.AddHeader($"{optionStr}.title");
        ui.AddHeader($"{optionStr}.display_option.title", true);

        optionStr = "option.video_option.display_option";
        ui.AddSelection($"{optionStr}.display_mode",
            new string[] {
                "full_screen",
                "windowd",
                "borderless_windowed"
            },
            () => { option.displayMode = (displayModeEnum)Util.changeEnum(typeof(displayModeEnum), (int)option.displayMode, -1); },
            () => { option.displayMode = (displayModeEnum)Util.changeEnum(typeof(displayModeEnum), (int)option.displayMode, 1); },
            nameof(option.displayMode));

        Resolution[] fixedResolution = Util.getFixedResolutions();
        string[] resolutionRatio = new string[fixedResolution.Length];
        for (int i = 0; i < fixedResolution.Length; i++)
        {
            int[] ratio = Util.getResolutionRatio(fixedResolution[i].width, fixedResolution[i].height);
            resolutionRatio[i] = $"{fixedResolution[i].width} X {fixedResolution[i].height} ( {ratio[0]} : {ratio[1]} )";
        }
        ui.AddSelection($"{optionStr}.resolution.title",
            $"{optionStr}.resolution.description",
            resolutionRatio,
            () => { option.resolution = Util.changeCycleValue(resolutionRatio.Length, (int)option.resolution, -1); },
            () => { option.resolution = Util.changeCycleValue(resolutionRatio.Length, (int)option.resolution, 1); },
            nameof(option.resolution));
        ui.AddSelection($"{optionStr}.display_frame",
            new string[] {
                "synchronization",
                "unlimited",
                "custom"
            },
            () => { option.displayFrame = (displayFrameEnum)Util.changeEnum(typeof(displayFrameEnum), (int)option.displayFrame, -1); },
            () => { option.displayFrame = (displayFrameEnum)Util.changeEnum(typeof(displayFrameEnum), (int)option.displayFrame, 1); },
            nameof(option.displayFrame));
        ui.AddToggle($"{optionStr}.v_sync",
            (active) => { option.vsync = active; },
            nameof(option.vsync));
        ui.AddButton(new OptionUI.ButtonUI.ButtonObject[] { new OptionUI.ButtonUI.ButtonObject($"{optionStr}.apply", () => { option.applyResolution(); }) });

        optionStr = "option.video_option.graphic_option";
        ui.AddHeader($"{optionStr}.title", true);
        ui.AddSelection($"{optionStr}.texture_quality", false,
            () => { option.textureQuality = (TextureQualityEnum)Util.changeEnum(typeof(TextureQualityEnum), (int)option.textureQuality, -1); },
            () => { option.textureQuality = (TextureQualityEnum)Util.changeEnum(typeof(TextureQualityEnum), (int)option.textureQuality, 1); },
            nameof(option.textureQuality));
        ui.AddSelection($"{optionStr}.anti_aliasing.title",
            $"{optionStr}.anti_aliasing.description",
            new string[] {
                "general.active.disable",
                "MSAA",
                "FXAA",
                "SMAA"
            },
            () => { option.antiAliasing = (AntiAliasingEnum)Util.changeEnum(typeof(AntiAliasingEnum), (int)option.antiAliasing, -1); },
            () => { option.antiAliasing = (AntiAliasingEnum)Util.changeEnum(typeof(AntiAliasingEnum), (int)option.antiAliasing, 1); },
            nameof(option.antiAliasing));
        ui.AddSelection($"{optionStr}.bloom", true,
            () => { option.bloom = (BloomEnum)Util.changeEnum(typeof(BloomEnum), (int)option.bloom, -1); },
            () => { option.bloom = (BloomEnum)Util.changeEnum(typeof(BloomEnum), (int)option.bloom, 1); },
            nameof(option.bloom));

        optionStr = "option.other_option";
        ui.AddHeader($"{optionStr}.title");
        ui.AddToggle($"{optionStr}.show_frame",
            (active) => { option.isShowFrameRate = active; },
            nameof(option.isShowFrameRate));
        ui.AddToggle($"{optionStr}.show_time",
            (active) => { option.isShowTime = active; },
            nameof(option.isShowTime));

        return ui;
    }
    
    public void DrawOptionUI(OptionUI ui)
    {
        Vector3 totalPos = Vector3.up * (450f + 50f);

        OptionUI.UiType beforeType = OptionUI.UiType.Header;

        
        Transform optionParents = manager.parents;
        GameObject optionHead = Resources.Load<GameObject>("prefabs/ui/option/Text Head");
        GameObject optionSubHead = Resources.Load<GameObject>("prefabs/ui/option/Text SubHead");
        GameObject optionToggleUI = Resources.Load<GameObject>("prefabs/ui/toggle/Toggle");
        GameObject optionButtonUI = Resources.Load<GameObject>("prefabs/ui/button/Button");

        foreach (OptionUI.OptionUIStyle uiStyle in ui.ui)
        {
            float defaultGap = 105f;
            switch (beforeType)
            {
                case OptionUI.UiType.Header: defaultGap = 55f; break;
                case OptionUI.UiType.Button: defaultGap = 55f; break;
                default:            defaultGap = 115f; break;
            }
            totalPos.y -= defaultGap;

            switch (uiStyle.getStructType())
            {
                case OptionUI.UiType.Header:
                    OptionUI.Header uiHead = (OptionUI.Header)uiStyle;
                    GameObject head;

                    if (defaultGap == 115f) totalPos.y -= 50;

                    if (!uiHead._isSmall)   {   head = Instantiate(optionHead, optionParents);      }
                    else                    {   head = Instantiate(optionSubHead, optionParents);   }

                    head.GetComponent<RectTransform>().anchoredPosition3D = totalPos;
                    head.GetComponent<LocaleString>().setKey(uiHead._key);

                    head.SetActive(true);
                    break;
                case OptionUI.UiType.Selection:
                    OptionUI.SelectionUI uiSelection = (OptionUI.SelectionUI)uiStyle;

                    GameObject selection = CustomSelection.buildSelection(
                        optionParents,
                        totalPos,
                        uiSelection._key,
                        option.GetType(),
                        option,
                        uiSelection.value,
                        uiSelection.selectionKeys,
                        uiSelection.prev,
                        uiSelection.next,
                        () => option.applyOption());
                    selection.SetActive(true);
                    break;
                case OptionUI.UiType.Toggle:
                    OptionUI.ToggleUI uiToggle = (OptionUI.ToggleUI)uiStyle;
                    GameObject toggle = Instantiate(optionToggleUI, optionParents);

                    toggle.GetComponent<RectTransform>().anchoredPosition3D = totalPos;
                    toggle.GetComponent<LocaleString>().setKey(uiToggle._key);

                    //description add

                    CustomToggleButton btn = toggle.transform.GetChild(1).GetComponent<CustomToggleButton>();
                    btn.toggleActive = (bool)option.GetType().GetField(uiToggle.value).GetValue(option);

                    btn.onValueChanged.AddListener(uiToggle._setActiveFunc);
                    btn.onValueChanged.AddListener((x) => option.applyOption());

                    toggle.SetActive(true);
                    break;
                case OptionUI.UiType.Button:
                    OptionUI.ButtonUI uiButton = (OptionUI.ButtonUI)uiStyle;

                    for (int i = 0; i < uiButton._buttons.Length; i++)
                    {
                        GameObject button = Instantiate(optionButtonUI, optionParents);
                        Vector3 pos = totalPos;
                        pos.x = i * 160f;
                        button.GetComponent<RectTransform>().anchoredPosition3D = pos;
                        button.GetComponent<CustomButton>().onClick.AddListener(uiButton._buttons[i].action);
                        button.transform.GetChild(0).GetChild(0).GetComponent<LocaleString>().setKey(uiButton._buttons[i].text);

                        button.SetActive(true);
                    }
                    break;
            }
            beforeType = uiStyle.getStructType();
        }

        manager.optionLength = totalPos.y + 250;
    }
}
