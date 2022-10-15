using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;
using BansheeGz.BGDatabase;

public class Tutorial : MonoBehaviour
{
    Main main;   
    BtnUI btnUI;

    ICustomVariableManager variableManager;

    public Object TutorialEffect;

    public string tutorialCode;

    public GameObject InkBtn;
    public GameObject ShopBtn;
    public Object Item;
    GameObject ItemL;
    GameObject ItemR;
    public GameObject GroupGenItems;
    public GameObject MinigameBtn;
    public GameObject BingoBtn;
    public GameObject MenuBtn;
    public GameObject ConfigBtn;
    public GameObject ReplayStoryBtn;
    public GameObject BagBtn;
    public GameObject NINIBtn;
    public GameObject DNEEBtn;
    public GameObject GPBtn;
    public GameObject JNMBtn;
    public GameObject LZBBtn;
    public GameObject ULCSBtn;
    public GameObject StoryPlayBtn;
    public Object TutorialGridImageBoard;
    public List<Sprite> TutorialGridImages;

    List<string> genGameObjectNameList;
    List<GameObject> genGameObjectList;

    void OnEnable()
    {
        main = FindObjectOfType<Main>();
        btnUI = FindObjectOfType<BtnUI>();
        variableManager = Engine.GetService<ICustomVariableManager>();  
        variableManager.SetVariableValue("g_TutorialCode","");      
        tutorialCode = "";
        genGameObjectNameList = new List<string>(); 
        genGameObjectList = new List<GameObject>();
    }

    void OnDisable() 
    {              
        for (int i = 0; i < genGameObjectList.Count; i++)
        {
            Destroy(genGameObjectList[i]);
        }       
    }

    void Update()
    {
        string naniTutorialCode = variableManager.GetVariableValue("g_TutorialCode");

        if (tutorialCode != naniTutorialCode)
        {
            tutorialCode = naniTutorialCode;
            RunTutorial();
        }        
    }

    void RunTutorial()
    {
        if (tutorialCode[1] == '1')
        {
            switch (tutorialCode)
            {
                case "S1-01" : 
                EffectOn(InkBtn); //Ink 이펙트 켜기
                break;
                
                case "S1-02" : 
                btnUI.ClickInkPanel(); //Ink 버튼 누르기
                break;
                
                case "S1-03" : 
                EffectOff(InkBtn); //Ink 이펙트 끄기
                break;
                
                case "S1-04" : 
                GenItems();
                break;
                                
                case "S1-05" : 
                HideItems();
                break;
                
                case "S1-06" : 
                EffectOn(MinigameBtn); //Minigame 이펙트 켜기
                //Minigame 내부 이펙트 재생
                break;
                
                case "S1-07" : 
                //Minigame 내부 이펙트 정지
                EffectOff(MinigameBtn); //Minigame 이펙트 끄기
                break;
                

                case "S1-08" : 
                EffectOn(BingoBtn); //Bingo 이펙트 켜기
                //Bingo 내부 이펙트 재생
                break;
                
                case "S1-09" : 
                //Bingo 내부 이펙트 정지
                EffectOff(BingoBtn); //Bingo 이펙트 끄기
                break;
                
                case "S1-10" : 
                EffectOn(MenuBtn); //Menu 이펙트 켜기
                break;
                
                case "S1-11" : 
                btnUI.ClickFordedMenuBtn(); //Menu 버튼 누르기
                EffectOff(MenuBtn);
                EffectOn(MenuBtn);
                EffectOn(ConfigBtn); //Config 이펙트 켜기
                EffectOn(ReplayStoryBtn); //Story 이펙트 켜기
                EffectOn(BagBtn); //Bag 이펙트 켜기
                break;
                

                case "S1-12" : 
                EffectOff(MenuBtn); //Menu 이펙트 끄기
                EffectOff(ReplayStoryBtn); //Story 이펙트 끄기
                EffectOff(BagBtn); //Bag 이펙트 끄기
                break;
                
                case "S1-13" : 
                EffectOff(ConfigBtn); //Config 이펙트 끄기
                EffectOn(ReplayStoryBtn); //Story 이펙트 켜기
                break;
                
                case "S1-14" : 
                EffectOff(ReplayStoryBtn); //Story 이펙트 끄기
                EffectOn(BagBtn); //Bag 이펙트 켜기
                break;
                
                case "S1-15" : 
                EffectOff(BagBtn); //Bag 이펙트 끄기
                break;

                case "S1-16" :
                btnUI.ClickFordedMenuBtn(); //Menu 버튼 누르기
                break;

                case "S1-17" :
                EffectOn(ShopBtn);
                break;

                case "S1-18" :
                EffectOff(ShopBtn);
                break;

                case "S1-19" : 
                btnUI.ClickInkPanel(); //Ink 버튼 한번 더 누르기
                break;

                default:
                break;
            }        
        }
        else if (tutorialCode[1] == '2')
        {
            switch (tutorialCode)
            {
                case "S2-01" : 
                EffectOn(NINIBtn);
                break;

                case "S2-02" : 
                EffectOff(NINIBtn);
                break;

                case "S2-03" : 
                EffectOn(DNEEBtn);
                break;

                case "S2-04" : 
                EffectOff(DNEEBtn);
                break;

                case "S2-05" : 
                EffectOn(GPBtn);
                break;

                case "S2-06" : 
                EffectOff(GPBtn);
                break;

                case "S2-07" : 
                EffectOn(JNMBtn);
                break;

                case "S2-08" : 
                EffectOff(JNMBtn);
                break;

                case "S2-09" : 
                EffectOn(LZBBtn);
                break;

                case "S2-10" : 
                EffectOff(LZBBtn);
                break;

                case "S2-11" : 
                EffectOn(ULCSBtn);
                break;

                case "S2-12" : 
                EffectOff(ULCSBtn);
                break;

                default:
                break;
            }
        }
        else if (tutorialCode[1] == '3')
        {
            switch (tutorialCode)
            {
                case "S3-01" : 
                GenTutorialGridImageBoard();
                ChangeTutorialGridImage(0);
                break;

                case "S3-02" : 
                ChangeTutorialGridImage(1);
                break;

                case "S3-03" : 
                ChangeTutorialGridImage(0);
                break;

                case "S3-04" : 
                ChangeTutorialGridImage(2);                              
                break;

                case "S3-05" : 
                ChangeTutorialGridImage(3);            
                break;

                case "S3-06" : 
                EffectOn(StoryPlayBtn);
                break;

                case "S3-07" : 
                EffectOff(StoryPlayBtn);
                HideTutorialGridImageBoard();
                break;

                default:
                break;
            }
        }        
    }

    void EffectOn(GameObject inputGameObject)
    {        
        if (genGameObjectNameList.Contains(inputGameObject.name) == false)
        {
            GameObject particle = spawnParticle(TutorialEffect, inputGameObject);        
            particle.name = "TutorialEffect" + inputGameObject.name;
            genGameObjectNameList.Add(inputGameObject.name);
            genGameObjectList.Add(particle);
            particle.GetComponent<ParticleSystem>().Play(true);
        }
        else
        {
            GameObject particle = GameObject.Find("TutorialEffect" + inputGameObject.name);
            particle.GetComponent<ParticleSystem>().Play(true);
        }
    }

    void EffectOff(GameObject inputGameObject)
    {
        GameObject particle = GameObject.Find("TutorialEffect" + inputGameObject.name);
        particle.GetComponent<ParticleSystem>().Stop(true);
    }

    GameObject spawnParticle(Object inputPaticles, GameObject inputGameObject)
    {
        GameObject particles = (GameObject)Instantiate(inputPaticles, inputGameObject.transform);
		#if UNITY_3_5
			particles.SetActiveRecursively(true);
		#else
			particles.SetActive(true);
			for(int i = 0; i < particles.transform.childCount; i++)
				particles.transform.GetChild(i).gameObject.SetActive(true);
		#endif
		
		return particles;
    }

    void GenItems()
    {
        List<Vector3> siteList = new List<Vector3>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int posX = 90 + 156 * i - 455;
                int posY = 90 + 156 * j - 377;
                Vector3 site = new Vector3(posX, posY, 0);
                siteList.Add(site);
            }
        }

        Vector3 siteL = siteList[4];
        Vector3 siteR = siteList[24];

        float randXNumberL = Random.Range(-40, 40);
        float randYNumberL = Random.Range(-40, 40);
        float randXNumberR = Random.Range(-40, 40);
        float randYNumberR = Random.Range(-40, 40);

        Vector3 siteAddedRandL = new Vector3(siteL.x + randXNumberL, siteL.y + randYNumberL, siteL.z);
        Vector3 siteAddedRandR = new Vector3(siteR.x + randXNumberR, siteR.y + randYNumberR, siteR.z);

        GameObject ItemLTmp = Instantiate(Item, GroupGenItems.transform) as GameObject;
        ItemL = ItemLTmp;
        genGameObjectList.Add(ItemL);
        ItemL.transform.localPosition = siteAddedRandL;
        ItemL.transform.localScale = Vector3.one; 

        GameObject ItemRTmp = Instantiate(Item, GroupGenItems.transform) as GameObject;
        ItemR = ItemRTmp;
        genGameObjectList.Add(ItemR);
        ItemR.transform.localPosition = siteAddedRandR;
        ItemR.transform.localScale = Vector3.one; 

        List<BGEntity> Items = BGRepo.I["Item"].FindEntities(entity => entity.Name.Contains("a"));
        int randNumberL = Random.Range(0, Items.Count);
        int randNumberR = Random.Range(0, Items.Count);
        string randItemNameL = Items[randNumberL].Name; 
        string randItemNameR = Items[randNumberR].Name; 

        ItemL.transform.Find("ImageM").GetComponent<Image>().sprite = main.ItemList[randItemNameL];
        ItemR.transform.Find("ImageM").GetComponent<Image>().sprite = main.ItemList[randItemNameR];

        ItemL.name = "TutorialItemL";
        ItemR.name = "TutorialItemR";

        ItemL.GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0 , Random.Range(0, 360));
        ItemR.GetComponent<RectTransform>().eulerAngles = new Vector3 (0, 0 , Random.Range(0, 360));
    }

    void HideItems()
    {
        ItemL.GetComponent<CanvasGroup>().alpha = 0;
        ItemR.GetComponent<CanvasGroup>().alpha = 0;
    }

    void GenTutorialGridImageBoard()
    {
        GameObject TutorialGridImagePanel = Instantiate(TutorialGridImageBoard, gameObject.transform) as GameObject;
        TutorialGridImagePanel.name = "TutorialGridImagePanel";
        genGameObjectList.Add(TutorialGridImagePanel);
    }

    void ChangeTutorialGridImage(int index)
    {
        GameObject TutorialGridImagePanel = GameObject.Find("TutorialGridImagePanel");
        Image TutorialImage = TutorialGridImagePanel.transform.Find("TutorialImage").GetComponent<Image>();
        TutorialImage.color = new Color(1, 1, 1, 1);
        TutorialImage.sprite = TutorialGridImages[index];
    }

    void HideTutorialGridImageBoard()
    {
        GameObject TutorialGridImagePanel = GameObject.Find("TutorialGridImagePanel");
        TutorialGridImagePanel.SetActive(false);
    }
}
