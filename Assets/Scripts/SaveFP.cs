using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Naninovel;

public class SaveFP : MonoBehaviour
{
    public GameObject CoverPanel;
    string dialogueCode;

    void OnEnable()
    {
        ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();        
        string dialogueCode = variableManager.GetVariableValue("g_DialogueCode");
        string mode = variableManager.GetVariableValue("g_Mode");
        string characterCode = "";
        if (dialogueCode.Length != 11 && dialogueCode.Length > 10)
        {
            switch (dialogueCode[11])
            {
                    case 'D': characterCode = "DNEE";
                            break;
                    case 'G': characterCode = "GP";
                            break;
                    case 'J': characterCode = "JNM";
                            break;
                    case 'L': characterCode = "LZB";
                            break;
                    case 'U': characterCode = "ULCS";
                            break;                    
            }
        }
        else if (dialogueCode.Length != 11 && dialogueCode.Length < 10 && dialogueCode.Length > 0)
        {
            switch (dialogueCode[0])
            {
                case 'T': characterCode = "Tutorial";
                        break;
                default: characterCode = "END";
                        break;
            }            
        }
        else
        {
            characterCode = "NINI";
        }

        if (characterCode != "NINI" && characterCode != "Tutorial" && characterCode != "END" && mode == "Normal" && SaveEnable(characterCode, dialogueCode) == true)
        {
            int scoreTmp = int.Parse(variableManager.GetVariableValue("g_FP"));
            string name = "FP" + characterCode;

            if (dialogueCode[dialogueCode.Length - 1] == '1')
            {
                ES3.Save<int>(name, scoreTmp);
            }
            else
            {
                int score = ES3.Load<int>(name);    
                int scoreTotal = score + scoreTmp;
                if (scoreTotal > 10)
                {
                    scoreTotal = 10;
                }
                else if (scoreTotal < 0)
                {
                    scoreTotal = 0;
                }
                ES3.Save<int>(name, scoreTotal);
            }        
            Debug.Log("FP저장: " + name + "-" + ES3.Load<int>(name) + "(" + scoreTmp + ")");            
            
            SaveReadDialogueList(characterCode, dialogueCode);

            BtnBingo btnBingo = FindObjectOfType<BtnBingo>();
            btnBingo.RefreshAfterReading();

            variableManager.SetVariableValue("g_DialogueCode", "");
            variableManager.SetVariableValue("g_FP", "0");
        }
        else if (characterCode == "NINI" && mode == "Normal" && SaveEnable(characterCode, dialogueCode) == true)
        {
            SaveReadDialogueList(characterCode, dialogueCode);

            BtnBingo btnBingo = FindObjectOfType<BtnBingo>();
            btnBingo.RefreshAfterReading();

            variableManager.SetVariableValue("g_DialogueCode", "");
        }
        else if (characterCode == "END" && mode == "Normal" && SaveEnable(characterCode, dialogueCode) == true)
        {
            SaveReadDialogueList(characterCode, dialogueCode);

            BtnEnding btnEnding = FindObjectOfType<BtnEnding>();
            btnEnding.CheckReadEnding();

            variableManager.SetVariableValue("g_DialogueCode", "");
            
            CheckGoTitle();
        }
        else if (characterCode == "Tutorial" && mode == "Normal" && SaveEnable(characterCode, dialogueCode) == true)
        {
            SaveReadDialogueList(characterCode, dialogueCode);

            variableManager.SetVariableValue("g_DialogueCode", "");
        }
    }

    void SaveReadDialogueList(string inputCharacterCode, string inputDialogueCode)
    {
        int inputDialogueNumber = inputDialogueCode[inputDialogueCode.Length - 1] - '0';

        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
        
        readDialogueList[inputCharacterCode] = inputDialogueNumber;
        ES3.Save<Dictionary<string, int>>("readDialogueList", readDialogueList);

        Debug.Log("본 대화가 새로 저장됨: " + inputCharacterCode + "-" + inputDialogueNumber);

        string readDateTime = System.DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분");
        Dictionary<int, string> readDialogueDateTimeList = new Dictionary<int, string>();
        if (ES3.KeyExists("readDialogueDateTimeList" + inputCharacterCode) == true)
        {
            readDialogueDateTimeList = ES3.Load<Dictionary<int, string>>("readDialogueDateTimeList" + inputCharacterCode);
        }        
        readDialogueDateTimeList.Add(inputDialogueNumber, readDateTime);
        ES3.Save<Dictionary<int, string>>("readDialogueDateTimeList" + inputCharacterCode, readDialogueDateTimeList);
        Debug.Log("readDialogueDateTimeList" + inputCharacterCode + " / " + inputDialogueNumber + " / " + readDateTime + " 기록됨!");

        if (inputCharacterCode == "Tutorial" && inputDialogueNumber == 0)
        {
            FindObjectOfType<WindEffect>().RunWindEffect();
        }
    }

    bool SaveEnable(string inputCharacterCode, string inputDialogueCode)
    {    
        if (ES3.KeyExists("readDialogueList") == true)
        {
            Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
            readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");
              
            int inputDialogueNumber = inputDialogueCode[inputDialogueCode.Length - 1] - '0';
            int savedDialogueNumber = readDialogueList[inputCharacterCode];
            if (inputDialogueNumber > savedDialogueNumber)
            {
                Debug.Log("저장 가능!");
                return true;
            }
            else
            {
                Debug.Log("저장 불가능!");                
                return false;
            }           
        }
        else
        {
            Dictionary<string, int> readDialogueList = new Dictionary<string, int>();            
            readDialogueList.Add("Tutorial", -1);
            readDialogueList.Add("NINI", -1);
            readDialogueList.Add("DNEE", -1);
            readDialogueList.Add("GP", -1);
            readDialogueList.Add("JNM", -1);
            readDialogueList.Add("LZB", -1);
            readDialogueList.Add("ULCS", -1);
            readDialogueList.Add("END", -1);
            ES3.Save<Dictionary<string, int>>("readDialogueList", readDialogueList); 
            Debug.Log("저장 가능!");
            return true;
        }      
    }

    void CheckGoTitle()
    {                
        Dictionary<string, int> readDialogueList = new Dictionary<string, int>();
        readDialogueList = ES3.Load<Dictionary<string, int>>("readDialogueList");

        if (readDialogueList["END"] == 8)
        {
            CoverPanel.GetComponent<CanvasGroup>().alpha = 1;
            
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = false;
            Engine.Destroy();
            StartCoroutine(LoadAsyncScene("Title"));
        }        
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}