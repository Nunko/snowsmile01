using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResize : MonoBehaviour
{
    public GameObject UICanvas;
    float screenW;
    float screenH;

    void Start()
    {
        screenW = Screen.width;
        screenH = Screen.height;
        Resize();        
    }

    void Update()
    {
        if (screenW != Screen.width || screenH != Screen.height)
        {
            screenW = Screen.width;
            screenH = Screen.height;
            Resize();
            Debug.Log("화면 크기 변화로 캔버스 스케일러 설정 변화");
        }
    }

    void Resize()
    {
        float setR = (float) 1/2f;
        float screenR = (float) screenW/screenH;
        float scaleNum =  screenR/setR;
        if (setR >= screenR)
        {
            UICanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
        else
        {
            UICanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
    }
}
