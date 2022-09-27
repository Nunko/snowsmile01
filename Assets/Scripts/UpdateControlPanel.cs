using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateControlPanel : MonoBehaviour
{
    float setR_BG;
    float screenW;
    float screenH;
    public GameObject ControlPanelContainer;

    void Start() 
    {
        SetVariable();
    }

    void Update()
    {        
        if (screenW != Screen.width || screenH != Screen.height)
        {
            screenW = Screen.width;
            screenH = Screen.height; 
            ChangeTop();
        }
    }

    void SetVariable()
    {        
        setR_BG = (float) 1/2;
        screenW = Screen.width;
        screenH = Screen.height;
        ChangeTop();
    }

    void ChangeTop()
    {
        float screenR = screenW/screenH;

        if (screenR < setR_BG)
        {
            float moveNum = 0f;
            if (screenH > 1920)
            {
                float byNum = setR_BG/screenR;
                moveNum = -(setR_BG-screenR)*1920*byNum;
            }
            else
            {
                moveNum = -(setR_BG-screenR)*1920;
            }
            ControlPanelContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0, moveNum);
        }
        else
        {
            ControlPanelContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
        Debug.Log("컨트롤박스 이동");
    }
}
