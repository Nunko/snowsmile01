using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Naninovel;

public class BtnBingo : MonoBehaviour
{
    //Sc
    Variables variables;
    Main main;
    WindEffect windEffect;    

    //public GameObject ~Main
    public GameObject BubblePanel;
    public GameObject MainUIPanel;

    //public GameObject ~BingoSelection
    public GameObject BingoPanel;
    public GameObject SelectionPanel;
    public List<GameObject> SelectionCharaterButtons;        

    //public GameObject ~BingoBoard
    public GameObject BingoBoardPanel;
    public GameObject GridPanel;
    public Object GridBtn;
    public List<Sprite> BackImages;
    public List<Sprite> KeyImages;
    public GameObject BingoInformationForNINI;            
    public GameObject StoryPopUpPanel; 
    public GameObject StoryBtnText;    
    public GameObject StoryPopUpPanelWindowBack;
    public List<GameObject> StoryPopUpPanelWindowCharacters;
    public GameObject StoryPlayBtn;    

    public GameObject EndingBoardPanel;
    public GameObject EndingBtn;
    public GameObject EndingFirstPanel;
    public GameObject EndingSecondPanel;
    public GameObject TextPanel;

    //private variable
    bool checkBingoBtn;
    string characterCode;    


    //basic Function
    void Awake()
    {
        variables = FindObjectOfType<Variables>();        
        windEffect = FindObjectOfType<WindEffect>();
        main = FindObjectOfType<Main>();

        checkBingoBtn = false;
        characterCode = "";        
    }

    void Start()
    {
        LoadAllItem();
        CheckGirdInformation();
    }


    //Function related to basic Function
    List<BGEntity> allItem;
    void LoadAllItem()
    {        
        allItem = new List<BGEntity>();
        allItem = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a"));        
    }
    
    void CheckGirdInformation()
    {
        List<string> characterCodeList = new List<string>() {"NINI", "DNEE", "GP", "JNM", "LZB", "ULCS"};
        List<int> ItemsListCountByCharac = new List<int>();
        for (int i = 0; i < characterCodeList.Count; i++)
        {                   
            characterCode = characterCodeList[i];
            string gridInformationByCharac = "gridItemListBy" + characterCodeList[i];
            if (ES3.KeyExists(gridInformationByCharac) == false)
            {
                Debug.Log("그리드 아이템 미생성. 생성! " + gridInformationByCharac);
                SetNewGridRandomly(gridInformationByCharac);
            }
            List<string> ItemsListByCharac = new List<string>(); 
            ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);
            ItemsListCountByCharac.Add(ItemsListByCharac.Count);                
        }

        for (int i = 0; i < ItemsListCountByCharac.Count; i++)        
        {
            for (int j = 0; j < ItemsListCountByCharac.Count; j++)
            {
                int imj  = ItemsListCountByCharac[i] - ItemsListCountByCharac[j];
                if (imj != 0)
                {
                    Debug.Log("그리드 아이템 생성 오류. 재생성!");
                    if (imj > 0)
                    {
                        characterCode = characterCodeList[j];
                        string gridInformationByCharac = "gridItemListBy" + characterCodeList[j];
                        SetNewGridRandomly(gridInformationByCharac);
                        List<string> ItemsListByCharac = new List<string>(); 
                        ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);
                        ItemsListCountByCharac[j] = ItemsListByCharac.Count;
                    }
                    else
                    {
                        characterCode = characterCodeList[i];
                        string gridInformationByCharac = "gridItemListBy" + characterCodeList[i];
                        SetNewGridRandomly(gridInformationByCharac);
                        List<string> ItemsListByCharac = new List<string>(); 
                        ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);
                        ItemsListCountByCharac[i] = ItemsListByCharac.Count;
                    }
                }
            }
        }    
    }

    void SetNewGridRandomly(string gridInformationByCharac)
    {
        List<int> randItemsIndexList = new List<int>();
        if (characterCode != "NINI")
        {
            List<int> keyItemsIndex = FindKeyItemsIndex();
            randItemsIndexList.AddRange(keyItemsIndex);
        }

        int numberP;
        int numberA;
        int numberM;
        int countP = 0;
        int countA = 0;
        int countM = 0;
        switch (characterCode)
        {
            case "DNEE": numberP = 8; numberA = 10; numberM = 2; break;
            case "GP": numberP = 6; numberA = 12; numberM = 2; break;
            case "JNM": numberP = 12; numberA = 6; numberM = 2; break;
            case "LZB": numberP = 16; numberA = 0; numberM = 4; break;
            case "ULCS": numberP = 10; numberA = 4; numberM = 6; break;
            default: numberP = 10; numberA = 10; numberM = 5; break;
        }

        while (randItemsIndexList.Count < 25)
        {
            int randNumber = Random.Range(0, allItem.Count);
            string randItemName = allItem[randNumber].Name;

            if (randItemsIndexList.Contains(randNumber) == false)
            {
                bool conditionP = randItemName.StartsWith("P") == true && countP != numberP;
                bool conditionA = randItemName.StartsWith("A") == true && countA != numberA;
                bool conditionM = randItemName.StartsWith("M") == true && countM != numberM;

                if (conditionP)
                {
                    countP++;
                    randItemsIndexList.Add(randNumber);
                }
                else if (conditionA)
                {
                    countA++;
                    randItemsIndexList.Add(randNumber);
                }
                else if (conditionM)
                {
                    countM++;
                    randItemsIndexList.Add(randNumber);
                }                    
            }
            else
            {
                continue;
            }
        }
        Debug.Log("아이템 수 P: " + countP + "/ A: " + countA + "/ M: " + countM);

        for (int i = 0; i < randItemsIndexList.Count; i++)
        {
            int randNumber1 = Random.Range(0, randItemsIndexList.Count);
            int randNumber2 = Random.Range(0, randItemsIndexList.Count);

            int tmpNumber = randItemsIndexList[randNumber1];
            randItemsIndexList[randNumber1] = randItemsIndexList[randNumber2];
            randItemsIndexList[randNumber2] = tmpNumber;
        }

        List<string> randItemsNameList = new List<string>();
        for (int i = 0; i< randItemsIndexList.Count; i++)
        {
            randItemsNameList.Add(allItem[randItemsIndexList[i]].Name);
        }

        ES3.Save<List<string>>(gridInformationByCharac, randItemsNameList);
        Debug.Log("빙고 목록 생성: " + gridInformationByCharac);
    }

    List<int> FindKeyItemsIndex()
    {
        List<string> keyItemsList = FindKeyItems();
        List<int> keyItemsIndex = new List<int>() {0, 0, 0, 0, 0};  
        for (int i = 0; i < allItem.Count; i++)
        {
            if (keyItemsList.Contains(allItem[i].Name) == true)
            {
                for (int j = 0; j < keyItemsList.Count; j++)
                {
                    if (keyItemsList[j] == allItem[i].Name)
                    {
                        keyItemsIndex[j] = i;
                    }
                }
            }
        }
        return keyItemsIndex;
    }

    List<string> FindKeyItems()
    {
        List<string> keyItemsList;

        switch (characterCode)
        {
            case "DNEE": 
            {
                keyItemsList = new List<string>() {"Animalia024", "Plantae055", "Plantae067", "Animalia017", "Animalia031"};                
                break;
            }
            case "GP": 
            {
                keyItemsList = new List<string>() {"Animalia031", "Animalia018", "Animalia025", "Animalia014", "Plantae068"}; 
                break;
            }
            case "JNM": 
            {
                keyItemsList = new List<string>() {"Plantae021", "Animalia013", "Animalia027", "Animalia020", "Animalia019"}; 
                break;
            }
            case "LZB": 
            {
                keyItemsList = new List<string>() {"Plantae026", "Plantae057", "Plantae066", "Plantae016", "Plantae013"}; 
                break;
            }
            case "ULCS": 
            {
                keyItemsList = new List<string>() {"Plantae041", "Animalia003", "Plantae060", "Minerals004", "Animalia031"}; 
                break;
            }
            default :
            {
                keyItemsList = new List<string>(); 
                break;
            }
        }        

        return keyItemsList;
    }


    //public Function ~Main
    public void CheckBingoBtn()
    {
        if (checkBingoBtn == false && BingoPanel.activeSelf == false)
        {
            checkBingoBtn = true;
            windEffect.StopWindEffect();
            ClickBingoBtn();            
            checkBingoBtn = false;
        }
        else
        {
            Debug.Log("이야기 보러가기 버튼 실행 중");
        }
    }

    void ClickBingoBtn()
    {
        BubblePanel.GetComponent<CanvasGroup>().interactable = false;
        BubblePanel.GetComponent<CanvasGroup>().alpha = 0;
        BubblePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        MainUIPanel.GetComponent<CanvasGroup>().interactable = false;
        MainUIPanel.GetComponent<CanvasGroup>().alpha = 0;
        MainUIPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        BingoPanel.SetActive(true);  

        RunTutorial(1);

        windEffect.RunWindEffect();

        CheckSelectionCharacterIcon();
        CheckEndingConditions();
    }

    async void RunTutorial(int tutorialNumber)
    {
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
        if (readDialogueList["Tutorial"] < tutorialNumber)
        {
            ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
            variableManager.SetVariableValue("g_Mode", "Tutorial");
            variableManager.SetVariableValue("g_DialogueCode", "Tutorial" + tutorialNumber);
            var switchCommand = new SwitchToNovelMode { ScriptName = "Tutorial" };
            await switchCommand.ExecuteAsync();
            GameObject Printer = GameObject.Find("NewTMProDialoguePrinter");
            Printer.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;  
        }
    }

    void CheckSelectionCharacterIcon()
    {
        List<string> characterCodeList = new List<string>() {"NINI", "DNEE", "GP", "JNM", "LZB", "ULCS"};
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        for (int i = 0; i < characterCodeList.Count; i++)
        {
            if (readDialogueList[characterCodeList[i]] == 7)
            {
                SelectionCharaterButtons[i].transform.Find("IconEmboss").GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                SelectionCharaterButtons[i].transform.Find("Icon").GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            }
        }
    } 

    
    //public Function ~BingoSelection
    public void ClickBackToMainBtn()
    {        
        windEffect.StopWindEffect();

        BingoPanel.SetActive(false);
        MainUIPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        MainUIPanel.GetComponent<CanvasGroup>().alpha = 1;
        MainUIPanel.GetComponent<CanvasGroup>().interactable = true;

        BubblePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        BubblePanel.GetComponent<CanvasGroup>().alpha = 1;
        BubblePanel.GetComponent<CanvasGroup>().interactable = true;

        windEffect.RunWindEffect();
    }

    public void ClickCharacBtn(string inputCharacterCode)
    {
        windEffect.StopWindEffect();

        GenGridBtn();
        characterCode = inputCharacterCode;
        ToAlphaZeroGridIcons();
        RefreshGridButton();
        SwitchOffAllStoryBtn();
        LoadGridImage();
        CheckBag();  
        SetRule();
        CheckItemName();        
        ApplyRule();
        SwitchOnStoryBtn();
        CheckBingoInformationForNINI();
        ActivateGridButton();

        SelectionPanel.SetActive(false);

        BingoBoardPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        BingoBoardPanel.GetComponent<CanvasGroup>().alpha = 1;
        BingoBoardPanel.GetComponent<CanvasGroup>().interactable = true;

        RunTutorial(2);
    }

    void GenGridBtn()
    {
        if (GridPanel.transform.childCount <25)
        {
            for (int i = GridPanel.transform.childCount; i < 25; i++)
            {
                GameObject NewGrid = Instantiate(GridBtn, GridPanel.transform) as GameObject;
                NewGrid.name = "GridBtn" + i;
            }
        }
    }

    void ToAlphaZeroGridIcons()
    {
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            grid.transform.Find("BackImage").GetComponent<Image>().color = new Color(1, 1, 1, 0);   
            grid.transform.Find("KeyIcon").GetComponent<Image>().color = new Color(1, 1, 1, 0);          
            grid.transform.Find("KeyIcon").GetComponentInChildren<TextMeshProUGUI>().color  = new Color(1, 1, 1, 0);   
        }
    }

    void RefreshGridButton()
    {
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            grid.GetComponent<Button>().interactable = false;
        }
    }

    void SwitchOffAllStoryBtn()
    {
        for (int i = 1; i < 7; i++)
        {
            GameObject StoryBtn = StoryPopUpPanel.transform.GetChild(i).gameObject;
            StoryBtn.GetComponent<Button>().interactable = false;
            StoryBtn.GetComponent<CanvasGroup>().interactable = false;            
            StoryBtn.GetComponent<CanvasGroup>().alpha = 0;
            StoryBtn.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    void LoadGridImage()
    {
        string gridInformationByCharac = "gridItemListBy" + characterCode;        
        if (ES3.KeyExists(gridInformationByCharac) == false)
        {
            SetNewGridRandomly(gridInformationByCharac);
        } 

        List<string> allItemNameList = new List<string>();
        allItemNameList = LoadAllItemName();

        List<string> ItemsListByCharac = new List<string>();
        ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);
        List<int> ItemsIndexList = new List<int>();        
        for (int i = 0; i <ItemsListByCharac.Count; i++)
        {            
            ItemsIndexList.Add(allItemNameList.IndexOf(ItemsListByCharac[i]));
        }

        for (int i = 0; i < ItemsIndexList.Count; i++)
        {
            int index = ItemsIndexList[i];
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            string itemName = allItem[index].Get<string>("kr");
            grid.transform.Find("BalloonImage").Find("ItemNameInBalloon").GetComponent<TextMeshProUGUI>().text = itemName;
            Image itemImage = grid.transform.Find("ItemImage").GetComponent<Image>();                 
            itemImage.sprite = main.ItemList[allItemNameList[index]];
            itemImage.color = new Color(1, 1, 1, 0);
            Image paperImage = grid.transform.Find("PaperImage").GetComponent<Image>();
            paperImage.color = new Color(1, 1, 1, 0);
            grid.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);              
        }
    }

    List<string> LoadAllItemName()
    {
        List<string> allItemNameList = new List<string>();
        for(int i = 0; i<allItem.Count; i++)
        {
            allItemNameList.Add(allItem[i].Name);
        }
        return allItemNameList;
    }

    List<int> paintedGridList;
    void CheckBag()
    {
        Dictionary<string, int> bag = new Dictionary<string, int>();
        bag = variables.LoadBag();

        string gridInformationByCharac = "gridItemListBy" + characterCode;  
        List<string> ItemsListByCharac = new List<string>();
        ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);
        
        paintedGridList = new List<int>();
        for(int i = 0; i < ItemsListByCharac.Count; i++)
        {
            if (bag.ContainsKey(ItemsListByCharac[i]) == true)
            {
                if (bag[ItemsListByCharac[i]] > 0)
                {
                    paintedGridList.Add(i);
                }                
            }           
        }
        Debug.Log("색칠 칸 수: " + paintedGridList.Count);                        
    }

    List<List<int>> JudgmentAll;
    void SetRule()
    {
        List<int> row1 = new List<int>() {0,1,2,3,4};
        List<int> row2 = new List<int>() {5,6,7,8,9};
        List<int> row3 = new List<int>() {10,11,12,13,14};
        List<int> row4 = new List<int>() {15,16,17,18,19};
        List<int> row5 = new List<int>() {20,21,22,23,24};
        List<int> column1 = new List<int>() {0,5,10,15,20};
        List<int> column2 = new List<int>() {1,6,11,16,21};
        List<int> column3 = new List<int>() {2,7,12,17,22};
        List<int> column4 = new List<int>() {3,8,13,18,23};
        List<int> column5 = new List<int>() {4,9,14,19,24};
        List<int> cross1 = new List<int>() {0,6,12,18,24};
        List<int> cross2 = new List<int>() {4,8,12,16,20};

        JudgmentAll = new List<List<int>>();
        JudgmentAll.Add(row1);
        JudgmentAll.Add(row2);
        JudgmentAll.Add(row3);
        JudgmentAll.Add(row4);
        JudgmentAll.Add(row5);
        JudgmentAll.Add(column1);
        JudgmentAll.Add(column2);
        JudgmentAll.Add(column3);
        JudgmentAll.Add(column4);
        JudgmentAll.Add(column5);
        JudgmentAll.Add(cross1);
        JudgmentAll.Add(cross2);
    }

    void CheckItemName()
    {
        RefreshKeyIcon();

        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            TextMeshProUGUI textMeshProUGUI = GridPanel.transform.GetChild(i).Find("ItemNameInPaper").GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = "?";
            textMeshProUGUI.color = new Color(0, 0, 0, 1);
        }

        List<int> keyItemList = new List<int>();
        keyItemList = FindkeyItemsGridNumber();
        List<int> gridShowsNameList = new List<int>();

        if (characterCode != "NINI")
        {
            List<int> currentKeyItemList = new List<int>();
            List<int> notCurrentKeyItemList = new List<int>();
            for (int i = 0; i < keyItemList.Count; i++)
            {
                if (paintedGridList.Contains(keyItemList[i]) == true)
                {
                    currentKeyItemList.Add(keyItemList[i]);
                }
                else
                {
                    notCurrentKeyItemList.Add(keyItemList[i]);
                }
            }

            if (currentKeyItemList.Count > 0)
            {
                for (int i = 0; i < currentKeyItemList.Count; i++)
                {

                    ShowKeyIcon(currentKeyItemList[i], keyItemList.IndexOf(currentKeyItemList[i]));
                    for (int j = 0; j < JudgmentAll.Count; j++)
                    {
                        if (JudgmentAll[j].Contains(currentKeyItemList[i]) == true)
                        {
                            for (int k = 0; k < JudgmentAll[j].Count; k++)
                            {
                                if (gridShowsNameList.Contains(JudgmentAll[j][k]) == false)
                                {
                                    gridShowsNameList.Add(JudgmentAll[j][k]);                                    
                                }
                            }
                        }
                    }
                }          
            }

            if (notCurrentKeyItemList.Count > 0)
            {
                for (int i = 0; i < notCurrentKeyItemList.Count; i++)
                {
                    gridShowsNameList.Add(notCurrentKeyItemList[i]);
                    ShowKeyIcon(notCurrentKeyItemList[i], keyItemList.IndexOf(notCurrentKeyItemList[i]));                  
                }
            }            
        }
        else
        {
            for (int i = 0; i < paintedGridList.Count; i++)
            {
                for (int j = 0; j < JudgmentAll.Count; j++)
                {
                    if (JudgmentAll[j].Contains(paintedGridList[i]) == true)
                    {
                        for (int k = 0; k < JudgmentAll[j].Count; k++)
                        {
                            if (gridShowsNameList.Contains(JudgmentAll[j][k]) == false)
                            {
                                gridShowsNameList.Add(JudgmentAll[j][k]);
                            }
                        }
                    }
                }
            }                              
        }        

        if (gridShowsNameList.Count > 0)
        {
            int gridShowsNameCount = 0;
            for (int i = 0; i < gridShowsNameList.Count; i++)
            {
                GameObject grid = GridPanel.transform.GetChild(gridShowsNameList[i]).gameObject;
                string itemName = grid.transform.Find("BalloonImage").Find("ItemNameInBalloon").GetComponent<TextMeshProUGUI>().text;
                TextMeshProUGUI itemNameInPaper = grid.transform.Find("ItemNameInPaper").GetComponent<TextMeshProUGUI>();
                if (itemNameInPaper.text == "?")
                {
                    itemNameInPaper.text = itemName;
                    gridShowsNameCount++;
                }                
                if (gridShowsNameCount == 25)
                {
                    Debug.Log("그리드 이름 보여주기 끝");
                    break;
                }                    
            } 
        }                                             
    }

    void RefreshKeyIcon()
    {
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            GameObject keyIcon = grid.transform.Find("KeyIcon").gameObject; 

            keyIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0);  
            keyIcon.GetComponentInChildren<TextMeshProUGUI>().color  = new Color(1, 1, 1, 0);   
            keyIcon.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0); 
            keyIcon.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0); 
        }                 
    }

    List<int> FindkeyItemsGridNumber()
    {

        List<int> keyItemsGridList = new List<int>() {-1, -1, -1, -1, -1};

        if (characterCode != "NINI")
        {            
            List<string> keyItems = new List<string>();
            keyItems = FindKeyItems();
        
            string gridInformationByCharac = "gridItemListBy" + characterCode;
            List<string> ItemsList = new List<string>();        
            ItemsList = ES3.Load<List<string>>(gridInformationByCharac);
            
            for (int i = 0; i < ItemsList.Count; i++)
            {
                for (int j = 0; j < keyItems.Count; j++)
                {
                    if (ItemsList[i] == keyItems[j])
                    {
                        keyItemsGridList[j] = i;
                    }
                }
            }
        }

        Debug.Log("키아이템 그리드: " + keyItemsGridList[0] + ", " + keyItemsGridList[1] + ", " + keyItemsGridList[2] + ", " + keyItemsGridList[3] + ", " + keyItemsGridList[4]);

        return keyItemsGridList;        
    }

    void ShowKeyIcon(int keyGridNumber, int keyNumber)
    {
        GameObject grid = GridPanel.transform.GetChild(keyGridNumber).gameObject;
        GameObject keyIcon = grid.transform.Find("KeyIcon").gameObject;  

        int readDialogueList = LoadReadDialogueList();

        if (paintedGridList.Contains(keyGridNumber) == true)
        {            
            if (keyNumber == (readDialogueList - 1))
            {
                keyIcon.GetComponent<Image>().sprite = KeyImages[2];
            }
            else
            {
                keyIcon.GetComponent<Image>().sprite = KeyImages[1];                               
            } 
            keyIcon.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 1); 
            keyIcon.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(-12.5f, -60.3f, 0);        
        } 
        else
        {
            keyIcon.GetComponent<Image>().sprite = KeyImages[0]; 
            keyIcon.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1); 
            keyIcon.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(-25, -5, 0);
        } 

        if (keyNumber == (readDialogueList - 1)) 
        {
            keyIcon.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);  
        }   
        else
        {
            keyIcon.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 1);  
        }                          
        keyIcon.GetComponentInChildren<TextMeshProUGUI>().text = (keyNumber + 2).ToString();     
        keyIcon.GetComponentInChildren<TextMeshProUGUI>().color  = new Color(1, 1, 1, 1);    

        if (keyNumber < (readDialogueList - 1)) 
        {
            keyIcon.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 0); 
            keyIcon.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0); 
            keyIcon.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0); 
            keyIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0); 
        }            
    }

    int LoadReadDialogueList()
    {
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();

        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.SetVariableValue("g_readDNEE", readDialogueList["DNEE"].ToString());
        variableManager.SetVariableValue("g_readGP", readDialogueList["GP"].ToString());
        variableManager.SetVariableValue("g_readJNM", readDialogueList["JNM"].ToString());
        variableManager.SetVariableValue("g_readLZB", readDialogueList["LZB"].ToString());
        variableManager.SetVariableValue("g_readULCS", readDialogueList["ULCS"].ToString());
        variableManager.SetVariableValue("g_readEND", readDialogueList["END"].ToString());
        Debug.Log("g_read값 나니노벨 변수에 넣음");

        return readDialogueList[characterCode];
    }   

    List<int> finishedKeyList;
    void ApplyRule()
    {
        finishedKeyList = new List<int>() {-1, -1, -1, -1, -1};

        if (paintedGridList.Count > 0)
        {
            List<List<int>> CurrentBingo = new List<List<int>>();
            for (int i = 0; i < JudgmentAll.Count; i++)
            {
                for (int j = 0; j < JudgmentAll[i].Count ; j++)
                {
                    if (paintedGridList.Contains(JudgmentAll[i][j]) == false)
                    {
                        break;
                    }
                    if (j == JudgmentAll[i].Count - 1)
                    {
                        if (CurrentBingo.Contains(JudgmentAll[i]) == false)
                        {
                            CurrentBingo.Add(JudgmentAll[i]);
                        }
                    }
                }
            }
            Debug.Log("현재 빙고 수: " + CurrentBingo.Count);

            if (paintedGridList.Count > 0)
            {
                List<int> keyItemList = new List<int>();
                keyItemList = FindkeyItemsGridNumber();
                if (characterCode == "NINI")
                {
                    finishedKeyList[0] = CurrentBingo.Count;    
                    Debug.Log("보유 키 색 변화: " + 0 + "-" + finishedKeyList[0]);
                }

                for(int i = 0; i < paintedGridList.Count; i++)
                {
                    GameObject grid = GridPanel.transform.GetChild(paintedGridList[i]).gameObject;
                    grid.transform.Find("ItemNameInPaper").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
                    grid.transform.Find("PaperImage").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    grid.transform.Find("ItemImage").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    
                    if (CurrentBingo.Count > 0 && characterCode != "NINI")
                    {
                        for (int j = 0; j < CurrentBingo.Count; j++)
                        {
                            if (CurrentBingo[j].Contains(paintedGridList[i]))
                            {
                                for (int k = 0; k < keyItemList.Count; k++)
                                {
                                    if (CurrentBingo[j].Contains(keyItemList[k]) == true)
                                    {
                                        if (finishedKeyList[k] == -1)
                                        {
                                            finishedKeyList[k] = keyItemList[k];
                                            Debug.Log("보유 키 색 변화: " + k + "-" + finishedKeyList[k]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }        
    }

    void SwitchOnStoryBtn()
    {
        int readDialogueList = LoadReadDialogueList();
        int tmpNumber = 1;
        string firstCharacName = "";
        string storyNumberText = "";
        Debug.Log("과거 읽은 이야기 번호: " + readDialogueList);
        TextMeshProUGUI StoryPlayBtnText = StoryPlayBtn.GetComponentInChildren<TextMeshProUGUI>();
        StoryPlayBtnText.color = new Color(1, 1, 1, 0);

        if (readDialogueList != -1 && characterCode != "NINI" && readDialogueList < 7)
        {                        
            for (int i = 1; i < 7; i++)
            {            
                if (readDialogueList >= i)
                {
                    GameObject StoryBtn = StoryPopUpPanel.transform.GetChild(i).gameObject;
                    StoryBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    StoryBtn.GetComponent<CanvasGroup>().alpha = 1;
                    StoryBtn.GetComponent<CanvasGroup>().interactable = true;
                    tmpNumber++;
                    StoryPlayBtnText.text = "기억 더 필요";
                    StoryPlayBtn.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 1); 

                    if (i != 6 && finishedKeyList[i - 1] != -1)
                    {
                        StoryPlayBtnText.text = "보러가기";
                        StoryPlayBtn.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);
                        StoryBtn.GetComponent<Button>().interactable = true;
                    }
                    else if (i == 6)
                    {
                        StoryPlayBtnText.text = "보러가기";
                        StoryPlayBtn.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);
                        StoryBtn.GetComponent<Button>().interactable = true;
                    }                                       

                    Debug.Log("이야기 버튼 활성화: " + StoryBtn.name);
                }                
                else
                {                    
                    break;
                }                  
            }            
        }
        else if (characterCode == "NINI" && readDialogueList != -1 && readDialogueList < 7)
        {
            for (int i = 1; i < 7; i++)
            {            
                if (readDialogueList >= i)
                {
                    GameObject StoryBtn = StoryPopUpPanel.transform.GetChild(i).gameObject;
                    StoryBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    StoryBtn.GetComponent<CanvasGroup>().alpha = 1;
                    StoryBtn.GetComponent<CanvasGroup>().interactable = true;                    
                    tmpNumber++;
                    StoryPlayBtnText.text = "기억 더 필요";
                    StoryPlayBtn.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 1); 

                    if (finishedKeyList[0] >= i)
                    {                        
                        StoryPlayBtnText.text = "보러가기";
                        StoryPlayBtn.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);
                        StoryBtn.GetComponent<Button>().interactable = true; 
                    }                     
                                      
                    Debug.Log("이야기 버튼 활성화: " + StoryBtn.name);
                }
                else
                {                    
                    break;
                }                
            }            
        }
        else if (readDialogueList == 7)
        {
            GameObject StoryBtn = StoryPopUpPanel.transform.GetChild(6).gameObject;
            StoryBtn.GetComponent<CanvasGroup>().blocksRaycasts = true;
            StoryBtn.GetComponent<CanvasGroup>().alpha = 1;
            StoryBtn.GetComponent<CanvasGroup>().interactable = true;  
            tmpNumber = 7;
            StoryPlayBtnText.text = "모두 읽음";
            StoryPlayBtn.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 1); 
        }
        else if (readDialogueList == -1)
        {
            StoryPlayBtnText.text = "보러가기";
            StoryPlayBtn.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);
        }

        StoryPlayBtnText.color = new Color(1, 1, 1, 1);

        RefreshBackImage();
        if (readDialogueList != -1 && readDialogueList < 6)
        {
            ShowBackImages(readDialogueList - 1);
        }        

        switch (characterCode)
        {
            case "DNEE": firstCharacName = "대니엘렐과";
                        break;
            case "GP": firstCharacName = "가플과";
                        break;
            case "JNM": firstCharacName = "주나무와";
                        break;
            case "LZB": firstCharacName = "리즈비와";
                        break;
            case "ULCS": firstCharacName = "율리시스와";
                        break;
            default: firstCharacName = "";
                        break;
        }

        Debug.Log("현재 숫자: " + tmpNumber);
        SetStoryPopUpPanelWindowBack(tmpNumber);
        RefreshStoryPopUpPanelWindowCharacter();
        SetStoryPopUpPanelWindowCharacters(tmpNumber);

        switch(tmpNumber)
        {
            case 2: storyNumberText = "두 번째";
                    break; 
            case 3: storyNumberText = "세 번째";
                    break;
            case 4: storyNumberText = "네 번째";
                    break;
            case 5: storyNumberText = "다섯 번째";
                    break;
            case 6: storyNumberText = "여섯 번째";
                    break;
            case 7: storyNumberText = "마지막";
                    break;
            default: storyNumberText = "첫 번째";
                    break;
        }

        string btnSentence = firstCharacName + " 나일나일의 " + storyNumberText + " 이야기";
        StoryBtnText.GetComponent<TextMeshProUGUI>().text = btnSentence;
    }

    void RefreshBackImage()
    {
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            GameObject BackImage = grid.transform.Find("BackImage").gameObject;
            BackImage.GetComponent<Image>().color = new Color(87/255f, 115/255f, 143/255f, 0);
        }
    }

    void ShowBackImages(int keyNumber)
    {
        List<int> keyGrids = FindkeyItemsGridNumber();
        int keyGrid= keyGrids[keyNumber];
        List<int> judgmentNumbers = new List<int>();
        List<int> gridsTmp = new List<int>();

        for (int i = 0; i < JudgmentAll.Count; i++)
        {
            for (int j = 0; j < JudgmentAll[i].Count; j++)
            {
                if (JudgmentAll[i].Contains(keyGrid) == true)
                {
                    if (JudgmentAll[i][j] != keyGrid && gridsTmp.Contains(JudgmentAll[i][j]) == false)
                    {
                        gridsTmp.Add(JudgmentAll[i][j]);
                        judgmentNumbers.Add(i);
                    }
                }
            }
        }

        for (int i = 0; i < gridsTmp.Count; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(gridsTmp[i]).gameObject;
            GameObject BackImage = grid.transform.Find("BackImage").gameObject;

            if (paintedGridList.Contains(gridsTmp[i]) == true)
            {
                if (judgmentNumbers[i] < 5)
                {
                    BackImage.GetComponent<Image>().sprite = BackImages[1];
                }
                else if (judgmentNumbers[i] >=5 && judgmentNumbers[i] < 10)
                {
                    BackImage.GetComponent<Image>().sprite = BackImages[2];
                }
                else if (judgmentNumbers[i] == 10)
                {
                    BackImage.GetComponent<Image>().sprite = BackImages[3];
                }
                else if (judgmentNumbers[i] == 11)
                {
                    BackImage.GetComponent<Image>().sprite = BackImages[4];
                }
            }
            else
            {
                BackImage.GetComponent<Image>().sprite = BackImages[0];
            }

            BackImage.GetComponent<Image>().color = new Color(12/255f, 33/255f, 72/255f, 1);
        }        
    }

    void SetStoryPopUpPanelWindowBack(int index)
    {
        List<int> backList = new List<int>();
        switch (characterCode)
        {
            case "NINI" : 
            List<int> backListNINI = new List<int>() {4, 12, 6, 21, 17, 10, 3};
            backList = backListNINI;
            break;

            case "DNEE" : 
            List<int> backListDNEE = new List<int>() {4, 7, 8, 4, 8, 10, 4};
            backList = backListDNEE;
            break;

            case "GP" : 
            List<int> backListGP = new List<int>() {7, 8, 7, 12, 8, 9, 12};
            backList = backListGP;
            break;

            case "JNM" : 
            List<int> backListJNM = new List<int>() {6, 19, 9, 12, 17, 19, 13};
            backList = backListJNM;
            break;

            case "LZB" : 
            List<int> backListLZB = new List<int>() {21, 18, 21, 20, 21, 21, 21};
            backList = backListLZB;
            break;

            case "ULCS" : 
            List<int> backListULCS = new List<int>() {12, 19, 7, 4, 9, 12, 12};
            backList = backListULCS;
            break;

            default : 
            break;
        }

        StoryPopUpPanelWindowBack.GetComponent<Image>().sprite = main.BgListM[(backList[(index - 1)] - 1)]; 
    }

    void RefreshStoryPopUpPanelWindowCharacter()
    {
        Debug.Log("미리보기 창에 캐릭터: 모두 알파값 0");
        for (int i = 0; i < StoryPopUpPanelWindowCharacters.Count; i++)
        {
            StoryPopUpPanelWindowCharacters[i].GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void SetStoryPopUpPanelWindowCharacters(int index)
    {
        int readDialogueList = LoadReadDialogueList();

        if (readDialogueList < index)
        {
            Debug.Log("미리보기 창에 캐릭터 등장: 조건 만족!");
            List<string> character0 = new List<string>();
            List<string> character1 = new List<string>();
            List<string> character2 = new List<string>();
            List<string> character3 = new List<string>();
            
            if (characterCode == "NINI")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SleepyC", "false", "30.1"};
                    character0 = _character0;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankO", "true", "88.4"};
                    List<string> _character1 = new List<string>() {"DNEE", "AngryN", "false", "67"};
                    List<string> _character2 = new List<string>() {"GP", "BlushC", "false", "21.3"};
                    character0 = _character1;
                    character1 = _character0;
                    character2 = _character2;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedG", "false", "18.3"};
                    List<string> _character1 = new List<string>() {"DNEE", "SmileN", "true", "39.6"};
                    List<string> _character2 = new List<string>() {"JNM", "Default", "false", "78.8"};
                    character0 = _character1;
                    character1 = _character2;
                    character2 = _character0;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "AngryN", "false", "25"};
                    List<string> _character1 = new List<string>() {"DNEE", "Default", "false", "71.5"};
                    List<string> _character2 = new List<string>() {"LZB", "SurprisedB", "true", "48.2"};
                    character0 = _character0;
                    character1 = _character1;
                    character2 = _character2;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "true", "80"};
                    List<string> _character1 = new List<string>() {"ULCS", "WatchN", "true", "26.9"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedN", "false", "34.9"};
                    List<string> _character1 = new List<string>() {"DNEE", "SurprisedN", "false", "63.3"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"DNEE", "AngryDO", "true", "42.8"};
                    List<string> _character1 = new List<string>() {"GP", "AngryDO", "false", "71.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
            }
            else if (characterCode == "DNEE")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileOH", "false", "30.1"};
                    List<string> _character1 = new List<string>() {"DNEE", "SmileDH", "false", "62.7"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"DNEE", "SmileN", "false", "71.5"};
                    List<string> _character1 = new List<string>() {"NINI", "SurprisedG", "false", "21.6"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "true", "78.7"};
                    character0 = _character0;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "AngryB", "false", "30.1"};
                    List<string> _character1 = new List<string>() {"DNEE", "PityN", "false", "62.7"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "EatN", "false", "21.6"};
                    List<string> _character1 = new List<string>() {"DNEE", "EatN", "false", "71.5"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedN", "false", "27"};
                    List<string> _character1 = new List<string>() {"DNEE", "SmileDH", "false", "63.3"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankC", "false", "21.6"};
                    character0 = _character0;
                }
            }
            else if (characterCode == "GP")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankO", "false", "41.7"};
                    List<string> _character1 = new List<string>() {"GP", "Default", "false", "84.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileO", "false", "21.3"};
                    List<string> _character1 = new List<string>() {"GPF1", "Default", "false", "84.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "AngryB", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"GP", "SadOT", "false", "71.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileC", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"GP", "FocusN", "false", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedG", "false", "50"};
                    character0 = _character0;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankC", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"GP", "FocusN", "false", "28.7"};
                    List<string> _character2 = new List<string>() {"GPF1", "SmileCO", "false", "12.5"};
                    character0 = _character0;
                    character1 = _character2;
                    character2 = _character1;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileOH", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"GP", "SadCT", "false", "71.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
            }
            else if (characterCode == "JNM")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedG", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"JNM", "BlankO", "false", "73.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"JNM", "BlankS", "false", "73.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankO", "false", "21.3"};
                    List<string> _character1 = new List<string>() {"JNM", "SmileN", "false", "73.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileCH", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"JNM", "BlankO", "true", "26.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankC", "false", "21.3"};
                    List<string> _character1 = new List<string>() {"JNM", "Default", "false", "73.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedN", "false", "35"};
                    List<string> _character1 = new List<string>() {"JNM", "SmileN", "true", "13.7"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"JNM", "BlankC", "false", "26.7"};
                    character0 = _character0;
                }
            }
            else if (characterCode == "LZB")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileC", "true", "50"};
                    List<string> _character1 = new List<string>() {"LZB", "AngryDO", "false", "71.3"};
                    character0 = _character1;
                    character1 = _character0;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "true", "83.3"};
                    List<string> _character1 = new List<string>() {"LZB", "AngryC", "false", "56.5"};
                    List<string> _character2 = new List<string>() {"LZBF1", "SmileN", "false", "19.4"};
                    List<string> _character3 = new List<string>() {"LZBF2", "SmileN", "true", "38"};
                    character0 = _character2;
                    character1 = _character3;
                    character2 = _character0;
                    character3 = _character1;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "AngryN", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"LZB", "Default", "false", "71.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileCH", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"LZB", "SurprisedB", "true", "31.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedN", "true", "83.3"};
                    character0 = _character0;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankC", "false", "28.7"};
                    List<string> _character1 = new List<string>() {"LZB", "SmileC", "false", "71.3"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileCH", "false", "40.4"};
                    List<string> _character1 = new List<string>() {"LZB", "SurprisedGO", "false", "78.7"};
                    List<string> _character2 = new List<string>() {"LZBF1", "SurprisedG", "true", "21.3"};
                    List<string> _character3 = new List<string>() {"LZBF2", "SurprisedG", "true", "59.6"};
                    character0 = _character2;
                    character1 = _character3;
                    character2 = _character1;
                    character3 = _character0;
                }
            }
            else if (characterCode == "ULCS")
            {
                if (index == 1)
                {
                    List<string> _character0 = new List<string>() {"NINI", "BlankO", "true", "78.7"};
                    List<string> _character1 = new List<string>() {"ULCS", "ThinkN", "true", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 2)
                {
                    List<string> _character0 = new List<string>() {"ULCS", "AngryN", "false", "71.3"};
                    character0 = _character0;
                }
                else if (index == 3)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"ULCS", "SmileA", "false", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 4)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SurprisedN", "false", "50"};
                    character0 = _character0;
                }
                else if (index == 5)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileO", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"ULCS", "SurprisedN", "true", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 6)
                {
                    List<string> _character0 = new List<string>() {"NINI", "SmileC", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"ULCS", "Default", "false", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
                else if (index == 7)
                {
                    List<string> _character0 = new List<string>() {"NINI", "Default", "true", "71.3"};
                    List<string> _character1 = new List<string>() {"ULCS", "SmileA", "false", "28.7"};
                    character0 = _character0;
                    character1 = _character1;
                }
            }            

            Debug.Log("미리보기 창에 캐릭터 등장: 정보 입력");
            InputStoryPopUpPanelWindowCharacter(character0, 0);            
            if (character1.Count > 0)
            {
                InputStoryPopUpPanelWindowCharacter(character1, 1);
            }
            if (character2.Count > 0)
            {
                InputStoryPopUpPanelWindowCharacter(character2, 2);
            }
            if (character3.Count > 0)
            {
                InputStoryPopUpPanelWindowCharacter(character3, 3);
            }
        }      
        else
        {
            Debug.Log("미리보기 창에 캐릭터 등장: 조건 불만족!");
        }          
    }

    void InputStoryPopUpPanelWindowCharacter(List<string> characterInformation, int index)
    {
        string characterNameCode = "";
        if (characterInformation[0] != "GPF1" && characterInformation[0] != "LZBF1" && characterInformation[0] != "LZBF2")
        {
            characterNameCode = characterInformation[0];
        }
        else if (characterInformation[0] == "GPF1")
        {
            characterNameCode = "GP";
        }
        else if (characterInformation[0] == "LZBF1" || characterInformation[0] == "LZBF2")
        {
            characterNameCode = "LZB";
        }
        
        string preText = "";
        if (characterInformation[0] == "GPF1")
        {
            preText = "e_gp";
        }
        else
        {
            preText = characterInformation[0].ToLower();
        }

        string characterFaceCode = characterInformation[1];

        Sprite characterSprite = Resources.Load<Sprite>("Sprites/Charac/" + characterNameCode + "/"+ preText + characterFaceCode);
        StoryPopUpPanelWindowCharacters[index].GetComponent<Image>().sprite = characterSprite;    

        float characterWidth = 0;
        float characterHeight = 0;
        if (characterInformation[0] != "DNEE" && characterInformation[0] != "JNM" && characterInformation[0] != "ULCS")
        {
            characterWidth = 128*2.8f;
            characterHeight = 128*2.8f;
        }
        else if (characterInformation[0] == "DNEE")
        {
            characterWidth = 171*2.8f;
            characterHeight = 128*2.8f;
        }
        else if (characterInformation[0] == "JNM")
        {
            characterWidth = 160*2.8f;
            characterHeight = 220*2.8f;
        }
        else if (characterInformation[0] == "ULCS")
        {
            characterWidth = 128*2.8f;
            characterHeight = 160*2.8f;
        }

        StoryPopUpPanelWindowCharacters[index].GetComponent<RectTransform>().sizeDelta = new Vector2(characterWidth, characterHeight);

        if (characterInformation[2] == "true")
        {
            StoryPopUpPanelWindowCharacters[index].GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            StoryPopUpPanelWindowCharacters[index].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        float multiNumber = float.Parse(characterInformation[3])*0.01f;
        float plusNumber = 622*multiNumber - 622*0.5f;
        StoryPopUpPanelWindowCharacters[index].GetComponent<RectTransform>().localPosition = new Vector3(plusNumber, -350, 0);

        StoryPopUpPanelWindowCharacters[index].GetComponent<CanvasGroup>().alpha = 1;
    }

    void CheckBingoInformationForNINI()
    {
        BingoInformationForNINI.GetComponent<CanvasGroup>().alpha = 0;
        
        if (characterCode == "NINI")
        {
            TextMeshProUGUI bingoInformationTextForNINI = BingoInformationForNINI.GetComponentInChildren<TextMeshProUGUI>();
            bool needForChange = false;

            string numberText = "";        
            switch (finishedKeyList[0])
            {
                case 1 : 
                numberText = "한 ";
                needForChange = true;
                break;

                case 2 : 
                numberText = "두 ";
                needForChange = true;
                break;

                case 3 : 
                numberText = "세 ";
                needForChange = true;
                break;

                case 4 : 
                numberText = "네 ";
                needForChange = true;
                break;

                case 5 : 
                numberText = "다섯 ";
                needForChange = true;
                break;
                
                default : 
                break;
            }
            
            if (needForChange == true)
            {
                bingoInformationTextForNINI.text = numberText + " 줄 완성!";
                BingoInformationForNINI.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                bingoInformationTextForNINI.text = "";
            }            
        }
    }

    void ActivateGridButton()
    {
        for (int i = 0; i < paintedGridList.Count; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(paintedGridList[i]).gameObject;
            grid.GetComponent<Button>().interactable = true;
        }
    }


    //public Function ~BingoBoard
    public void ClickBackToSelectionBtn()
    {
        BingoBoardPanel.GetComponent<CanvasGroup>().interactable = false;
        BingoBoardPanel.GetComponent<CanvasGroup>().alpha = 0;
        BingoBoardPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        RefreshBalloonImage();

        SelectionPanel.SetActive(true);

        windEffect.RunWindEffect();
        
        CheckSelectionCharacterIcon();
        CheckEndingConditions();
    }    

    public async void ClickStoryBtn()
    {
        string clickedDialogueBtnName = EventSystem.current.currentSelectedGameObject.name;    
        string clickedNum = clickedDialogueBtnName.Substring(clickedDialogueBtnName.Length-1, 1);
        string category = "";
        if (characterCode == "NINI")
        {
            category = "FirstRound";
        }
        else
        {
            category = "SecondRound" + characterCode;
        }
        string dialogueCode = category + clickedNum;

        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.SetVariableValue("g_DialogueCode", dialogueCode);
        variableManager.SetVariableValue("g_Mode", "Normal");       

        var switchCommand = new SwitchToNovelMode { ScriptName = category };
        await switchCommand.ExecuteAsync();
    }

    void RefreshBalloonImage()
    {
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject grid = GridPanel.transform.GetChild(i).gameObject;
            grid.transform.Find("BalloonImage").GetComponent<CanvasGroup>().alpha = 0;  
        }
    }


    //public Function ~Ending
    public void ClickEndingBtn()
    {
        windEffect.StopWindEffect();
        
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
        int readNumber = readDialogueList["END"];

        if (readNumber == 8)
        {
            TextPanel.SetActive(true);
        }
        else
        {
            EndingBoardPanel.SetActive(true);  
            SelectionPanel.SetActive(false);
        }        
    }

    public void ClickBackToSelectionBtnInEndingPanel()
    {
        if (EndingSecondPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            EndingSecondPanel.GetComponent<CanvasGroup>().interactable = false;
            EndingSecondPanel.GetComponent<CanvasGroup>().alpha = 0;
            EndingSecondPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

            EndingFirstPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            EndingFirstPanel.GetComponent<CanvasGroup>().alpha = 1;
            EndingFirstPanel.GetComponent<CanvasGroup>().interactable = true;
        }

        EndingBoardPanel.SetActive(false);   
        SelectionPanel.SetActive(true);

        windEffect.RunWindEffect();
    } 

    public void ClickBackToMainBtnInEndingPanel()
    {        
        windEffect.StopWindEffect();

        if (EndingSecondPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            EndingSecondPanel.GetComponent<CanvasGroup>().interactable = false;
            EndingSecondPanel.GetComponent<CanvasGroup>().alpha = 0;
            EndingSecondPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

            EndingFirstPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            EndingFirstPanel.GetComponent<CanvasGroup>().alpha = 1;
            EndingFirstPanel.GetComponent<CanvasGroup>().interactable = true;
        }
        EndingBoardPanel.SetActive(false);        
        BingoPanel.SetActive(false);
        SelectionPanel.SetActive(true);

        MainUIPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        MainUIPanel.GetComponent<CanvasGroup>().alpha = 1;
        MainUIPanel.GetComponent<CanvasGroup>().interactable = true;

        BubblePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        BubblePanel.GetComponent<CanvasGroup>().alpha = 1;
        BubblePanel.GetComponent<CanvasGroup>().interactable = true;

        windEffect.RunWindEffect();
    }

    void CheckEndingConditions()
    {
        List<string> characterCodeList = new List<string>() {"NINI", "DNEE", "GP", "JNM", "LZB", "ULCS"};
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        bool isOK = true;
        for (int i = 0; i < characterCodeList.Count; i++)
        {
            if (readDialogueList[characterCodeList[i]] < 7)
            {
                isOK = false;   
                break;             
            }            
        }

        if (isOK == true && EndingBtn.activeSelf == false)
        {
            EndingBtn.SetActive(true);
        }
    }

    public void ClickTextPanel()
    {
        TextPanel.SetActive(false);
    }

    
    //public Function ~Other
    public void RefreshAfterReading()
    {
        ToAlphaZeroGridIcons();        
        SwitchOffAllStoryBtn();
        SetKeyIcon();     
        SwitchOnStoryBtn();
        SwitchOffOnStoryPopUpPanelWindowBack();
    }

    void SwitchOffOnStoryPopUpPanelWindowBack()
    {
        StoryPopUpPanelWindowBack.SetActive(false);
        StoryPopUpPanelWindowBack.SetActive(true);
    }

    void SetKeyIcon()
    {
        RefreshKeyIcon();
        List<int> keyItemList = new List<int>();
        keyItemList = FindkeyItemsGridNumber();
        if (characterCode != "NINI")
        {
            List<int> currentKeyItemList = new List<int>();
            List<int> notCurrentKeyItemList = new List<int>();
            for (int i = 0; i < keyItemList.Count; i++)
            {
                if (paintedGridList.Contains(keyItemList[i]) == true)
                {
                    currentKeyItemList.Add(keyItemList[i]);
                }
                else
                {
                    notCurrentKeyItemList.Add(keyItemList[i]);
                }
            }

            if (currentKeyItemList.Count > 0)
            {
                for (int i = 0; i < currentKeyItemList.Count; i++)
                {
                    ShowKeyIcon(currentKeyItemList[i], keyItemList.IndexOf(currentKeyItemList[i]));
                }
            }
            if (notCurrentKeyItemList.Count > 0)
            {
                for (int i = 0; i < notCurrentKeyItemList.Count; i++)
                {
                    ShowKeyIcon(notCurrentKeyItemList[i], keyItemList.IndexOf(notCurrentKeyItemList[i]));                  
                }
            }
        }
    }       
}