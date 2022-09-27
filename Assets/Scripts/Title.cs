using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    public GameObject PopUpPanel;
    public GameObject VersionText;

    void Start()
    {
        Debug.Log("게임 실행");
        Input.multiTouchEnabled = false;
        VersionText.GetComponent<TextMeshProUGUI>().text = Application.version;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape) == true && SplashScreen.isFinished == true)
            {
                if (PopUpPanel.activeSelf == false)
                {
                    ShowPopUpPanel();
                }
                else
                {
                    HidePopUpPanel();
                }
            }
        }
    }

    public void ClickTitleScreen()
    {
        Debug.Log("메인 씬으로 진입 시도");
        StartCoroutine(LoadAsyncScene("Main"));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }    

    void ShowPopUpPanel()
    {
        Time.timeScale = 0;
        PopUpPanel.SetActive(true);
    }

    public void ClickQuitBtn()
    {
        Application.Quit();
    }

    public void HidePopUpPanel()
    {      
        Invoke ("ActiveFalsePopUpPanel", 0.3f);       
    }

    void ActiveFalsePopUpPanel()
    {
        PopUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
