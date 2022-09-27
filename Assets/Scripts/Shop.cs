using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BansheeGz.BGDatabase;
using TMPro;

public class Shop : MonoBehaviour
{
    //Sc
    Variables variables;    
    Main main;
    BtnUI btnUI;

    //public Object
    public Object ItemPanel;

    //public GameObject    
    public GameObject DisplayPanel;        
    public GameObject PopUpPanel;
    public GameObject PurchaseBtn;
    public GameObject InkAmountPanel;
    public TextMeshProUGUI ItemCostText;
    public List<GameObject> ShopPopUpCategoryBorderList;
    public List<GameObject> ShopPopUpCharacterBorderList;
    public List<GameObject> ShopPopUpKeyCharacterList;

    //private variable
    public int inkAmount;
    public int squares;
    public int itemCost;
    List<BGEntity> allItemsList;
    List<string> allItemNamesList;
    Dictionary<string, int> bag;
    List<int> shopItemNumbers;    
    public List<int> closeKeyItemNumbers;


    //basic Function
    void Awake()
    {
        variables = FindObjectOfType<Variables>();
        main = FindObjectOfType<Main>();
        btnUI = FindObjectOfType<BtnUI>();    

        squares = 9;
        itemCost = 2;
        ItemCostText.text = itemCost.ToString();
        allItemsList = new List<BGEntity>();
        allItemNamesList = new List<string>();
        allItemNamesList = CreateAllItemNamesList();
        bag = new Dictionary<string, int>();
        shopItemNumbers = new List<int>() {-1, -1, -1, -1, -1, -1, -1, -1, -1};
        closeKeyItemNumbers = new List<int>();
    }

    void OnEnable()
    {                                                      
        RefreshInkAmount();

        Debug.Log("상점 아이템 채우기 시작!");          
        CreateSquares();        
        ShowDisplayPanel();
        SetGlassLightSite();
    }

    void OnDisable()
    {
        btnUI.RefreshInkAmount();
    }


    //Function related to basic Function
    List<string> CreateAllItemNamesList()
    {
        allItemsList = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a")); 
        for(int i = 0; i < allItemsList.Count; i++)
        {
            allItemNamesList.Add(allItemsList[i].Name);
        }
        Debug.Log("모든 아이템 이름 리스트 만들기! (상점)"); 
        return allItemNamesList;
    }

    void RefreshInkAmount()
    {
        inkAmount = variables.LoadInk();
        if (inkAmount < 1000)
        {
            InkAmountPanel.GetComponentInChildren<TextMeshProUGUI>().text = inkAmount.ToString();  
        }
        else if (inkAmount >= 1000)
        {
            InkAmountPanel.GetComponentInChildren<TextMeshProUGUI>().text = "999+";
        }
        else
        {
            InkAmountPanel.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        }              
    }

    void CreateSquares()
    {        
        bag = variables.LoadBag();
        FindCloseKeyItemNumbers();
        LoadShop();  
        CheckShop();             
        ChangeItemPanels();      
    }

    void FindCloseKeyItemNumbers()
    {
        closeKeyItemNumbers.Clear();

        List<string> charaList = new List<string>() {"DNEE", "GP", "JNM", "LZB", "ULCS"};
        for (int i = 0; i < charaList.Count; i++)
        {                    
            List<string> keyItemsList = new List<string>();
            keyItemsList = FindKeyItemNames(charaList[i]);
            List<int> keyItemNumbersListTmp = new List<int>();
            keyItemNumbersListTmp = FindKeyItemNumbers(charaList[i]);
            for (int j = 0; j < keyItemsList.Count; j++)
            {
                if (bag.ContainsKey(keyItemsList[j]) == false)
                {
                    if (closeKeyItemNumbers.Contains(keyItemNumbersListTmp[j]) == false)
                    {
                        closeKeyItemNumbers.Add(keyItemNumbersListTmp[j]);
                    }                    
                    break;
                }                       
            }
        }    

        Debug.Log("가까운 키아이템 리스트 생성");        
    }

    List<int> FindKeyItemNumbers(string characterCode)
    {
        List<string> keyItemsList = new List<string>();
        keyItemsList = FindKeyItemNames(characterCode);

        List<int> keyItemNumbersList = new List<int>();             
        for (int i = 0; i < keyItemsList.Count; i++)
        {
            keyItemNumbersList.Add(allItemNamesList.IndexOf(keyItemsList[i]));            
        }

        return keyItemNumbersList;
    }

    List<string> FindKeyItemNames(string characterCode)
    {
        List<string> keyItemsList = new List<string>();
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

    void LoadShop()
    {
        if (ES3.KeyExists("shopItemNumbers") == true)
        {
            Debug.Log("불러오기: 상점");
            shopItemNumbers = ES3.Load<List<int>>("shopItemNumbers");
        }
        else
        {
            CreateNumbers();
            SaveShop();
        }
    }

    void CreateNumbers()
    {
        for (int i = 1; i < squares; i++)
        {
            int randNumber = 0;
            string randItemName = "";
            do
            {
                randNumber = Random.Range(0, allItemNamesList.Count);
                randItemName = allItemNamesList[randNumber];
            } while (bag.ContainsKey(randItemName) == true || shopItemNumbers.Contains(randNumber) == true);
                                    
            shopItemNumbers[i] = randNumber;                        
        }
                 
        
        bool isContainedCloseKeyItemNumbers = false;
        for (int i = 0; i < closeKeyItemNumbers.Count; i++)
        {
            if (shopItemNumbers.Contains(closeKeyItemNumbers[i]) == true)
            {
                isContainedCloseKeyItemNumbers = true;
                break;
            }
        }
            
        if (isContainedCloseKeyItemNumbers == false)
        {
            while (shopItemNumbers[0] == -1)
            {                    
                int randNumber = closeKeyItemNumbers[Random.Range(0, closeKeyItemNumbers.Count)];                    
                if (shopItemNumbers.Contains(randNumber) == false)
                {
                    shopItemNumbers[0] = randNumber;
                }                    
            }                                                                   
        }
        else
        {
            while (shopItemNumbers[0] == -1)
            {
                int randNumber = Random.Range(0, allItemNamesList.Count);
                string randItemName = allItemNamesList[randNumber];
            
                if (bag.ContainsKey(randItemName) == false)
                {
                    if (shopItemNumbers.Contains(randNumber) == false)
                    {
                        shopItemNumbers[0] = randNumber;
                    }
                }
            }                  
        }                               

        for (int i = 0; i < shopItemNumbers.Count; i++)
        {
            int randNumberTmp = Random.Range(0, shopItemNumbers.Count);
            int numberTmp = shopItemNumbers[i];
            shopItemNumbers[i] = shopItemNumbers[randNumberTmp];
            shopItemNumbers[randNumberTmp] = numberTmp;
        }

        Debug.Log("생성할 품목 결정: " + shopItemNumbers.Count + "개 (상점)"); 
    }

    void SaveShop()
    {
        Debug.Log("저장하기: 상점");
        ES3.Save<List<int>>("shopItemNumbers", shopItemNumbers);
    }

    void CheckShop()
    {
        bool changeOK = false;
        List<int> OrdersNeedToBeFilled = new List<int>();
        for (int i = 0; i < squares; i++)
        {
            if (shopItemNumbers[i] != -1)
            {
                string itemName = allItemNamesList[shopItemNumbers[i]];
                if (bag.ContainsKey(itemName) == true)
                {
                    Debug.Log("획득했으므로 상점에서 제거: " + itemName);
                    shopItemNumbers[i] = -1;
                    SaveShop(); 
                    changeOK = true;
                    OrdersNeedToBeFilled.Add(i);
                }
            }            
        }

        if (OrdersNeedToBeFilled.Count > 0)
        {
            for (int i = 0; i < OrdersNeedToBeFilled.Count; i++)
            {
                int randNumberTmp = OrdersNeedToBeFilled[i];
                int randNumber = Random.Range(0, OrdersNeedToBeFilled.Count);
                OrdersNeedToBeFilled[i] = OrdersNeedToBeFilled[randNumber];
                OrdersNeedToBeFilled[randNumber] = randNumberTmp;
            }
        }

        List<string> ItemNamesAvailableForDisplay = new List<string>();
        for (int i = 0; i < allItemNamesList.Count; i++)
        {
            if (bag.ContainsKey(allItemNamesList[i]) == false && shopItemNumbers.Contains(allItemNamesList.IndexOf(allItemNamesList[i])) == false)
            {
                ItemNamesAvailableForDisplay.Add(allItemNamesList[i]);
            }
        }
        Debug.Log("상점 표시 가능 아이템 수: " + ItemNamesAvailableForDisplay.Count);

        if (changeOK == true)
        {            
            if (ItemNamesAvailableForDisplay.Count > 0)
            {
                for (int i = 0; i < OrdersNeedToBeFilled.Count; i++)
                {
                    if (ItemNamesAvailableForDisplay.Count > 0)
                    {                                         
                        if (i == 0 && CheckCloseKeyItemContained() == false)
                        {                               
                            if (closeKeyItemNumbers.Count > 0)
                            {
                                int randNumber = Random.Range(0, closeKeyItemNumbers.Count);
                                string randItemName = allItemNamesList[closeKeyItemNumbers[randNumber]];
                                shopItemNumbers[OrdersNeedToBeFilled[i]] = closeKeyItemNumbers[randNumber]; 
                                ItemNamesAvailableForDisplay.Remove(randItemName);
                                Debug.Log("진열창에 가까운 키아이템이 없으므로 추가([" + closeKeyItemNumbers[randNumber] + "]" + randItemName + ")"); 
                                SaveShop(); 
                            }
                            else
                            {
                                Debug.Log("이미 키아이템 모두 획득"); 
                                int randNumber = 0;
                                string randItemName = "";
                                do
                                {
                                    int randNumberTmp = Random.Range(0, ItemNamesAvailableForDisplay.Count); 
                                    randItemName = ItemNamesAvailableForDisplay[randNumberTmp]; 
                                    randNumber = allItemNamesList.IndexOf(randItemName);
                                }
                                while (shopItemNumbers.Contains(randNumber) == true);
                                
                                shopItemNumbers[OrdersNeedToBeFilled[i]] = randNumber; 
                                ItemNamesAvailableForDisplay.Remove(randItemName); 
                                SaveShop();  
                            }                               
                        }
                        else
                        {
                            int randNumber = 0;
                            string randItemName = "";
                            do
                            {
                                int randNumberTmp = Random.Range(0, ItemNamesAvailableForDisplay.Count); 
                                randItemName = ItemNamesAvailableForDisplay[randNumberTmp]; 
                                randNumber = allItemNamesList.IndexOf(randItemName);
                            } while (shopItemNumbers.Contains(randNumber) == true);

                            shopItemNumbers[OrdersNeedToBeFilled[i]] = randNumber; 
                            ItemNamesAvailableForDisplay.Remove(randItemName); 
                            SaveShop();  
                        }                                          
                    }                                               
                }
            }
            
            Debug.Log("체크 상점: 변화");  
        }         
        else
        {
            Debug.Log("체크 상점: 변화 없음");
        }    
    }

    bool CheckCloseKeyItemContained()
    {
        bool isContained = false;
        for (int j = 0; j < closeKeyItemNumbers.Count; j++)
        {
            if (shopItemNumbers.Contains(closeKeyItemNumbers[j]) == true)
            {
                Debug.Log("진열창에 가까운 키아이템이 있음"); 
                isContained = true;
            }
        }
        
        if (isContained == true) return true;
        else return false;
    }

    void ChangeItemPanels()
    {        
        List<int> keyItemNumbersListAll = new List<int>();
        List<string> charaList = new List<string>() {"DNEE", "GP", "JNM", "LZB", "ULCS"};
        for (int i = 0; i < charaList.Count; i++)
        {
            List<int> keyItemNumbersListTmp = new List<int>();
            keyItemNumbersListTmp = FindKeyItemNumbers(charaList[i]);            
            for (int j = 0; j < keyItemNumbersListTmp.Count; j++)
            {
                keyItemNumbersListAll.Add(keyItemNumbersListTmp[j]);
            }
        }
                
        int displayPanelChildCount = DisplayPanel.transform.childCount;
        if (displayPanelChildCount < squares)
        {
            for (int i = displayPanelChildCount; i < squares; i++)
            {
                GameObject NewItem = Instantiate(ItemPanel, DisplayPanel.transform) as GameObject;           
            }
        }        
        for (int i = 0; i < squares; i++)
        {
            GameObject DisplayPanelChild = DisplayPanel.transform.GetChild(i).gameObject;
            if (shopItemNumbers[i] != -1)
            {
                DisplayPanelChild.name = ItemPanel.name + i;
                DisplayPanelChild.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = allItemsList[shopItemNumbers[i]].Get<string>("kr");
                DisplayPanelChild.transform.Find("Seal").Find("Image").GetComponent<Image>().sprite = main.ItemList[allItemNamesList[shopItemNumbers[i]]];

                if (closeKeyItemNumbers.Contains(shopItemNumbers[i]) == true)
                {
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaper").GetComponent<Image>().color = new Color(0, 0, 0, 1);  
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaperLight").GetComponent<Image>().color = new Color(1, 1, 1, 1); 
                }
                else if (keyItemNumbersListAll.Contains(shopItemNumbers[i]) == true && closeKeyItemNumbers.Contains(shopItemNumbers[i]) == false)
                {
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaper").GetComponent<Image>().color = new Color(0, 0, 0, 1);  
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaperLight").GetComponent<Image>().color = new Color(1, 1, 1, 0); 
                }
                else
                {
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaper").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    DisplayPanelChild.transform.Find("Seal").Find("UnderPaperLight").GetComponent<Image>().color = new Color(1, 1, 1, 0); 
                }
            }     
            else
            {
                DisplayPanelChild.GetComponent<Button>().interactable = false;
                DisplayPanelChild.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = "매진";
                DisplayPanelChild.transform.Find("Seal").GetComponent<CanvasGroup>().alpha = 0;
            }       
        }
    }

    void ShowDisplayPanel()
    {
        DisplayPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        DisplayPanel.GetComponent<CanvasGroup>().alpha = 1;        
        DisplayPanel.GetComponent<CanvasGroup>().interactable = true;
    }

    void SetGlassLightSite()
    {
        for (int i = 0; i < DisplayPanel.transform.childCount; i++)
        {
            GameObject DisplayPanelChild = DisplayPanel.transform.GetChild(i).gameObject;   
            GameObject GlassLight = DisplayPanelChild.transform.Find("Glass").Find("GlassLight").gameObject;
            RectTransform glassLightRT = GlassLight.GetComponent<RectTransform>();
            int glassLightWidth = (int)glassLightRT.sizeDelta.x;
            int randNumberX = Random.Range(-glassLightWidth/2, glassLightWidth/2);
            GlassLight.GetComponent<RectTransform>().localPosition = new Vector3 (randNumberX, glassLightRT.localPosition.y, glassLightRT.localPosition.z);
        }        
    }

    
    //public Function
    public void ClickItemPanel(int index)
    {
        FillItemSquare(index);
        ChangePopUpIconBorderColor(index);
        SetPopUpKeyCharacter(index);
        CheckPurchaseBtn();
        ShowPopUpPanel();
    }

    void FillItemSquare(int clickedIndex)
    {
        GameObject ItemSquare = PopUpPanel.transform.Find("ItemSquare").gameObject;
        GameObject DisplayPanelChild = DisplayPanel.transform.GetChild(clickedIndex).gameObject;            
        ItemSquare.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = DisplayPanelChild.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text; 
        ItemSquare.transform.Find("Image").GetComponent<Image>().sprite = DisplayPanelChild.transform.Find("Seal").Find("Image").GetComponent<Image>().sprite;
        ItemSquare.transform.Find("UnderPaper").GetComponent<Image>().color = DisplayPanelChild.transform.Find("Seal").Find("UnderPaper").GetComponent<Image>().color;
        ItemSquare.transform.Find("UnderPaperLight").GetComponent<Image>().color = DisplayPanelChild.transform.Find("Seal").Find("UnderPaperLight").GetComponent<Image>().color;
        PurchaseBtn.name = "PurchaseBtn" + clickedIndex;
    }

    void ChangePopUpIconBorderColor(int clickedIndex)
    {
        int itemNumber = shopItemNumbers[clickedIndex];
        string itemName = allItemNamesList[itemNumber];
        if (itemName[0] == 'P')
        {
            ShopPopUpCategoryBorderList[0].GetComponent<Image>().color = new Color (0, 0, 0, 1);
            ShopPopUpCategoryBorderList[1].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
            ShopPopUpCategoryBorderList[2].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
        }
        else if (itemName[0] == 'A')
        {
            ShopPopUpCategoryBorderList[0].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
            ShopPopUpCategoryBorderList[1].GetComponent<Image>().color = new Color (0, 0, 0, 1);
            ShopPopUpCategoryBorderList[2].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
        }
        else if (itemName[0] == 'M')
        {
            ShopPopUpCategoryBorderList[0].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
            ShopPopUpCategoryBorderList[1].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
            ShopPopUpCategoryBorderList[2].GetComponent<Image>().color = new Color (0, 0, 0, 1);
        }
        else
        {
           ShopPopUpCategoryBorderList[0].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
           ShopPopUpCategoryBorderList[1].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1); 
           ShopPopUpCategoryBorderList[2].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
        }

        List<string> characterCodeList = new List<string>() {"NINI", "DNEE", "GP", "JNM", "LZB", "ULCS"};
        for (int i = 0; i < characterCodeList.Count; i++)
        {
            List<string> ItemsListByCharac = new List<string>();        
            string gridInformationByCharac = "gridItemListBy" + characterCodeList[i];
            ItemsListByCharac = ES3.Load<List<string>>(gridInformationByCharac);

            if (ItemsListByCharac.Contains(itemName) == true)
            {
                ShopPopUpCharacterBorderList[i].GetComponent<Image>().color = new Color (0, 0, 0, 1);
            }
            else
            {
                ShopPopUpCharacterBorderList[i].GetComponent<Image>().color = new Color (145/255f, 235/255f, 255/255f, 1);
            }
        }        
    }

    void SetPopUpKeyCharacter(int clickedIndex)
    {
        int itemNumber = shopItemNumbers[clickedIndex];

        List<int> keyCharacterList = new List<int>();
        List<int> keyCharacterOrderList = new List<int>();
        List<string> charaList = new List<string>() {"DNEE", "GP", "JNM", "LZB", "ULCS"};
        for (int i = 0; i < charaList.Count; i++)
        {
            List<int> keyItemNumbersListTmp = new List<int>();
            keyItemNumbersListTmp = FindKeyItemNumbers(charaList[i]);                        

            if (keyItemNumbersListTmp.Contains(itemNumber) == true)
            {
                keyCharacterList.Add(i);
                keyCharacterOrderList.Add(keyItemNumbersListTmp.IndexOf(itemNumber));
            }
        }

        for (int i = 0; i < ShopPopUpKeyCharacterList.Count; i++)
        {
            ShopPopUpKeyCharacterList[i].GetComponent<CanvasGroup>().alpha = 0;
        }
        
        if (keyCharacterList.Count > 0)
        {
            for (int i = 0; i < keyCharacterList.Count; i++)
            {
                ShopPopUpKeyCharacterList[keyCharacterList[i]].GetComponentInChildren<TextMeshProUGUI>().text = (keyCharacterOrderList[i] + 2).ToString();
                ShopPopUpKeyCharacterList[keyCharacterList[i]].GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }

    void CheckPurchaseBtn()
    {      
        if (inkAmount >= itemCost)
        {
            PurchaseBtn.GetComponent<Button>().interactable = true;
            PurchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = "구매";
        }
        else
        {
            PurchaseBtn.GetComponent<Button>().interactable = false;
            PurchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = "부족";
        }
    }

    void ShowPopUpPanel()
    {
        PopUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        PopUpPanel.GetComponent<CanvasGroup>().alpha = 1;
        PopUpPanel.GetComponent<CanvasGroup>().interactable = true;
    }


    public void HidePopUpPanel()
    {
        PopUpPanel.GetComponent<CanvasGroup>().interactable = false;
        PopUpPanel.GetComponent<CanvasGroup>().alpha = 0;
        PopUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        SetGlassLightSite();
    }


    public void ClickPurchaseBtn()
    {
        int inkAmountTmp = inkAmount;
        inkAmountTmp -= itemCost;

        int clickedIndex = int.Parse(PurchaseBtn.name[(PurchaseBtn.name.Length - 1)].ToString());
        string itemName = allItemNamesList[shopItemNumbers[clickedIndex]];
        bag.Add(itemName, 1);        
        Debug.Log("아이템 구매 성공! [" + clickedIndex + "]" + itemName);

        variables.SaveBag(bag);
        variables.SaveInk(inkAmountTmp); 
        Debug.Log("아이템 구매 반영");        

        RefreshInkAmount();
        HidePopUpPanel();
        HideDisplayPanel();
        CreateSquares();    
        ShowDisplayPanel();   
        SetGlassLightSite();
    }

    void HideDisplayPanel()
    {
        DisplayPanel.GetComponent<CanvasGroup>().interactable = false;        
        DisplayPanel.GetComponent<CanvasGroup>().alpha = 0;        
        DisplayPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }    
}
