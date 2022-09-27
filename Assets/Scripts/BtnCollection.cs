using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BansheeGz.BGDatabase;
using TMPro;

public class BtnCollection : MonoBehaviour
{
    BtnUI btnUI;
    public GameObject CollectionPanel;
    public GameObject CollectionFirstTab;
    public GameObject CollectionItemTab;
    public GameObject CollectionBtnPanel;
    public GameObject BagPanel;
    public Object Item;
    public Object ItemFirstTab;
    string category;
    int categorySortNum;
    List<string> coList;
    bool checkCollectionBtn;
    Main main;
    Variables variables;
    string categoryForTextBar;
    Dictionary<string, int> bag;
    public GameObject ScrollF;
    public GameObject ScrollO;

    void Awake()
    {
        main = FindObjectOfType<Main>();
        variables = FindObjectOfType<Variables>();
        btnUI = FindObjectOfType<BtnUI>();

        checkCollectionBtn = false;
        category = "";
        categoryForTextBar = "";
        categorySortNum = -1;

        bag = new Dictionary<string, int>();
    }

    public void CheckCollectionBtn()
    {
        if (checkCollectionBtn == false && CollectionPanel.activeSelf == false)
        {
            checkCollectionBtn = true;                   
            ClickCollectionBtn();
            btnUI.ClickFordedMenuBtn();
            checkCollectionBtn = false;
        }
        else
        {
            Debug.Log("도감 버튼 실행 중");
        }
    }

    void ClickCollectionBtn()
    {
        CollectionBtnPanel.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(85/255f, 20/255f, 20/255f, 1);   
        CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);   

        bag = variables.LoadBag();               

        coList = new List<string>();
        CollectionPanel.SetActive(true);          
        CollectionFirstTab.SetActive(true);              

        GenFirstTabItems();
    }

    public void ClickCollectionExitBtn()
    {
        categorySortNum = -1;

        CollectionFirstTab.SetActive(false);
        CollectionItemTab.SetActive(false);
        CollectionPanel.SetActive(false);

        ClearContentsPanelFirstTab();
    }

    void ClearContentsPanelFirstTab()
    {
        GameObject ContentsPanelFirstTab = CollectionFirstTab.transform.Find("Scroll View").Find("ViewPanel").Find("ContentsPanel").gameObject;
        for (int i = 0; i < ContentsPanelFirstTab.transform.childCount; i++)
        {
            Destroy(ContentsPanelFirstTab.transform.GetChild(i).gameObject);
        } 
    }

    public void ClickCollectionFirstTabBtn()
    {
        categorySortNum = -1;
        categoryForTextBar = "";
        ScrollF.GetComponent<Scrollbar>().value = 1;

        CollectionBtnPanel.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(85/255f, 20/255f, 20/255f, 1);
        CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
        CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);  

        CollectionFirstTab.SetActive(true);
        CollectionItemTab.SetActive(false);
    }

    void GenFirstTabItems()
    {
        List<BGEntity> allItem = new List<BGEntity>();
        allItem = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a"));

        GameObject ContentsPanelFirstTab = CollectionFirstTab.transform.Find("Scroll View").Find("ViewPanel").Find("ContentsPanel").gameObject;
        for (int i = 0; i < allItem.Count; i++)
        {
            string itemName = allItem[i].Name;
            if (bag.ContainsKey(itemName) == true)
            {
                GameObject NewItem = Instantiate(ItemFirstTab, ContentsPanelFirstTab.transform) as GameObject;
                NewItem.name = "(FirstTab)" + itemName;
                NewItem.transform.Find("DictImage").gameObject.GetComponent<Image>().sprite = main.ItemList[itemName];   
            }
        }

        int numberForFirstText = ContentsPanelFirstTab.transform.childCount;
        string secondText = "";
        if (numberForFirstText < 25)
        {
            secondText = "앞으로 어떤 새로움을 마주하게 될까요?";
        }
        else if (numberForFirstText >= 25 && numberForFirstText < 50)
        {
            secondText = "어디를 보든 재밌고 기억에 남아요";
        }
        else if (numberForFirstText >= 50 && numberForFirstText < 75)
        {
            secondText = "아직 기억에 담을 수 있는 풍경이 많아요";
        }
        else if (numberForFirstText >= 75 && numberForFirstText < 100)
        {
            secondText = "비슷해 보여도 가만히 들여다보면 모두 달라요";
        }
        else if (numberForFirstText >= 100 && numberForFirstText < 123)
        {
            secondText = "모두 조화롭게 어우러져 자연을 이루고 있어요";
        }
        else if (numberForFirstText == 123)
        {
            secondText = "바람이 산들산들 기분이 좋아서 잠시 쉬고 있어요";
        }

        GameObject InformationPanelFirstTab = CollectionFirstTab.transform.Find("Scroll View").Find("ViewPanel").Find("InformationPanel").gameObject;
        string inputText0 = numberForFirstText + "개를 기억하고 있어요";
        InformationPanelFirstTab.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = inputText0;
        InformationPanelFirstTab.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = secondText;
    
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentsPanelFirstTab.GetComponent<RectTransform>());          
    }

    public void ClickCollectionItemTabBtn(string inputcategory)
    {        
        ScrollO.GetComponent<Scrollbar>().value = 1;
        CollectionBtnPanel.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(127/255f, 63/255f, 0/255f, 1);
        switch (inputcategory)
        {
            case "Plantae":
            CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);  
            CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(15/255f, 105/255f, 70/255f, 1);
            break;

            case "Animalia0":
            CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);  
            CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(15/255f, 105/255f, 70/255f, 1);
            break;

            case "Animalia1":
            CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);  
            CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(15/255f, 105/255f, 70/255f, 1);
            break;

            case "Minerals":
            CollectionBtnPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color(28/255f, 140/255f, 78/255f, 1);
            CollectionBtnPanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color(15/255f, 105/255f, 70/255f, 1);
            break;

            default:
            break;
        }

        List<char> numList = new List<char> {'0','1','2','3','4','5','6','7','8','9'};

        if (numList.Contains(inputcategory[inputcategory.Length - 1]) == true)
        {
            category = inputcategory.Substring(0, inputcategory.Length - 1);
            categorySortNum = int.Parse(inputcategory[inputcategory.Length - 1].ToString());
        }
        else
        {
            category = inputcategory;
            categorySortNum = -1;
        }
        categoryForTextBar = inputcategory;

        CollectionFirstTab.SetActive(false);              
        CollectionItemTab.SetActive(true);

        if(BagPanel.transform.childCount == 0)
        {          
            ShowCategoryItems();
        }
        else
        {
            for(int i = 0; i < BagPanel.transform.childCount ; i++)
            {
                Destroy(BagPanel.transform.GetChild(i).gameObject);
            }

            ShowCategoryItems();
        }
    }

    void ShowCategoryItems()
    {        
        if(BagPanel.transform.childCount == 0)
        {
            GenCategoryItems();
        }
        else
        {
                                    
            StartCoroutine("WaitAllDestroy");                              
        }                
    }

    void GenCategoryItems()
    {
        List<BGEntity> Items = BGRepo.I["Item"].FindEntities(entity => entity.Name.StartsWith(category));
        int startingIndex;
        int maxCount;
        if (categorySortNum >= 0)
        {
            if (category == "Animalia" && categorySortNum == 0)
            {
                startingIndex = 0;
                maxCount = 26;
            }
            else if (category == "Animalia" && categorySortNum == 1)
            {
                startingIndex = 26;
                maxCount = 45;
            }
            else
            {
                startingIndex = 0;
                maxCount = Items.Count;
            }
        }
        else
        {
            startingIndex = 0;
            maxCount = Items.Count;
        }

        int maxCountNew = 0;
        for(int i = startingIndex; i < maxCount; i++)
        {
            if (bag.ContainsKey(Items[i].Name) == true)
            {
                maxCountNew = i + 1;
            }
        }

        for(int i = startingIndex; i < maxCountNew; i++)
        {
            GameObject NewItem = Instantiate(Item, BagPanel.transform) as GameObject;   
            NewItem.name = Items[i].Name;         
            
            if (bag.ContainsKey(NewItem.name) == true)
            {                
                switch (categoryForTextBar)
                {
                    case "Plantae" : 
                    NewItem.transform.Find("TextBar").gameObject.GetComponent<Image>().color = new Color(0/255f, 127/255f, 0/255f, 1);
                    break;

                    case "Animalia0":
                    NewItem.transform.Find("TextBar").gameObject.GetComponent<Image>().color = new Color(218/255f, 110/255f, 10/255f, 1);
                    break;

                    case "Animalia1":
                    NewItem.transform.Find("TextBar").gameObject.GetComponent<Image>().color = new Color(191/255f, 63/255f, 63/255f, 1);  
                    break;

                    case "Minerals":
                    NewItem.transform.Find("TextBar").gameObject.GetComponent<Image>().color = new Color(68/255f, 68/255f, 68/255f, 1); 
                    break;

                    default:
                    break;
                }

                NewItem.transform.Find("DictImage").gameObject.GetComponent<Image>().sprite = main.ItemList[NewItem.name];                             
                
                int j = 0;
                while (Items[j].Name != NewItem.name)
                {
                    j++;
                }
                
                NewItem.transform.GetComponentInChildren<TextMeshProUGUI>().text = Items[j].Get<string>("kr");
            }
            else
            {                
                NewItem.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    IEnumerator WaitAllDestroy()
    {
        coList.Add("WaitAllDestroy");
        yield return new WaitUntil(() => BagPanel.transform.childCount == 0);
        GenCategoryItems();
        coList.Clear();                       
    }
}
