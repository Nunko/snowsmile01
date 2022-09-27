using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Naninovel;

public class BackIcons : MonoBehaviour
{
    public List<GameObject> Icons;
    public GameObject UICanvas;
    public float time;
    List<string> CoList;

    void Awake()
    {
        CoList = new List<string>();
    }

    void Update()
    {        
        if (CoList.Count == 0 && UICanvas.GetComponent<CanvasGroup>().alpha == 0)
        {
            StartCoroutine("MoveIcon");
        }                
    }

    IEnumerator MoveIcon()
    {
        CoList.Add("MoveIcon");
        int i = 0;
        float movePoint = 5.6f;

        yield return new WaitForSeconds(1f);        

        while (UICanvas.GetComponent<CanvasGroup>().alpha == 0)
        {                        
            if (Engine.Initialized == true)
            {
                IScriptPlayer scriptPlayer = Engine.GetService<IScriptPlayer>();
                ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();

                if (scriptPlayer.Playing == true)
                {                
                    for (int j = 0; j < Icons.Count; j++)
                    {
                        Icons[j].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                    yield return new WaitUntil(() => variableManager.GetVariableValue("g_DialogueCode") == ""); 
                    break;                           
                }
            }
            float posX = Icons[i].GetComponent<RectTransform>().localPosition.x;
            Icons[i].GetComponent<RectTransform>().localPosition = new Vector3 (posX, 0, 0);
            Icons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(time);
            Icons[i].GetComponent<RectTransform>().localPosition = new Vector3 (posX, movePoint, 0);
            yield return new WaitForSeconds(time);
            Icons[i].GetComponent<RectTransform>().localPosition = new Vector3 (posX, 0, 0);
            yield return new WaitForSeconds(time);
            Icons[i].GetComponent<RectTransform>().localPosition = new Vector3 (posX, movePoint, 0);
            yield return new WaitForSeconds(time);
            Icons[i].GetComponent<RectTransform>().localPosition =  new Vector3 (posX, 0, 0);
            yield return new WaitForSeconds(time);
            i++;
            i = i%Icons.Count;  
        }

        CoListClear();        
        yield return null;
    }

    void CoListClear()
    {
        for (int j = 0; j < Icons.Count; j++)
        {
            Icons[j].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        CoList.Clear();
    }
}
