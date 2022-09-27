using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Naninovel;
using TMPro;

public class BtnEnding : MonoBehaviour
{
    BtnUI btnUI;
    public List<GameObject> EndingBacks;
    public List<GameObject> EndingBtns;
    public List<GameObject> EndingBtnTexts;
    public GameObject EndingTextSecondPanel;
    public GameObject NINIImage;
    
    void Awake()
    {
        btnUI = FindObjectOfType<BtnUI>();
    }

    void OnEnable()
    {
        CheckReadEnding();        
    }

    public void CheckReadEnding()
    {
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
        int readNumber = readDialogueList["END"];
        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.SetVariableValue("g_readEND", readDialogueList["END"].ToString());

        int activeNumber = 0;
        activeNumber = readNumber + 1;

        for (int i = 0; i < EndingBtnTexts.Count; i++)
        {
            if (EndingBtnTexts[i].activeSelf == true)
            {
                EndingBtnTexts[i].SetActive(false);                
            }            
        }
        for (int i = 0; i < EndingBtns.Count; i++)
        {
            if (EndingBtns[i].GetComponent<Button>().interactable == true)
            {
                EndingBtns[i].GetComponent<Button>().interactable = false;
            }
        }
        if (activeNumber > 4)
        {
            for (int i = 4; i < activeNumber; i++)
            {
                if (EndingBtns[i].activeSelf == true)
                {
                    EndingBtns[i].SetActive(false);
                }
            }
        }

        for (int i = 0; i <= activeNumber; i++)
        {
            if (i == 9)
            {
                break;
            }

            if (i < EndingBacks.Count)
            {
                if (EndingBacks[i].activeSelf == false)
                {
                    EndingBacks[i].SetActive(true);
                }
            }

            if (i < EndingBtns.Count && i < 4)
            {
                if (EndingBtns[i].activeSelf == false)
                {
                    EndingBtns[i].SetActive(true);
                }  
            }
            else if (i < EndingBtns.Count && activeNumber >= 4 && activeNumber != 9)
            {
                if (EndingBtns[activeNumber].activeSelf == false)
                {
                    EndingBtns[activeNumber].SetActive(true);
                }                
            }                      
        }

        
        if (activeNumber < 4)
        {
            EndingBtnTexts[activeNumber].SetActive(true);
            EndingBtns[activeNumber].GetComponent<Button>().interactable = true;
        }
        else if (activeNumber >= 4 && activeNumber <= 8)
        {
            EndingBtnTexts[3].SetActive(true);
            EndingBtns[3].GetComponent<Button>().interactable = true;

            if (activeNumber >= 4 && activeNumber < 8)
            {
                EndingTextSecondPanel.GetComponent<TextMeshProUGUI>().text = "말풍선을 누르면 \n이야기를 볼 수 있어요";
            }
            else if (activeNumber == 8)
            {
                EndingTextSecondPanel.GetComponent<TextMeshProUGUI>().text = "나일나일을 누르면 \n대화가 시작돼요";
            }

            EndingBtns[activeNumber].GetComponent<Button>().interactable = true;            
        }

        if (activeNumber > 3 && activeNumber < 8)
        {
            NINIImage.GetComponent<Image>().sprite = btnUI.CharactersForInkPanel[2];
        }
        else if (activeNumber == 8)
        {
            NINIImage.GetComponent<Image>().sprite = btnUI.CharactersForInkPanel[14];
        }

        if (activeNumber == 3 || activeNumber == 8 || activeNumber == 9)
        {
            btnUI.ShowAndHideCharacterImageForInkPanel();
        }        
    }

    public async void ClickEndingBtn()
    {
        string clickedDialogueBtnName = EventSystem.current.currentSelectedGameObject.name;    
        string clickedNum = clickedDialogueBtnName.Substring(clickedDialogueBtnName.Length-1, 1);
        string category = "Ending";
        string dialogueCode = category + clickedNum;

        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.SetVariableValue("g_DialogueCode", dialogueCode);
        variableManager.SetVariableValue("g_Mode", "Normal");       

        var switchCommand = new SwitchToNovelMode { ScriptName = category };
        await switchCommand.ExecuteAsync();
    }
}
