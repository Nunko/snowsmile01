using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Naninovel;
using Naninovel.UI;

public class ControlPanelContainer : MonoBehaviour
{
    public GameObject ControlPanel;
    IScriptPlayer scriptPlayer;
    bool isOn;

    void OnEnable()
    {
        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();  
        string mode = variableManager.GetVariableValue("g_Mode");
        if (mode == "Tutorial")
        {
            ControlPanel.GetComponent<CanvasGroup>().interactable = false;
            ControlPanel.GetComponent<CanvasGroup>().alpha = 0;
            ControlPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        scriptPlayer = Engine.GetService<IScriptPlayer>();
        isOn = false;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape) == true)
            {
                if (scriptPlayer.Playing == true)
                {
                    IUIManager uiManager = Engine.GetService<IUIManager>();
                    if (isOn == false)
                    {           
                        isOn = true;             
                        uiManager.GetUI<IPauseUI>()?.Hide();
                        uiManager.GetUI<IBacklogUI>()?.Show();
                    }
                    else
                    {                        
                        uiManager.GetUI<IBacklogUI>()?.Hide();
                        isOn = false;
                    }                    
                }
            }
        }
    }
}
