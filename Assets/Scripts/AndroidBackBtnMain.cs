using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackBtnMain : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject PopUpPanel;
    public GameObject FordedMenuBlockPanel;
    public GameObject ConfigPanel;
    public GameObject ReplayStoryPanel;
    public GameObject CollectionPanel;
    public GameObject ShopPanel;
    public GameObject ShopPopUpPanel; //캔버스그룹
    public GameObject BingoPanel; //선조건
    public GameObject BingoSelectionPanel;
    public GameObject BingoBoardPanel; //캔버스그룹
    public GameObject EndingBoardPanel; //선조건

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape) == true)
            {
                if (UICanvas.GetComponent<CanvasGroup>().alpha == 1)
                {
                    if (PopUpPanel.activeSelf == false)
                    {
                        if (FordedMenuBlockPanel.activeSelf == true)
                        {
                            BtnUI btnUI = FindObjectOfType<BtnUI>();
                            btnUI.ClickFordedMenuBtn();
                        }
                        else if (ConfigPanel.activeSelf == true)
                        {
                            BtnUI btnUI = FindObjectOfType<BtnUI>();
                            btnUI.ClickBackToMainBtn(ConfigPanel);
                            Configuration configuration = FindObjectOfType<Configuration>();
                            configuration.SaveSetting();
                        }
                        else if (ReplayStoryPanel.activeSelf == true)
                        {
                            BtnReplayStory btnReplayStory = FindObjectOfType<BtnReplayStory>();
                            btnReplayStory.ClickReplayStoryExitBtn();
                        }
                        else if (CollectionPanel.activeSelf == true)
                        {
                            BtnCollection btnCollection = FindObjectOfType<BtnCollection>();
                            btnCollection.ClickCollectionExitBtn();
                        }
                        else if (ShopPanel.activeSelf == true)
                        {
                            if (ShopPopUpPanel.GetComponent<CanvasGroup>().alpha == 1)
                            {
                                Shop shop = FindObjectOfType<Shop>();
                                shop.HidePopUpPanel();
                            }
                            else
                            {
                                BtnUI btnUI = FindObjectOfType<BtnUI>();
                                btnUI.ClickBackToMainBtn(ShopPanel);
                            }
                        }
                        else if (BingoPanel.activeSelf == true)
                        {
                            BtnBingo btnBingo = FindObjectOfType<BtnBingo>();
                            if (BingoSelectionPanel.activeSelf == true)
                            {                        
                                btnBingo.ClickBackToMainBtn();
                            }
                            else if (BingoBoardPanel.GetComponent<CanvasGroup>().alpha == 1)
                            {
                                btnBingo.ClickBackToSelectionBtn();
                            }
                            else if (EndingBoardPanel.activeSelf == true)
                            {
                                btnBingo.ClickBackToSelectionBtnInEndingPanel();
                            }   
                        }
                        else
                        {
                            ShowPopUpPanel();
                        }
                    }
                    else
                    {
                        HidePopUpPanel();
                    }
                }
            }            
        }
    }

    void ShowPopUpPanel()
    {
        Time.timeScale = 0;
        PopUpPanel.SetActive(true);
    }

    public void ClickQuitBtn()
    {
        Application.Quit();
    }

    public void HidePopUpPanel()
    {      
        Invoke ("ActiveFalsePopUpPanel", 0.3f);        
    }

    void ActiveFalsePopUpPanel()
    {
        PopUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
