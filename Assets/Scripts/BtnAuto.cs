using UnityEngine;
using UnityEngine.UI;
using Naninovel;

public class BtnAuto : MonoBehaviour
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
        if (scriptPlayer.AutoPlayActive == true && targetImage.sprite.name != targetOn.name)
        {
            targetImage.sprite = targetOn;
            Debug.Log("Change AutoBtnImage On");
        }
        else if (scriptPlayer.AutoPlayActive == false && targetImage.sprite.name == targetOn.name)
        {
            targetImage.sprite = targetOff;
            Debug.Log("Change AutoBtnImage Off");
        }
    }
}
