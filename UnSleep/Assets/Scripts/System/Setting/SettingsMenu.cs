using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // 싱글톤을 참조해야 하는데 (SoundManager)
    // 설정도 항상 언제든 나올 수 있으니까 싱글톤에 들어가겠죠..?
    // 일단 해당 클래스는 싱글톤으로 안 만들고 그냥 냅둘게요

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

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    private void Start()
    {
        //Audio
        masterVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.Master, value));
        masterVolume.value = 0.7f;
        bgmVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.BGM, value));
        bgmVolume.value = 0.7f;
        seVolume.onValueChanged.AddListener(value => soundManager.SetVolume(SoundType.SE, value));
        seVolume.value = 0.7f;

        masterMute.onValueChanged.AddListener(value => SetMute(SoundType.Master, value));
        masterMute.isOn = false;
        bgmMute.onValueChanged.AddListener(value => SetMute(SoundType.BGM, value));
        bgmMute.isOn = false;
        seMute.onValueChanged.AddListener(value => SetMute(SoundType.SE, value));
        seMute.isOn = false;

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
        qualityDropdown.value = 0;

        //Screen - screen type
        screenTypeDropdown.ClearOptions();
        options.Clear();
        options.Add("Full Screen");
        options.Add("Borderless Window");
        options.Add("Window");
        screenTypeDropdown.AddOptions(options);
        screenTypeDropdown.onValueChanged.AddListener(value => SetScreen(value));
        screenTypeDropdown.value = 0;

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
        resolutionDropdown.value = 0;

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
            switch (type)
            {
                case SoundType.Master:
                    soundManager.Unmute(type, masterVolume.value);
                    break;
                case SoundType.BGM:
                    soundManager.Unmute(type, bgmVolume.value);
                    break;
                case SoundType.SE:
                    soundManager.Unmute(type, seVolume.value);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetQuality(int qualityIdx)
    {
        QualitySettings.SetQualityLevel(5 - qualityIdx);
    }

    public void SetResolution(int resolutionIdx)
    {
        Resolution resolution;
        resolution = resolutions[resolutions.Length - 1 - resolutionIdx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
    }
}
