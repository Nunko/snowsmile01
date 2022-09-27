using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;
using TMPro;

public class Configuration : MonoBehaviour
{
    IAudioManager audioManager;
    IStateManager settingsManager;
    public GameObject MusicVolumeText;
    bool isChanged;

    void Awake()
    {
        audioManager = Engine.GetService<IAudioManager>();
        settingsManager = Engine.GetService<IStateManager>();   
        isChanged = false;  
    }

    void Update()
    {
        if (isChanged == true)
        {
            ChangeBGMText();
        }        
    }

    void OnApplicationQuit()
    {
        if (gameObject.activeSelf == true)
        {
            SaveSetting();
        }
    }

    void ChangeBGMText()
    {
        float inputNumberF = audioManager.BgmVolume*100;
        int inputNumber = (int)inputNumberF;        
        if (MusicVolumeText.GetComponent<TextMeshProUGUI>().text != inputNumber.ToString())
        {
            MusicVolumeText.GetComponent<TextMeshProUGUI>().text = inputNumber.ToString(); 
        }                          
    }

    public void SaveSetting()
    {
        settingsManager.SaveSettingsAsync();
        Debug.Log("환경설정: 설정 저장!");     
        isChanged = false;     
    }

    public void SliderOnChanged()
    {
        if (isChanged == false)
        {
            isChanged = true;
        }
    }
}
