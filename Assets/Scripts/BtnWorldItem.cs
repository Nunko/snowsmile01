using UnityEngine;
using UnityEngine.EventSystems;

public class BtnWorldItem : MonoBehaviour
{
    BtnGain btnGain;
    float clickedTime;
    public float longTime;
    bool isClick;

    void Awake()
    {
        btnGain = FindObjectOfType<BtnGain>();
    }

    void Update()
    {
        if (isClick == true)
        {
            clickedTime += Time.deltaTime;
        }
        else
        {
            clickedTime = 0;
        }
    }

    public void ButtonDown()
    {
        isClick = true;
    }

    public void ButtonUp()
    {
        isClick = false;

        if(clickedTime > longTime)
        {
            btnGain.CheckGainBtn();
        }
        else
        {
            ClickBtnWorldItem();
        }
    }

    public void ClickBtnWorldItem()
    {
        GameObject CurrentBtn = EventSystem.current.currentSelectedGameObject;
        btnGain.GainOneWorldItem(CurrentBtn);
    }
}
