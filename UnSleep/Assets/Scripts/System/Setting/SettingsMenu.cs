using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("소리 UI")]
    public Slider[] volume;
    public Toggle[] isMute;

    [Header("그래픽 UI")]
    public Dropdown qualityDropdown;

    [Header("화면 UI")]
    public Dropdown screenTypeDropdown;
    public Dropdown resolutionDropdown;

    private SoundManager soundManager;
    private List<Resolution> resolutions = new List<Resolution>();

    private SystemOption data;


    public void Save()
    {
        if (data == null) return;
        data.volume_master = volume[0].value;
        data.volume_bgm = volume[1].value;
        data.volume_se = volume[2].value;
        data.mute_master = isMute[0].isOn;
        data.mute_bgm = isMute[1].isOn;
        data.mute_se = isMute[2].isOn;
        data.graphic = qualityDropdown.value;
        data.screenType = screenTypeDropdown.value;
        data.resolutionType = resolutionDropdown.value;
        SaveDataManager.Instance.SaveSystemOption(data);
    }


    private void Start()
    {
        soundManager = SoundManager.Instance;
        data = SaveDataManager.Instance.LoadSystemOption();
        if (data == null)
        {
            data = new SystemOption();
            data.volume_master = 0.7f;
            data.volume_bgm = 0.7f;
            data.volume_se = 0.7f;
            data.mute_master = false;
            data.mute_bgm = false;
            data.mute_se = false;
            data.graphic = 0;
            data.screenType = 0;
            data.resolutionType = 0;
        }

        //Audio
        volume[0].onValueChanged.AddListener((value) => soundManager.SetVolume(SoundType.Master, value));
        volume[0].value = data.volume_master;
        volume[1].onValueChanged.AddListener((value) => soundManager.SetVolume(SoundType.BGM, value));
        volume[1].value = data.volume_bgm;
        volume[2].onValueChanged.AddListener((value) => soundManager.SetVolume(SoundType.SE, value));
        volume[2].value = data.volume_se;

        isMute[0].onValueChanged.AddListener(value => soundManager.SetMute(SoundType.Master, value));
        isMute[0].isOn = data.mute_master;
        isMute[1].onValueChanged.AddListener(value => soundManager.SetMute(SoundType.BGM, value));
        isMute[1].isOn = data.mute_bgm;
        isMute[2].onValueChanged.AddListener(value => soundManager.SetMute(SoundType.SE, value));
        isMute[2].isOn = data.mute_se;

        // Toggle은 기본 setting이 true이고, 만약 isOn이 true라면 함수호출을 안 함
        soundManager.SetVolume(SoundType.Master, data.volume_master);
        soundManager.SetVolume(SoundType.BGM, data.volume_bgm);
        soundManager.SetVolume(SoundType.SE, data.volume_se);
        soundManager.SetMute(SoundType.Master, data.mute_master);
        soundManager.SetMute(SoundType.BGM, data.mute_bgm);
        soundManager.SetMute(SoundType.SE, data.mute_se);

        //Graphics
        List<string> qualityOptions = new List<string>(6);
        qualityDropdown.ClearOptions();
        qualityOptions.Add("최고");
        qualityOptions.Add("매우 높음");
        qualityOptions.Add("높음");
        qualityOptions.Add("중간");
        qualityOptions.Add("낮음");
        qualityOptions.Add("매우 낮음");
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.onValueChanged.AddListener(value => SetQuality(value));
        qualityDropdown.value = data.graphic;

        //Screen - resolution
        List<string> resolutionOptions = new List<string>();
        HashSet<int> widthSet = new HashSet<int>();
        var allResolutions = Screen.resolutions;
        for (int i = allResolutions.Length - 1; i >= 0; --i)
        {
            if (!widthSet.Contains(allResolutions[i].width))
            {
                if (allResolutions[i].width * 9 == allResolutions[i].height * 16)
                {
                    resolutions.Add(allResolutions[i]);
                    resolutionOptions.Add(allResolutions[i].width + " x " + allResolutions[i].height);
                    widthSet.Add(allResolutions[i].width);
                }
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.onValueChanged.AddListener(value => SetResolution(value));
        resolutionDropdown.value = data.resolutionType;

        //Screen - screen type
        List<string> screenOptions = new List<string>(3);
        screenTypeDropdown.ClearOptions();
        screenOptions.Add("전체 화면 모드");
        screenOptions.Add("테두리 없는 창모드");
        screenOptions.Add("창모드");
        screenTypeDropdown.AddOptions(screenOptions);
        screenTypeDropdown.onValueChanged.AddListener(value => SetScreen(value));
        screenTypeDropdown.value = data.screenType;
    }

    private void SetQuality(int qualityIdx)
    {
        QualitySettings.SetQualityLevel(qualityDropdown.options.Count - 1 - qualityIdx);
        data.graphic = qualityDropdown.options.Count - 1 - qualityIdx;
    }

    private void SetResolution(int resolutionIdx, FullScreenMode mode = FullScreenMode.Windowed)
    {
        if (mode == FullScreenMode.Windowed)
        {
            var resolution = resolutions[resolutionIdx];
            Screen.SetResolution(resolution.width, resolution.height, mode);
            data.resolutionType = resolutionIdx;
        }
        else
        {
            var resolution = resolutions[0];
            Screen.SetResolution(resolution.width, resolution.height, mode);
            data.resolutionType = resolutionIdx;
        }
    }

    private void SetScreen(int screenIdx)
    {
#if UNITY_STANDALONE_WIN
        switch (screenIdx)
        {
            case 0: // Full Screen
                Application.runInBackground = false;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                SetResolution(0, FullScreenMode.ExclusiveFullScreen);
                resolutionDropdown.interactable = false;
                break;
            case 1: // Borderless
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                SetResolution(0, FullScreenMode.FullScreenWindow);
                resolutionDropdown.interactable = false;
                break;
            case 2:    // Window
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                resolutionDropdown.interactable = true;
                break;
        }
#endif

#if UNITY_STANDALONE_OSX
        switch (screenIdx)
        {
            case 0: // Full Screen
                Application.runInBackground = false;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                SetResolution(0, FullScreenMode.FullScreenWindow);
            resolutionDropdown.interactable = false;
                break;
            case 1: // Borderless
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                SetResolution(0, FullScreenMode.MaximizedWindow);
            resolutionDropdown.interactable = false;
                break;
            case 2:    // Window
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;
            resolutionDropdown.interactable = true;
                break;
        }
#endif

        data.screenType = screenIdx;
    }
}
