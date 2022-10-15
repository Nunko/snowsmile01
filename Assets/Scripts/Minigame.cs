using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;
using BansheeGz.BGDatabase;

public class Minigame : MonoBehaviour
{
    public GameObject GuidePanel;
    public GameObject ShowGuidePanelBtn;
    public GameObject PopUpPanel;
    public List<string> ColorStringListA;
    public List<string> ColorStringListB;
    public List<GameObject> Colors;    

    void Awake()
    {
        Engine.Destroy();
    }

    void Start()
    {        
        if (ES3.KeyExists("MinigameBestScore") == true)
		{
            CloseGuidePanel();
        }
        else
        {
            Time.timeScale = 0;
        }
        
        LoadColorList();
        SetColor();        
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape) == true)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (PopUpPanel.activeSelf == false)
                {                    
                    if (GuidePanel.activeSelf == false)
                    {
                        gameManager.pauseGame();
                    }
                    else
                    {
                        ClickGuidePanel();
                    }
                }
                else
                {
                    if (GuidePanel.activeSelf == false)
                    {
                        gameManager.continueGame();
                    }
                    else
                    {
                        ClickGuidePanel();
                    }                    
                }
            }
        }
    }

    void SetColor()
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

        int underNumber = Colors.Count - 1;
        for (int i = 0; i < Colors.Count; i++)
        {                        
           Colors[i].GetComponent<SpriteRenderer>().color = new Color((float)(firstNumber[0] + secondNumber[0]*i/underNumber)/255, (float)(firstNumber[1] + secondNumber[1]*i/underNumber)/255, (float)(firstNumber[2] + secondNumber[2]*i/underNumber)/255, 1);
        }
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

    public void ClickGuidePanel()
    {
        if (PopUpPanel.activeSelf != true)
        {
            GuidePanel.GetComponent<CanvasGroup>().alpha = 0;
            Invoke ("CloseGuidePanel", 0.3f);  
            Time.timeScale = 1;
        } 
        else
        {
            CloseGuidePanel();
        } 
    }

    public void CloseGuidePanel()
    {
        GuidePanel.SetActive(false);
        Button GuidePanelBackBtn = GuidePanel.GetComponent<Button>();
        if (GuidePanelBackBtn.interactable == false)
        {
            GuidePanelBackBtn.interactable = true;
        }
        CanvasGroup ShowGuidePanelBtnCG = ShowGuidePanelBtn.GetComponent<CanvasGroup>();
        if (ShowGuidePanelBtnCG.alpha == 0)
        {
            ShowGuidePanelBtnCG.blocksRaycasts = true;
            ShowGuidePanelBtnCG.alpha = 1;
            ShowGuidePanelBtnCG.interactable = true;
        }        
    }

    public void ClickShowGuidePanelBtn()
    {
        CanvasGroup ShowGuidePanelBtnCG = ShowGuidePanelBtn.GetComponent<CanvasGroup>();
        ShowGuidePanelBtnCG.interactable = false;
        ShowGuidePanelBtnCG.alpha = 0;
        ShowGuidePanelBtnCG.blocksRaycasts = false;

        GuidePanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        GuidePanel.GetComponent<CanvasGroup>().alpha = 1;
        GuidePanel.SetActive(true);
    }
}
