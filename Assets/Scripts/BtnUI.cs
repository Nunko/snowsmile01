using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Naninovel;

public class BtnUI : MonoBehaviour
{
    Variables variables;    
    public GameObject CharacterImageForInkPanel;
    public GameObject BlockPanel;
    public GameObject ConfigBtn;
    public GameObject ReplayStoryBtn;
    public GameObject CollectionBtn;    
    public GameObject ConfigPanel;
    public List<Sprite> CharactersForInkPanel;
    public GameObject InkPanelImage;
    public GameObject InkImage;
    public GameObject ShopBtn;
    public GameObject ShopPanel;
    bool isVisibleCharacterImage;
    public bool isMasterMode;

    void Awake()
    {
        variables = FindObjectOfType<Variables>();
        isVisibleCharacterImage = true;
        AudioListener.volume = 1;
    }

    void Start()
    {
        ShowAndHideCharacterImageForInkPanel();
    }

    public void ClickInkPanel()
    {
        if (isVisibleCharacterImage == true)
        {
            isVisibleCharacterImage = false;            

            ShowAndHideCharacterImageForInkPanel();            
            RefreshInkAmount();
             
            InkImage.SetActive(true);

            ShopBtn.SetActive(true);
        }
        else
        {
            isVisibleCharacterImage = true;            

            ShopBtn.SetActive(false);

            InkImage.SetActive(false);

            ShowAndHideCharacterImageForInkPanel();
        }                   
    }

    public void RefreshInkAmount()
    {
        string inputText = "";
        int inkCount = variables.LoadInk();
        if (inkCount < 1000)
        {
            inputText = inkCount.ToString();
        }
        else if (inkCount >= 1000)
        {
            inputText = "999+";
        }
        else
        {
            inputText = "0";
        }
        InkImage.transform.GetComponentInChildren<TextMeshProUGUI>().text = inputText; 
    }

    public void ClickFordedMenuBtn()
    {
        if (CollectionBtn.activeSelf == true)
        {            
            ConfigBtn.SetActive(false);
            ReplayStoryBtn.SetActive(false);
            CollectionBtn.SetActive(false); 
            BlockPanel.SetActive(false);           
        }
        else
        {
            BlockPanel.SetActive(true);
            ConfigBtn.SetActive(true);
            ReplayStoryBtn.SetActive(true);
            CollectionBtn.SetActive(true);  
        }
    }

    public void ClickConfigBtn()
    {
        ClickFordedMenuBtn();
        ConfigPanel.SetActive(true);
    }

    public void ClickBackToMainBtn(GameObject inputGameObject)
    {
        inputGameObject.SetActive(false);
    }

    public void ClickMinigame()
    {                   
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = false;

        IAudioManager audioManager = Engine.GetService<IAudioManager>();
        AudioListener.volume = audioManager.BgmVolume;

        StartCoroutine(LoadAsyncScene("game"));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ShowAndHideCharacterImageForInkPanel()
    {
        if (ES3.KeyExists("readDialogueList") == true)
        {
            Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
            readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
            int readNumber = readDialogueList["END"];

            Image CharacterImageForInkPanelImage = CharacterImageForInkPanel.GetComponent<Image>();
            if (readNumber >= 2 && readNumber < 7 && CharacterImageForInkPanelImage != CharactersForInkPanel[2])
            {
                CharacterImageForInkPanelImage.sprite = CharactersForInkPanel[2]; 
            }
            else if (readNumber == 7 && CharacterImageForInkPanelImage != CharactersForInkPanel[8])
            {
                CharacterImageForInkPanelImage.sprite = CharactersForInkPanel[14]; 
            }
            else if (readNumber == 8 && CharacterImageForInkPanelImage != CharactersForInkPanel[8])
            {
                CharacterImageForInkPanelImage.sprite = CharactersForInkPanel[8]; 
            }
            else
            {
                if (isVisibleCharacterImage == true)
                {
                    int randNumber = Random.Range(0, CharactersForInkPanel.Count);
                    CharacterImageForInkPanelImage.sprite = CharactersForInkPanel[randNumber]; 
                }
            }  
        }
        else
        {
            Image CharacterImageForInkPanelImage = CharacterImageForInkPanel.GetComponent<Image>();
            int randNumber = Random.Range(0, CharactersForInkPanel.Count);
            CharacterImageForInkPanelImage.sprite = CharactersForInkPanel[randNumber]; 
        }     
    }

    public void ClickShopBtn()
    {
        ShopPanel.SetActive(true);
        Debug.Log("상점 열림");        
    }  

    public void ClickTestBtn()
    {
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();            
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
        readDialogueList["NINI"] = 7;
        readDialogueList["DNEE"] = 7;
        readDialogueList["GP"] = 7; 
        readDialogueList["JNM"] = 7; 
        readDialogueList["LZB"] = 7;
        readDialogueList["ULCS"] = 7;
        readDialogueList["END"] = 7;
        ES3.Save<Dictionary<string, int>>("readDialogueList", readDialogueList); 
    }
}
