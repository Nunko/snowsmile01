using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public Object windEffect;

    public GameObject BingoBtn;
    public GameObject BingoPanel;
    public GameObject NINIBtn;
    public GameObject DNEEBtn;
    public GameObject GPBtn;
    public GameObject JNMBtn;
    public GameObject LZBBtn;
    public GameObject ULCSBtn;
    public GameObject ENDBtn;

    Dictionary<string, int> bag;
    List<string> WindEffectList;

    Variables variables;

    void Start()
    {
        bag = new Dictionary<string, int>();
        WindEffectList = new List<string>();
        variables = FindObjectOfType<Variables>();
        SetRule();        
    }

    public void RunWindEffect()
    {        
        bag = variables.LoadBag();

        bool isOK = false;

        StopWindEffect();

        if (isMain() == true)
        {
            isOK = CheckWindEffectForMain(); 
            if (isOK == true)
            {                
                GenWindEffect(BingoBtn);
            }           
        }
        else
        {
            List<GameObject> inputGameObject = new List<GameObject>() {NINIBtn, DNEEBtn, GPBtn, JNMBtn, LZBBtn, ULCSBtn, ENDBtn};
            for (int i = 0; i < inputGameObject.Count; i++)
            {
                string characterCode = "";
                switch (i)
                {
                    case 0: characterCode = "NINI"; break;
                    case 1: characterCode = "DNEE"; break;
                    case 2: characterCode = "GP"; break;
                    case 3: characterCode = "JNM"; break;
                    case 4: characterCode = "LZB"; break;
                    case 5: characterCode = "ULCS"; break;
                    default: characterCode = "END"; break;
                }

                if (characterCode == "END")
                {
                    isOK = CheckWindEffectForEnding();
                }
                else
                {
                    isOK = CheckWindEffectForBingo(characterCode);
                }
                
                if (isOK == true)
                {
                    GenWindEffect(inputGameObject[i]);
                }   
            }
        }
    }

    public void StopWindEffect()
    {
        if (isMain() == true)
        {
            GameObject inputGameObject = BingoBtn;
            if (WindEffectList.Contains(inputGameObject.name) == true)
            {
                inputGameObject.transform.Find("WindEffectA").GetComponent<ParticleSystem>().Stop(true);
                inputGameObject.transform.Find("WindEffectB").GetComponent<ParticleSystem>().Stop(true);
            }            
        }
        else
        {
            List<GameObject> inputGameObject = new List<GameObject>() {NINIBtn, DNEEBtn, GPBtn, JNMBtn, LZBBtn, ULCSBtn, ENDBtn};
            for (int i = 0; i < inputGameObject.Count; i++)
            {
                if (WindEffectList.Contains(inputGameObject[i].name) == true)
                {
                    inputGameObject[i].transform.Find("WindEffectA").GetComponent<ParticleSystem>().Stop(true);
                    inputGameObject[i].transform.Find("WindEffectB").GetComponent<ParticleSystem>().Stop(true);
                }
            }
        }
    }

    bool isMain()
    {
        if (BingoPanel.activeSelf == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool CheckWindEffectForMain()
    {
        if (ES3.KeyExists("readDialogueList") == false)
        {
            Debug.Log("ES3.KeyExists(readDialogueList) == false");
            return false;
        }
        else
        {
            Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
            readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

            List<string> charaList = new List<string>() {"NINI", "DNEE", "GP", "JNM", "LZB", "ULCS", "END"};

            for (int i = 0; i < charaList.Count; i++)
            {
                Debug.Log("바람 생성 조건 확인 시작 [" + charaList[i] + "]");
                int numb = readDialogueList[charaList[i]];
                if (charaList[i] != "END")
                {
                    if (numb == -1) {return true;}
                    else if (numb > 0 && numb < 6)
                    {
                        List<string> ItemsListByCharac = new List<string>();
                        ItemsListByCharac = ES3.Load<List<string>>("gridItemListBy" + charaList[i]);  

                        if (i > 0)
                        {
                            List<string> keyList = FindKeyItems(charaList[i]);
                            string keyName = keyList[numb - 1];
                            if (bag.ContainsKey(keyName) == true)
                            {
                                int keyNumb = ItemsListByCharac.IndexOf(keyName);
                                List<int> requiredGridList = new List<int>();
                                
                                for (int j = 0; j < JudgmentAll.Count; j++)
                                {
                                    if (JudgmentAll[j].Contains(keyNumb) == true)
                                    {
                                        for (int k = 0; k < JudgmentAll[j].Count; k++)
                                        {
                                            requiredGridList.Add(JudgmentAll[j][k]);
                                        }

                                        List<string> requiredItemList = new List<string>();
                                        for (int k = 0; k < requiredGridList.Count; k++)
                                        {
                                            requiredItemList.Add(ItemsListByCharac[requiredGridList[k]]);
                                        }

                                        for (int k = 0; k < requiredItemList.Count; k++)
                                        {
                                            if (bag.ContainsKey(requiredItemList[k]) == false) {break;}
                                            else
                                            {
                                                if (k == requiredItemList.Count - 1) {return true;}
                                            }
                                        }
                                    }
                                }                            
                            }
                        }
                        else
                        {
                            int count = 0;

                            for (int j = 0; j < JudgmentAll.Count; j++)
                            {                            
                                for (int k = 0; k < JudgmentAll[j].Count; k++)
                                {
                                    if (bag.ContainsKey(ItemsListByCharac[JudgmentAll[j][k]]) == false) {break;}
                                    else
                                    {
                                        if (k == JudgmentAll[j].Count - 1) {count++;}
                                    }
                                }                            
                            }

                            if (numb <= count) {return true;}
                        }                    
                    }
                    else if (numb == 6) {return true;}                        
                }
                else
                {
                    return CheckWindEffectForEnding();
                }
            }        

            return false;          
        }
    }

    bool CheckWindEffectForBingo(string characterCode)
    {
        Debug.Log("바람 생성 조건 확인 시작 [" + characterCode + "]");

        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        int numb = readDialogueList[characterCode];
        if (numb == -1)
        {
            return true;
        }
        else if (numb > 0 && numb < 6)
        {
            List<string> ItemsListByCharac = new List<string>();
            ItemsListByCharac = ES3.Load<List<string>>("gridItemListBy" + characterCode);  

            if (characterCode != "NINI")
            {
                List<string> keyList = FindKeyItems(characterCode);
                string keyName = keyList[numb - 1];
                if (bag.ContainsKey(keyName) == true)
                {
                    int keyNumb = ItemsListByCharac.IndexOf(keyName);
                    List<int> requiredGridList = new List<int>();
                    
                    for (int j = 0; j < JudgmentAll.Count; j++)
                    {
                        if (JudgmentAll[j].Contains(keyNumb) == true)
                        {
                            for (int k = 0; k < JudgmentAll[j].Count; k++)
                            {
                                requiredGridList.Add(JudgmentAll[j][k]);
                            }

                            List<string> requiredItemList = new List<string>();
                            for (int k = 0; k < requiredGridList.Count; k++)
                            {
                                requiredItemList.Add(ItemsListByCharac[requiredGridList[k]]);
                            }

                            for (int k = 0; k < requiredItemList.Count; k++)
                            {
                                if (bag.ContainsKey(requiredItemList[k]) == false)
                                {
                                    break;
                                }
                                else
                                {
                                    if (k == requiredItemList.Count - 1)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }                            
                }
            }
            else
            {
                int count = 0;

                for (int j = 0; j < JudgmentAll.Count; j++)
                {
                    
                    for (int k = 0; k < JudgmentAll[j].Count; k++)
                    {
                        if (bag.ContainsKey(ItemsListByCharac[JudgmentAll[j][k]]) == false)
                        {
                            break;
                        }
                        else
                        {
                            if (k == JudgmentAll[j].Count - 1)
                            {
                                count++;
                            }
                        }
                    }                    
                }

                if (numb <= count)
                {
                    return true;
                }
            }            
        }
        else if (numb == 6)
        {
            return true;
        }
        return false;
    }

    void GenWindEffect(GameObject inputGameObject)
    {        
        if (WindEffectList.Contains(inputGameObject.name) == false)
        {
            GameObject particleA = spawnParticle(windEffect, inputGameObject, "A");
            particleA.name = "WindEffectA";     
            GameObject particleB = spawnParticle(windEffect, inputGameObject, "B");
            particleB.name = "WindEffectB";   
            WindEffectList.Add(inputGameObject.name);

            Debug.Log("바람 생성! AB " + inputGameObject.name);            
        }
        else
        {
            inputGameObject.transform.Find("WindEffectA").GetComponent<ParticleSystem>().Play(true);
            inputGameObject.transform.Find("WindEffectB").GetComponent<ParticleSystem>().Play(true);
            Debug.Log("바람 재생! " + inputGameObject.name);
        }        
    }

    List<string> FindKeyItems(string characterCode)
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

    GameObject spawnParticle(Object inputPaticles, GameObject inputGameObject, string ab)
    {        
        GameObject particles = (GameObject)Instantiate(inputPaticles, inputGameObject.transform);
        RectTransform rt = inputGameObject.GetComponent<RectTransform>();
        int width = (int)rt.sizeDelta.x;
        int height = (int)rt.sizeDelta.y;
        if (rt.pivot.x == 1 && rt.pivot.y == 0)
        {
            particles.GetComponent<RectTransform>().localPosition = new Vector3(-width, height/2, 0);
        }
        else if (rt.pivot.x == 0 && rt.pivot.y == 0.5)
        {
            particles.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }
        else if (rt.pivot.x == 1 && rt.pivot.y == 0.5)
        {
            particles.GetComponent<RectTransform>().localPosition = new Vector3(-width, 0, 0);
        }
        else
        {
            particles.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        }       

        string windType = ab;
        if (ab == "B")
        {
            particles.GetComponent<RectTransform>().eulerAngles = new Vector3(90, 0, 0);
        } 
		
		#if UNITY_3_5
			particles.SetActiveRecursively(true);
		#else
			particles.SetActive(true);
			for(int i = 0; i < particles.transform.childCount; i++)
				particles.transform.GetChild(i).gameObject.SetActive(true);
		#endif
		
		return particles;
    }

    bool CheckWindEffectForEnding()
    {
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        if (readDialogueList["NINI"] == 7 && readDialogueList["DNEE"] == 7 && readDialogueList["GP"] == 7 && readDialogueList["LZB"] == 7 && readDialogueList["ULCS"] == 7 && readDialogueList["END"] < 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
