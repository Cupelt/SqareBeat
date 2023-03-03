using System.Collections;
using System.Collections.Generic;
using com.cupelt.option;
using com.cupelt.util;
using UnityEngine;

public class OptionUIBuilder : MonoBehaviour
{
    private Option Option => GetComponent<OptionManager>().option;

    private OptionManager Manager => GetComponent<OptionManager>();

    public OptionUI createOptionUI()
    {
        OptionUI ui = new OptionUI();

        string optionStr = "option.language_option";

        ui.addHeader($"{optionStr}.title");
        ui.addSelection("option.language_option.language",
            new string[] {
                "en_us",
                "ko_kr"
            },
            () => { Option.lang = (LanguageEnum)Util.changeEnum(typeof(LanguageEnum), (int)Option.lang, -1); },
            () => { Option.lang = (LanguageEnum)Util.changeEnum(typeof(LanguageEnum), (int)Option.lang, 1); },
            nameof(Option.lang));

        optionStr = "option.video_option";
        ui.addHeader($"{optionStr}.title");
        ui.addHeader($"{optionStr}.display_option.title", true);

        optionStr = "option.video_option.display_option";
        ui.addSelection($"{optionStr}.display_mode",
            new string[] {
                "full_screen",
                "windowd",
                "borderless_windowed"
            },
            () => { Option.displayMode = (DisplayModeEnum)Util.changeEnum(typeof(DisplayModeEnum), (int)Option.displayMode, -1); },
            () => { Option.displayMode = (DisplayModeEnum)Util.changeEnum(typeof(DisplayModeEnum), (int)Option.displayMode, 1); },
            nameof(Option.displayMode));

        Resolution[] fixedResolution = Util.getFixedResolutions();
        string[] resolutionRatio = new string[fixedResolution.Length];
        for (int i = 0; i < fixedResolution.Length; i++)
        {
            int[] ratio = Util.getResolutionRatio(fixedResolution[i].width, fixedResolution[i].height);
            resolutionRatio[i] = $"{fixedResolution[i].width} X {fixedResolution[i].height} ( {ratio[0]} : {ratio[1]} )";
        }
        ui.addSelection($"{optionStr}.resolution.title",
            $"{optionStr}.resolution.description",
            resolutionRatio,
            () => { Option.resolution = Util.changeCycleValue(resolutionRatio.Length, (int)Option.resolution, -1); },
            () => { Option.resolution = Util.changeCycleValue(resolutionRatio.Length, (int)Option.resolution, 1); },
            nameof(Option.resolution));
        ui.addSelection($"{optionStr}.display_frame",
            new string[] {
                "synchronization",
                "unlimited",
                "custom"
            },
            () => { Option.displayFrame = (DisplayFrameEnum)Util.changeEnum(typeof(DisplayFrameEnum), (int)Option.displayFrame, -1); },
            () => { Option.displayFrame = (DisplayFrameEnum)Util.changeEnum(typeof(DisplayFrameEnum), (int)Option.displayFrame, 1); },
            nameof(Option.displayFrame));
        ui.AddToggle($"{optionStr}.v_sync",
            (Active) => { Option.vsync = Active; },
            nameof(Option.vsync));
        ui.AddButton(new OptionUI.ButtonUI.ButtonObject[] { new OptionUI.ButtonUI.ButtonObject($"{optionStr}.apply", () => { Option.applyResolution(); }) });

        optionStr = "option.video_option.graphic_option";
        ui.addHeader($"{optionStr}.title", true);
        ui.addSelection($"{optionStr}.texture_quality", false,
            () => { Option.textureQuality = (TextureQualityEnum)Util.changeEnum(typeof(TextureQualityEnum), (int)Option.textureQuality, -1); },
            () => { Option.textureQuality = (TextureQualityEnum)Util.changeEnum(typeof(TextureQualityEnum), (int)Option.textureQuality, 1); },
            nameof(Option.textureQuality));
        ui.addSelection($"{optionStr}.anti_aliasing.title",
            $"{optionStr}.anti_aliasing.description",
            new string[] {
                "general.active.disable",
                "MSAA",
                "FXAA",
                "SMAA"
            },
            () => { Option.antiAliasing = (AntiAliasingEnum)Util.changeEnum(typeof(AntiAliasingEnum), (int)Option.antiAliasing, -1); },
            () => { Option.antiAliasing = (AntiAliasingEnum)Util.changeEnum(typeof(AntiAliasingEnum), (int)Option.antiAliasing, 1); },
            nameof(Option.antiAliasing));
        ui.addSelection($"{optionStr}.bloom", true,
            () => { Option.bloom = (BloomEnum)Util.changeEnum(typeof(BloomEnum), (int)Option.bloom, -1); },
            () => { Option.bloom = (BloomEnum)Util.changeEnum(typeof(BloomEnum), (int)Option.bloom, 1); },
            nameof(Option.bloom));

        optionStr = "option.other_option";
        ui.addHeader($"{optionStr}.title");
        ui.AddToggle($"{optionStr}.show_frame",
            (active) => { Option.isShowFrameRate = active; },
            nameof(Option.isShowFrameRate));
        ui.AddToggle($"{optionStr}.show_time",
            (active) => { Option.isShowTime = active; },
            nameof(Option.isShowTime));

        return ui;
    }
    
    public void drawOptionUI(OptionUI UI)
    {
        Vector3 totalPos = Vector3.up * (450f + 50f);

        OptionUI.UiType beforeType = OptionUI.UiType.Header;

        
        Transform optionParents = Manager.parents;
        GameObject optionHead = Resources.Load<GameObject>("prefabs/ui/option/Text Head");
        GameObject optionSubHead = Resources.Load<GameObject>("prefabs/ui/option/Text SubHead");
        GameObject optionToggleUI = Resources.Load<GameObject>("prefabs/ui/toggle/Toggle");
        GameObject optionButtonUI = Resources.Load<GameObject>("prefabs/ui/button/Button");

        foreach (OptionUI.OptionUIStyle uiStyle in UI.ui)
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

                    if (defaultGap.Equals(115f)) totalPos.y -= 50;

                    if (!uiHead.isSmall)   {   head = Instantiate(optionHead, optionParents);      }
                    else                    {   head = Instantiate(optionSubHead, optionParents);   }

                    head.GetComponent<RectTransform>().anchoredPosition3D = totalPos;
                    head.GetComponent<LocaleString>().setKey(uiHead.key);

                    head.SetActive(true);
                    break;
                case OptionUI.UiType.Selection:
                    OptionUI.SelectionUI uiSelection = (OptionUI.SelectionUI)uiStyle;

                    GameObject selection = CustomSelection.buildSelection(
                        optionParents,
                        totalPos,
                        uiSelection.key,
                        Option.GetType(),
                        Option,
                        uiSelection.value,
                        uiSelection.selectionKeys,
                        uiSelection.prev,
                        uiSelection.next,
                        () => Option.applyOption());
                    selection.SetActive(true);
                    break;
                case OptionUI.UiType.Toggle:
                    OptionUI.ToggleUI uiToggle = (OptionUI.ToggleUI)uiStyle;
                    GameObject toggle = Instantiate(optionToggleUI, optionParents);

                    toggle.GetComponent<RectTransform>().anchoredPosition3D = totalPos;
                    toggle.GetComponent<LocaleString>().setKey(uiToggle.key);

                    //description add

                    CustomToggleButton btn = toggle.transform.GetChild(1).GetComponent<CustomToggleButton>();
                    btn.toggleActive = (bool)Option.GetType().GetField(uiToggle.value).GetValue(Option);

                    btn.onValueChanged.AddListener(uiToggle.setActiveFunc);
                    btn.onValueChanged.AddListener((X) => Option.applyOption());

                    toggle.SetActive(true);
                    break;
                case OptionUI.UiType.Button:
                    OptionUI.ButtonUI uiButton = (OptionUI.ButtonUI)uiStyle;

                    for (int i = 0; i < uiButton.buttons.Length; i++)
                    {
                        GameObject button = Instantiate(optionButtonUI, optionParents);
                        Vector3 pos = totalPos;
                        pos.x = i * 160f;
                        button.GetComponent<RectTransform>().anchoredPosition3D = pos;
                        button.GetComponent<CustomButton>().onClick.AddListener(uiButton.buttons[i].action);
                        button.transform.GetChild(0).GetChild(0).GetComponent<LocaleString>().setKey(uiButton.buttons[i].text);

                        button.SetActive(true);
                    }
                    break;
            }
            beforeType = uiStyle.getStructType();
        }

        Manager.optionLength = totalPos.y + 250;
    }
}
