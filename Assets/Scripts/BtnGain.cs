using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnGain : MonoBehaviour
{
    Variables variables;
    WorldItem worldItem;
    WindEffect windEffect;
    Dictionary<string, int> bag;
    public GameObject MainUIPanel;
    public GameObject GroupGenItems;
    public GameObject GainBtn;
    public GameObject InkPanel;
    public float speed;
    public bool gainBtnRunning;
    bool checkGainBtn;
    float clickedTime;
    public float longTime;
    bool isClick;

    void Awake()
    {
        variables = FindObjectOfType<Variables>();
        worldItem = FindObjectOfType<WorldItem>();
        windEffect = FindObjectOfType<WindEffect>();
        gainBtnRunning = false;
        checkGainBtn = false;
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
            CheckGainBtn();
        }
    }

    public void CheckGainBtn()
    {
        if (checkGainBtn == false)
        {
            checkGainBtn = true;
            Debug.Log("모두 얻기 버튼 실행 시작");
            ClickGainBtn();
            windEffect.RunWindEffect();
            checkGainBtn = false;
        }
        else
        {
            Debug.Log("얻기 버튼 실행 중");
        }
    }

    void ClickGainBtn()
    {
        if (gainBtnRunning == false)
        {
            gainBtnRunning = true;
            worldItem.StopItemRoutine();
            List<GameObject> CurrentWorldItemList = SearchItem();
            if (CurrentWorldItemList.Count > 0)
            {
                AddBag(CurrentWorldItemList);
                AlphaZeroAndDestroy(CurrentWorldItemList);                                    
            }
            else
            {
                Debug.Log("수집품이 없음!");                
            }
            worldItem.ClearItemFromVariable();    
            gainBtnRunning = false;
        }
        else
        {
            Debug.Log("얻기 대기!");

            while (gainBtnRunning == true)
            {
                
            }
            
            gainBtnRunning = true;
            worldItem.StopItemRoutine();
            List<GameObject> CurrentWorldItemList = SearchItem();
            if (CurrentWorldItemList.Count > 0)
            {
                AddBag(CurrentWorldItemList); 
                AlphaZeroAndDestroy(CurrentWorldItemList);                             
                worldItem.ClearItemFromVariable();
                gainBtnRunning = false;                
            }
            else
            {
                Debug.Log("수집품이 없음!");
                worldItem.ClearItemFromVariable();
                gainBtnRunning = false;
            } 
        }
    }

    List<GameObject> SearchItem()
    {
        List<GameObject> FoundItems = new List<GameObject>();        
        for (int i = 0; i < GroupGenItems.transform.childCount; i++)
        {
            FoundItems.Add(GroupGenItems.transform.GetChild(i).gameObject);
        }
        return FoundItems;
    }

    void AlphaZeroAndDestroy(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            MoveCollection(list[i]);

            list[i].GetComponent<CanvasGroup>().alpha = 0;
            list[i].GetComponent<CanvasGroup>().interactable = false;
            list[i].GetComponent<CanvasGroup>().blocksRaycasts = false;

            Destroy(list[i]);
        }

        BtnUI btnUI = FindObjectOfType<BtnUI>(); 
        btnUI.RefreshInkAmount();
    }

    void MoveCollection(GameObject Item)
    {
        GameObject NewItemClone = Instantiate(Item, MainUIPanel.transform);
        NewItemClone.transform.position = Item.transform.position;
        NewItemClone.transform.localScale = Vector3.one;
        NewItemClone.name = Item.name+"(Clone)";

        StartCoroutine(MovePoint(NewItemClone));
    }

    IEnumerator MovePoint(GameObject Obj)
    {
        Vector3 targetPoint = this.transform.position;

        if (Obj != null)
        {
            for (int i = 0; i < 24; i++)
            {
                if (Obj != null)
                {            
                    Obj.transform.position = Vector3.Lerp(Obj.transform.position, targetPoint, speed);
                    yield return new WaitForSecondsRealtime(0.04f);       
                }
                else
                {
                    yield return null;
                    break;
                }
            }

            if (Obj != null)
            { 
                Obj.GetComponent<CanvasGroup>().alpha = 0;
                Obj.GetComponent<CanvasGroup>().interactable = false;
                Obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
                
                Destroy(Obj);
            }
        }
    }

    public void GainOneWorldItem(GameObject CurrentBtn)
    {
        if (gainBtnRunning == false)
        {
            gainBtnRunning = true;
            worldItem.StopItemRoutine();
            AlphaZeroOne(CurrentBtn);
            AddBag(CurrentBtn);
            DestroyOneAlphaZero(CurrentBtn);            
            worldItem.RemoveItemFromVariable(CurrentBtn);
            gainBtnRunning = false;
        }
        else
        {
            Debug.Log("얻기 대기!");

            while (gainBtnRunning == true)
            {

            }

            gainBtnRunning = true;
            worldItem.StopItemRoutine();
            AlphaZeroOne(CurrentBtn);
            AddBag(CurrentBtn);
            DestroyOneAlphaZero(CurrentBtn);                    
            worldItem.RemoveItemFromVariable(CurrentBtn);
            gainBtnRunning = false;
        }
    }

    void AlphaZeroOne(GameObject Item)
    {
            MoveOneCollection(Item);

            Item.GetComponent<CanvasGroup>().alpha = 0;
            Item.GetComponent<CanvasGroup>().interactable = false;
            Item.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void MoveOneCollection(GameObject Item)
    {
        GameObject ItemClone = Instantiate(Item, MainUIPanel.transform);
        ItemClone.transform.position = Item.transform.position;
        ItemClone.transform.localScale = Vector3.one;

        StartCoroutine(MovePoint(ItemClone));
    }

    void DestroyOneAlphaZero(GameObject Item)
    {
        Destroy(Item);
    }

    public void AddBag(GameObject Item)
    {
        bag = new Dictionary<string, int>();
        bag = variables.LoadBag();
        string ItemName = Item.name.Substring(7);
        if (bag.ContainsKey(ItemName) == true)
        {
            bag[ItemName] += 1;
            int currentInkAmount = variables.LoadInk();
            currentInkAmount += 1;
            variables.SaveInk(currentInkAmount);
        }
        else
        {
            bag.Add(ItemName, 0);
            bag[ItemName] += 1;
        }
        variables.SaveBag(bag);
        Debug.Log("[가방]" + ItemName + ":" + bag[ItemName] + "개");  
    }

    public void AddBag(List<GameObject> ItemList)
    {
        bag = new Dictionary<string, int>();
        bag = variables.LoadBag();
        for (int i = 0; i < ItemList.Count; i++)
        {
            string ItemName = ItemList[i].name.Substring(7);
            if (bag.ContainsKey(ItemName) == true)
            {
                bag[ItemName] += 1;
                int currentInkAmount = variables.LoadInk();
                currentInkAmount += 1;
                variables.SaveInk(currentInkAmount);
            }
            else
            {
                bag.Add(ItemName, 0);
                bag[ItemName] += 1;
            }
            Debug.Log("[가방]" + ItemName + ":" + bag[ItemName] + "개");  
            variables.SaveBag(bag);
        }
    }
}
