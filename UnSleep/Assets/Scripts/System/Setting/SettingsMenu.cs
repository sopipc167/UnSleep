using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // ::이후 저장되어야 할 데이터 목록::
    //   1. Audio volume 설정 float 변수 3개
    //   2. Audio isMute 설정 bool 변수 3개
    //   3. Graphic 설정 int 변수 1개
    //   4. Resolution type 설정 int 변수 1개
    //   5. Screen type 설정 int 변수 1개

    [Header("소리 UI")]
    public Slider masterVolume;
    public Slider bgmVolume;
    public Slider seVolume;
    public Toggle masterMute;
    public Toggle bgmMute;
    public Toggle seMute;

    [Header("그래픽 UI")]
    public Dropdown qualityDropdown;

    [Header("화면 UI")]
    public Dropdown screenTypeDropdown;
    public Dropdown resolutionDropdown;

    private SoundManager soundManager;
    private List<string> options = new List<string>();
    private Resolution[] resolutions;

    private SystemOption data;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    private void OnDisable()
    {
        data.volume_master = masterVolume.value;
        data.volume_bgm = bgmVolume.value;
        data.volume_se = seVolume.value;
        data.mute_master = masterMute.isOn;
        data.mute_bgm = bgmMute.isOn;
        data.mute_se = seMute.isOn;
        SaveDataManager.Instance.SaveSystemOption(data);
    }

    private void Start()
    {
        data = SaveDataManager.Instance.LoadSystemOption();
        if (data == null)
        {
            data = new SystemOption();
            data.volume_master = 0.5f;
            data.volume_bgm = 0.5f;
            data.volume_se = 0.5f;
            data.mute_master = false;
            data.mute_bgm = false;
            data.mute_se = false;
            data.graphic = 0;
            data.screenType = 0;
            data.resolutionType = 0;
        }

        //Audio
        masterVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.Master, value));
        masterVolume.value = data.volume_master;
        bgmVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.BGM, value));
        bgmVolume.value = data.volume_bgm;
        seVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.SE, value));
        seVolume.value = data.volume_se;
        print(data.volume_master);
        print(masterVolume.value);

        masterMute.onValueChanged.AddListener(value => SetMute(SoundType.Master, value));
        masterMute.isOn = data.mute_master;
        bgmMute.onValueChanged.AddListener(value => SetMute(SoundType.BGM, value));
        bgmMute.isOn = data.mute_bgm;
        seMute.onValueChanged.AddListener(value => SetMute(SoundType.SE, value));
        seMute.isOn = data.mute_se;

        //Graphics
        qualityDropdown.ClearOptions();
        options.Add("Ultra");
        options.Add("Very High");
        options.Add("High");
        options.Add("Medium");
        options.Add("Low");
        options.Add("Very Low");
        qualityDropdown.AddOptions(options);
        qualityDropdown.onValueChanged.AddListener(value => SetQuality(value));
        qualityDropdown.value = data.graphic;

        //Screen - screen type
        screenTypeDropdown.ClearOptions();
        options.Clear();
        options.Add("Full Screen");
        options.Add("Borderless Window");
        options.Add("Window");
        screenTypeDropdown.AddOptions(options);
        screenTypeDropdown.onValueChanged.AddListener(value => SetScreen(value));
        screenTypeDropdown.value = data.screenType;

        //Screen - resolution
        options.Clear();
        resolutions = Screen.resolutions;
        for (int i = resolutions.Length - 1; i >= 0; --i)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
        }
        options = options.Distinct().ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.onValueChanged.AddListener(value => SetResolution(value));
        resolutionDropdown.value = data.resolutionType;

        SetScreen(screenTypeDropdown.value);
    }

    public void SetMute(SoundType type, bool isMute)
    {
        if (isMute)
        {
            soundManager.SetMute(type);
        }
        else
        {
            soundManager.Unmute(type);
        }
    }

    public void SetQuality(int qualityIdx)
    {
        QualitySettings.SetQualityLevel(5 - qualityIdx);
        data.graphic = 5 - qualityIdx;
    }

    public void SetResolution(int resolutionIdx)
    {
        Resolution resolution;
        resolution = resolutions[resolutions.Length - 1 - resolutionIdx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        data.resolutionType = resolutionIdx;
    }

    public void SetScreen(int screenIdx)
    {
#if UNITY_STANDALONE_WIN
        switch (screenIdx)
        {
            case 0: // Full Screen
                Application.runInBackground = false;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Borderless
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:    // Window
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
#endif

#if UNITY_STANDALONE_OSX
        switch (screenIdx)
        {
            case 0: // Full Screen
                Application.runInBackground = false;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1: // Borderless
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:    // Window
                Application.runInBackground = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
#endif
        data.screenType = screenIdx;
    }
}
