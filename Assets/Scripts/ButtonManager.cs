using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPane;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider vfxSlider;
    
    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnReset;
    [SerializeField] private Button btnGuide;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnMusic;
    [SerializeField] private Button btnSoundFx;
    public List<Button> btnLevels = new List<Button>();

    [SerializeField] private AudioManager audioManager;
    
    public static int LevelChoose = 1;
    
    
    private void Start()
    {
        if (btnHome) btnHome.onClick.AddListener(() => OnButtonClicked(btnHome.name));
        if (btnPlay) btnPlay.onClick.AddListener(() => OnButtonClicked(btnPlay.name));
        if (btnReset) btnReset.onClick.AddListener(() => OnButtonClicked(btnReset.name));
        if (btnGuide) btnGuide.onClick.AddListener(() => OnButtonClicked(btnGuide.name));
        if (btnSetting) btnSetting.onClick.AddListener(() => OnButtonClicked(btnSetting.name));
        if (btnMusic) btnMusic.onClick.AddListener(() => OnButtonClicked(btnMusic.name));
        if (btnSoundFx) btnSoundFx.onClick.AddListener(() => OnButtonClicked(btnSoundFx.name));
        
        if (musicSlider) musicSlider.onValueChanged.AddListener((value) => OnValueChanged(musicSlider.name));
        if (vfxSlider) vfxSlider.onValueChanged.AddListener((value) => OnValueChanged(vfxSlider.name));
        
        foreach (Button button in btnLevels)
        {
            if (button != null) button.onClick.AddListener(() => OnButtonLevelClicked(button));
        }
    }
    
    private void OnButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "btnHome":
                audioManager.PlayClickClip();
                SceneManager.LoadScene("Menu");
                break;
            case "btnPlay":
                audioManager.PlayClickClip();
                SceneManager.LoadScene("SelectLevel");
                break;
            case "btnReset":
                audioManager.PlayClickClip();
                SceneManager.LoadScene("GamePlay");
                break;
            case "btnGuide":
                audioManager.PlayClickClip();
                SceneManager.LoadScene("Guide");
                break;
            case "btnMusic":
                audioManager.PlayClickClip();
                float newVolumeMusic = Math.Abs(audioManager.GetVolumeMusic()) <= 0f ? 0.5f : 0f;
                audioManager.SetVolumeMusic(newVolumeMusic);
                musicSlider.value = newVolumeMusic;
                Debug.Log("Music");
                break;
            case "btnSoundVfx":
                audioManager.PlayClickClip();
                float newVolumeVfx = Math.Abs(audioManager.GetVolumeVfx()) <= 0f ? 0.5f : 0f;
                audioManager.SetVolumeVfx(newVolumeVfx);
                vfxSlider.value = newVolumeVfx;
                Debug.Log("Vfx");
                break;
            case "btnSetting":
                audioManager.PlayClickClip();
                settingPane.gameObject.SetActive(!settingPane.activeSelf);
                Debug.Log("musicValue: " + musicSlider.value + " vfxValue: " + vfxSlider.value);
                Debug.Log("setting");
                break;
        }
    }

    void OnValueChanged(string sliderName)
    {
        switch (sliderName)
        {
            case "musicSlider":
                audioManager.SetVolumeMusic(musicSlider.value);
                break;
            case "vfxSlider":
                audioManager.SetVolumeVfx(vfxSlider.value);
                break;
        }
    }

    private void OnButtonLevelClicked(Button button)
    {
        audioManager.PlayClickClip();
        SceneManager.LoadScene("GamePlay");
        LevelChoose = int.Parse(button.GetComponentInChildren<Text>().text);
    }
}
