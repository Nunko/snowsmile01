using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;
using BansheeGz.BGDatabase;

public class Main : MonoBehaviour
{    
    public GameObject bgImage;    
    public GameObject CoverPanel;
    public Dictionary<string,Sprite> ItemList;
    public List<Sprite> BgListM;
    public List<Sprite> BgListS;
    public AudioClip BGM;
    WindEffect windEffect;
    public List<string> ColorStringListA;
    public List<string> ColorStringListB;
    public List<GameObject> MinigameImages;
    public List<GameObject> BingoBtnLayers;
    public bool isMasterMode;

    void Start()
    {
        windEffect = FindObjectOfType<WindEffect>();
        isMasterMode = false;
        StartCoroutine("MainCoroutine");     
    }

    IEnumerator MainCoroutine()
    {
        NaninovelInitalize();        
        yield return new WaitUntil(() => Engine.Initialized == true);
        SetVariables();
        SetUIElements();        
        OpenMain(); 
        windEffect.RunWindEffect();        
    }

    async void NaninovelInitalize()
    {
        await RuntimeInitializer.InitializeAsync();
        Debug.Log("Naninovel Initialize");
    }

    void SetVariables()
    {
        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();  
        variableManager.ResetGlobalVariables();
    }

    void SetUIElements()
    {
        FillItemsList();
        FillBgsList();
        SetBgImage();  
        LoadColorList();       
        ChangeBtnColor(MinigameImages);
        ChangeBtnColor(BingoBtnLayers); 
        SetBGMOrRunPrologue();        
    }

    void FillItemsList()
    {        
        ItemList = new Dictionary<string, Sprite>();        
        List<BGEntity> Items = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a"));
        for(int i = 0; i < Items.Count; i++)
        {
            string itemName = Items[i].Name;
            int itemImageNameLength = itemName.Length;
            string itemImageName0 = itemName[0].ToString().ToLower();
            string itemImageName1 = itemName[itemImageNameLength - 3].ToString();
            string itemImageName2 = itemName[itemImageNameLength - 2].ToString();
            string itemImageName3 = itemName[itemImageNameLength - 1].ToString();
            string itemImageName = itemImageName0 + itemImageName1 + itemImageName2 + itemImageName3;
            ItemList.Add(itemName, Resources.Load<Sprite>("Sprites/Item/"+itemImageName));            
        }
        Debug.Log("불러온 아이템 스프라이트 수: " + ItemList.Count);
    }

    void FillBgsList()
    {
        BgListM = new List<Sprite>();        
        BgListS = new List<Sprite>(); 
        for (int i = 1; ; i++)
        {
            string BgNumber = "00";
            if (i < 10)
            {
                BgNumber = "0" + i;
            }
            else
            {
                BgNumber = i.ToString();
            }            
            string BgName = "BG" + BgNumber;
            Sprite BgSpriteM = Resources.Load<Sprite>("Sprites/Bg/M/"+BgName+"M");
            Sprite BgSpriteS = Resources.Load<Sprite>("Sprites/Bg/S/"+BgName+"S");
            if (BgSpriteM == null)
            {
                break;
            }
            BgListM.Add(BgSpriteM);       
            BgListS.Add(BgSpriteS);       
        }
        Debug.Log("불러온 배경 스프라이트 수: " + BgListM.Count);
    }

    void SetBgImage()
    {        
        if (ES3.KeyExists("readDialogueList") == true)
        {
            int randNumber = Random.Range(1, BgListM.Count + 1);   
            string BgNumber = "00";
            if (randNumber < 10)
            {
                BgNumber = "0" + randNumber;
            }
            else
            {
                BgNumber = randNumber.ToString();
            }            
            string BgName = "BG" + BgNumber;            
            bgImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Bg/"+BgName);

            Dictionary<string, int> outcome = ES3.Load<Dictionary<string, int>>("outcome");
            outcome["BG"] = randNumber;
            ES3.Save("outcome", outcome);
            Debug.Log("미니게임 값 저장소: BG " + outcome["BG"] + "번째");
        }
        else
        {
            bgImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Bg/"+"BG04");

            if(ES3.KeyExists("outcome") == false)
            {
                Dictionary<string, int> outcome = new Dictionary<string, int>();
                outcome.Add("P", 0);
                outcome.Add("A", 0);
                outcome.Add("M", 0);
                outcome.Add("new", 0);
                outcome.Add("BG", 4);
                ES3.Save("outcome", outcome);
                Debug.Log("미니게임 값 저장소 생성");
                Debug.Log("미니게임 값 저장소: BG " + outcome["BG"] + "번째");
            }
        }        
    }

    async void SetBGMOrRunPrologue()
    {
        if (ES3.KeyExists("readDialogueList") == true)
        {
            var switchCommand = new SwitchToNovelMode { ScriptName = "Main" };
            await switchCommand.ExecuteAsync();    
        }
        else
        {
            string dialogueCode = "FirstRound0";
            ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
            variableManager.SetVariableValue("g_DialogueCode", dialogueCode);

            var switchCommand = new SwitchToNovelMode { ScriptName = "FirstRound" };
            await switchCommand.ExecuteAsync();  
        }
           
    }

    void OpenMain()
    {
        CoverPanel.GetComponent<CanvasGroup>().alpha = 0;
        CoverPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Debug.Log("메인 커버 열림");
    }

    void LoadColorList()
    {
        ColorStringListA = new List<string>();
        ColorStringListB = new List<string>();

        List<BGEntity> ColorTable = BGRepo.I["ColorList"].EntitiesToList();
        for (int i = 0; i < ColorTable.Count; i++)
        {
            var tempColor = ColorTable[i];
            ColorStringListA.Add(tempColor.Get<string>("colorA"));
            ColorStringListB.Add(tempColor.Get<string>("colorB"));
        }
    }

    void ChangeBtnColor(List<GameObject> BtnLayerList)
    {
        Dictionary<string, int> outcome = ES3.Load<Dictionary<string, int>>("outcome");
        int bgNumber = outcome["BG"] - 1;

        List<int> colorAToInt = new List<int>() {0, 0, 0};
        List<int> colorBToInt = new List<int>() {0, 0, 0};
        
        for (int i = 0; i < colorAToInt.Count; i++)
        {            
            string colorStringTmp = ColorStringListA[bgNumber][(2*i + 1)].ToString() + ColorStringListA[bgNumber][(2*i + 2)].ToString();            
            int numberTmp = int.Parse(colorStringTmp, System.Globalization.NumberStyles.HexNumber);
            colorAToInt[i] = numberTmp;
        }
        for (int i = 0; i < colorBToInt.Count; i++)
        {
            string colorStringTmp = ColorStringListB[bgNumber][(2*i + 1)].ToString() + ColorStringListB[bgNumber][(2*i + 2)].ToString();
            int numberTmp = int.Parse(colorStringTmp, System.Globalization.NumberStyles.HexNumber);
            colorBToInt[i] = numberTmp;
        }

        List<int> firstNumber = new List<int>() {0, 0, 0};
        List<int> secondNumber = new List<int>() {0, 0, 0};
        for (int i = 0; i < firstNumber.Count; i++)
        {
            firstNumber[i] = colorAToInt[i];
            secondNumber[i] = colorBToInt[i] - colorAToInt[i];
        }

        int underNumber = BtnLayerList.Count - 1;
        for (int i = 0; i < BtnLayerList.Count; i++)
        {                        
           BtnLayerList[i].GetComponent<Image>().color = new Color((float)(firstNumber[0] + secondNumber[0]*i/underNumber)/255, (float)(firstNumber[1] + secondNumber[1]*i/underNumber)/255, (float)(firstNumber[2] + secondNumber[2]*i/underNumber)/255, 1);
        }
    }

    public void ClickChangeBgImage()
    {
        Dictionary<string, int> outcome = ES3.Load<Dictionary<string, int>>("outcome");
        int currentNumber = outcome["BG"];
        int randNumber = 0;
        if (isMasterMode == true)
        {
            if (currentNumber == BgListM.Count)
            {
                randNumber = 1;
            }
            else
            {
                randNumber = currentNumber + 1;
            }
        }
        else
        {
            do
            {
                randNumber = Random.Range(1, BgListM.Count + 1);
            } while (currentNumber == randNumber);           
        }         
        
        string BgNumber = "00";
        if (randNumber < 10)
        {
            BgNumber = "0" + randNumber;
        }
        else
        {
            BgNumber = randNumber.ToString();
        }            
        string BgName = "BG" + BgNumber;            
        bgImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Bg/"+BgName);
                        
        outcome["BG"] = randNumber;
        ES3.Save("outcome", outcome);
        Debug.Log("미니게임 값 저장소: BG " + outcome["BG"] + "번째");

        ChangeBtnColor(MinigameImages);
        ChangeBtnColor(BingoBtnLayers); 
    }
}
