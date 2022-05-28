using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Externals
    public PlayerConfigurationSO playerConfigurationSO;
    public GameObject pauseUIGO;
    public GameObject controlsUIGO;
    public Slider volumeSlider, sensitivitySlider;
    
    // Internals
    private AudioListener audioListener;
    private bool toggled;
    private bool initialized = false;

    private void Awake()
    {
        AudioListener.volume = PlayerConfigurationSO.Volume;
        sensitivitySlider.value = PlayerConfigurationSO.CameraSensitivity;
        volumeSlider.value = PlayerConfigurationSO.Volume;
        initialized = true;
        toggled = false;
        TogglePauseMenu();
    }

    private void Start()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (playerConfigurationSO.gameEnd)
            return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            controlsUIGO.SetActive(!controlsUIGO.activeSelf);
        }
    }

    private void TogglePauseMenu()
    {
        Time.timeScale = toggled ? 0 : 1;
        Cursor.lockState = toggled ? CursorLockMode.None : CursorLockMode.Locked;
        pauseUIGO.SetActive(toggled);
        toggled = !toggled;
    }
    
    public void SetAudioVolume()
    {
        if (!initialized)
            return;
        
        //mixer.SetFloat("MusicVol", Mathf.Log10(volumeSlider.value) * 20);
        PlayerConfigurationSO.Volume = volumeSlider.value;
        AudioListener.volume = PlayerConfigurationSO.Volume;
    }

    public void TestAudio()
    {
        FindObjectOfType<AudioManager>().PlayAnAmbientSound(true);
    }

    public void SetSensitivity()
    {
        if (!initialized)
            return;
        
        PlayerConfigurationSO.CameraSensitivity = sensitivitySlider.value;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
