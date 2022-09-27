using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BansheeGz.BGDatabase;

public class WorldItem : MonoBehaviour
{
    public GameObject BubblePanel;
    public GameObject CoverPanel;
    BtnGain btnGain;
    Variables variables;
    bool loadWorldItem;
    public bool itemRoutineGap;
    public GameObject GroupGenItems;
    public Object Item;
    public int MinGenSec;
    public int MaxGenSec;
    public int currentTimeInterval;
    List<string> myWorldItems;  
    public Dictionary<string, int> outcome;  
    public List<string> itemRoutinCoList;
    Main main;    
    public int countP;
    public int countA;
    public int countM;
    List<Vector3> siteList;
    int blankCount;
    public GameObject TextPanel;

    void Awake()
    {        
        variables = FindObjectOfType<Variables>();
        main = FindObjectOfType<Main>();
        loadWorldItem = false;
        itemRoutinCoList = new List<string>();
        blankCount = 30;
        countP = countA = countM = 0;
        siteList = new List<Vector3>();
    }

    void OnEnable()
    {
        Debug.Log("메인 씬 진입");        
        siteList = SetSiteList();
        myWorldItems = variables.LoadWorldItem();        
        LoadOutcome();               
    }    

    void Update()
    {        
        if (CoverPanel.GetComponent<CanvasGroup>().blocksRaycasts == false && BubblePanel.GetComponent<CanvasGroup>().alpha == 0)
        {
            if (GroupGenItems.transform.childCount == 0)
            {
                GenPreviousItem(myWorldItems);                              
            }
            if (countP != 0 || countA != 0 || countM != 0)
            {
                Debug.Log("미니게임 결과 생성 시작");  
                StartGenItem();
            }  

            BubblePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            BubblePanel.GetComponent<CanvasGroup>().interactable = true;
            BubblePanel.GetComponent<CanvasGroup>().alpha = 1;
        }
        else if (CoverPanel.GetComponent<CanvasGroup>().blocksRaycasts == false && BubblePanel.GetComponent<CanvasGroup>().alpha == 1)
        {

        }
        else
        {
            BubblePanel.GetComponent<CanvasGroup>().alpha = 0;
            BubblePanel.GetComponent<CanvasGroup>().interactable = false;
            BubblePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;                    
        }  
    }

    void LoadOutcome()
    {
        Debug.Log("불러오기: 게임 결과");
        if(ES3.KeyExists("outcome") == true)
        {
            Dictionary<string, int> outcome = ES3.Load<Dictionary<string, int>>("outcome");
            if (outcome["new"] == 0)
            {
                outcome["P"] = 0;
                outcome["A"] = 0;
                outcome["M"] = 0;
                ES3.Save("outcome", outcome);
            }
            else
            {
                countP = outcome["P"];
                countA = outcome["A"];
                countM = outcome["M"];
                outcome["P"] = outcome["A"] = outcome["M"] = outcome["new"] = 0;
                ES3.Save("outcome", outcome);
            }
        }
    }

    public void StartGenItem()
    {
        myWorldItems = variables.LoadWorldItem();

        while (loadWorldItem == false)
        {

        }

        StartCoroutineItemRoutine();
    }

    public IEnumerator ItemRoutine()
    {
        Debug.Log("수집품 출현 코루틴 실행");
        CheckBlank(); 
                
        yield return new WaitForSecondsRealtime(1.0f);

        if (blankCount > 0)
        {
            for(int i = 0 ; i < blankCount ; i ++)
            {
                itemRoutineGap = true;
                currentTimeInterval = RandomTimeInterval();
                yield return new WaitForSecondsRealtime(currentTimeInterval);
                itemRoutineGap = false;                                           
                GenItem();                
            }  
        }

        CheckBlank(); 
        if (blankCount == 0)
        {
            Debug.Log("수집 중지: 칸 가득 참!");
            StopCoroutineItemRoutine();
            
            Dictionary<string, int> bag = new Dictionary<string, int>();
            bag = variables.LoadBag();

            List<BGEntity> allItem = new List<BGEntity>();
            allItem = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a"));   

            if (bag.Count != allItem.Count)
            {
                TextPanel.SetActive(true);
            }            
        }
         
    }

    int RandomTimeInterval()
    {
        int time = Random.Range(MinGenSec,MaxGenSec);
        Debug.Log("수집품 출현 시간 간격: "+time);
        return time;
    }

    void GenItem()
    {
        string key = "";
        int fenceNumberA = countP;
        int fenceNumberB = countP + countA;   
        int randKeyNumber = Random.Range(0, countP + countA + countM);
        if (fenceNumberA != 0 && randKeyNumber < fenceNumberA)
        {
            key = "P";
        }
        else if (fenceNumberB != 0 && randKeyNumber >= fenceNumberA && randKeyNumber < fenceNumberB)
        {
            key = "A";
        }
        else
        {
            key = "M";
        }
        Debug.Log("아이템 생성 랜덤 숫자: " + randKeyNumber + "[" + key + "]");

        int randSiteNumber = 0;
        do
        {
            randSiteNumber = Random.Range(0, siteList.Count);
        } while (myWorldItems[randSiteNumber] != "blank");

        Vector3 site = siteList[randSiteNumber];
        float randXNumber = Random.Range(-40, 40);
        float randYNumber = Random.Range(-40, 40);
        Vector3 siteAddedRand = new Vector3(site.x + randXNumber, site.y + randYNumber, site.z);
        Debug.Log("수집품 출현 좌표: " + siteAddedRand);

        GameObject NewItem = Instantiate(Item, GroupGenItems.transform) as GameObject;
      
        NewItem.transform.localPosition = siteAddedRand;
        NewItem.transform.localScale = Vector3.one; 
        
        List<BGEntity> Items = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains(key));
        int randNumber = Random.Range(0, Items.Count);
        string randItemName = Items[randNumber].Name; 
        NewItem.transform.Find("ImageM").GetComponent<Image>().sprite = main.ItemList[randItemName];
        if (randSiteNumber < 10)
        {
            NewItem.name = "Site0" + randSiteNumber + "-" + randItemName;
        }
        else
        {
            NewItem.name = "Site" + randSiteNumber + "-" + randItemName;
        }
        NewItem.GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0 , Random.Range(0, 360));
        
        myWorldItems[randSiteNumber] = randItemName;
        variables.SaveWorldItem(myWorldItems);

        Debug.Log("출현 수집품: "+ NewItem.name);

        if (key == "P") countP--;
        else if (key == "A") countA--;
        else if (key == "M") countM--;
            
        if(countP <= 0 && countA <= 0 && countM <=0)
        {
            Debug.Log("수집 중지: 카운트 소진!");
            StopCoroutineItemRoutine();
        }
    }

    public void ClearItemFromVariable()
    {
        if(itemRoutinCoList.Count == 0)
        {
            Debug.Log("수집 재개 준비");
            myWorldItems = variables.LoadWorldItem();
            for (int i = 0; i < myWorldItems.Count; i++)
            {
                myWorldItems[i] = "blank";
            }
            variables.SaveWorldItem(myWorldItems);
        }
    }

    public void RemoveItemFromVariable(GameObject Item)
    {
        if(itemRoutinCoList.Count == 0)
        {
            Debug.Log("수집 재개 준비");
            int siteNumber = 0;
            if (Item.name[4] == '0')
            {
                siteNumber = Item.name[5] - '0';
            }
            else
            {
                int siteNumberA = Item.name[4] - '0';
                int siteNumberB = Item.name[5] - '0';
                siteNumber = siteNumberA*10 + siteNumberB;
            }            
            myWorldItems = variables.LoadWorldItem();
            myWorldItems[siteNumber] = "blank";
            variables.SaveWorldItem(myWorldItems);
        }
    }

    public void StopItemRoutine()
    {
        if(itemRoutinCoList.Count == 1)
        {        
            if (itemRoutineGap == false)
            {
                for(;;)
                {
                    if(itemRoutineGap == true)
                    {
                        StopCoroutineItemRoutine();
                        break;
                    }
                }
            
            }
            else
            {
                StopCoroutineItemRoutine();
            }
        }
        else
        {
            
        }
    }

    void GenPreviousItem(List<string> previousWorldItems)
    {
        itemRoutineGap = false;

        List<int> shuffleList = new List<int>();        
        for (int i = 0; i < previousWorldItems.Count; i++)
        {
            shuffleList.Add(i);
        }
        for (int i = 0; i < shuffleList.Count; i++)
        {
            int randShuffleNumber = Random.Range(0, shuffleList.Count);
            int numberTmp = shuffleList[i];
            shuffleList[i] = shuffleList[randShuffleNumber];
            shuffleList[randShuffleNumber] = numberTmp;          
        }        

        for (int i = 0; i < shuffleList.Count; i++)
        {
            int randShuffleNumber = shuffleList[i];
            string worldItemName = previousWorldItems[randShuffleNumber];
            if (worldItemName == "blank")
            {
                continue;
            } 

            Vector3 site = siteList[randShuffleNumber];

            GameObject NewItem = Instantiate(Item, GroupGenItems.transform) as GameObject;
            NewItem.transform.Find("ImageM").GetComponent<Image>().sprite = main.ItemList[worldItemName];

            float randXNumber = Random.Range(-40, 40);
            float randYNumber = Random.Range(-40, 40);
            Vector3 siteAddedRand = new Vector3(site.x + randXNumber, site.y + randYNumber, site.z);
            NewItem.transform.localPosition = siteAddedRand;
            NewItem.transform.localScale = Vector3.one;

            if (randShuffleNumber < 10)
            {
                NewItem.name = "Site0" + randShuffleNumber + "-" + worldItemName;
            }
            else
            {
                NewItem.name = "Site" + randShuffleNumber + "-" + worldItemName;
            }
            NewItem.GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0 , Random.Range(0, 360));
            Debug.Log("정보 불러오기: 출현 수집품-"+ NewItem.name);
        }

        Debug.Log("출현 수집품 정보 불러오기 완료");

        loadWorldItem = true;
    }

    void StartCoroutineItemRoutine()
    {
        if(itemRoutinCoList.Count == 0)
        {
            StartCoroutine("ItemRoutine");
            itemRoutinCoList.Add("ItemRoutine");
        }
        else
        {
            Debug.Log("루틴 이미 실행 중! " + itemRoutinCoList.Count);
        }
    }

    void StopCoroutineItemRoutine()
    {
        StopCoroutine("ItemRoutine");
        itemRoutinCoList.Clear();
        Debug.Log("수집품 출현 중지!");
    }

    List<Vector3> SetSiteList()
    {
        List<Vector3> siteListTmp = new List<Vector3>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int posX = 90 + 156 * i - 455;
                int posY = 90 + 156 * j - 377;
                Vector3 site = new Vector3(posX, posY, 0);
                siteListTmp.Add(site);
            }
        }
        return siteListTmp;
    }

    void CheckBlank()
    {
        int blankCountTmp = 0;
        for (int i = 0; i < myWorldItems.Count; i++)
        {
            if (myWorldItems[i] == "blank")
            {   
                blankCountTmp++;
            } 
        }

        blankCount = blankCountTmp;

        Debug.Log("빈 칸: " + blankCount + "칸");
    }

    public void ClickMainTextPanel()
    {
        TextPanel.SetActive(false);
    }
}
