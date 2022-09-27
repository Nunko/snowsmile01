using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;

public class BtnSkip : MonoBehaviour
{
    public Image targetImage;
    public Sprite targetOff;
    public Sprite targetOn;
    IScriptPlayer scriptPlayer;

    void Start()
    {
        scriptPlayer = Engine.GetService<IScriptPlayer>();  
    }

    void Update()
    {
        if (scriptPlayer.SkipActive == true && targetImage.sprite.name != targetOn.name)
        {
            targetImage.sprite = targetOn;
            Debug.Log("Change SkipBtnImage On");
        }
        else if (scriptPlayer.SkipActive == false && targetImage.sprite.name == targetOn.name)
        {
            targetImage.sprite = targetOff;
            Debug.Log("Change SkipBtnImage Off");
        }
    }
}
