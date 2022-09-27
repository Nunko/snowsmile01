using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject LoadingPanel;
    public GameObject IconImage;
    public List<Sprite> Icons;
    public float time;
    List<string> CoList;

    void Awake()
    {
        CoList = new List<string>();
    }

    void Start()
    {
        StartCoroutine("MoveIcon");   
    }

    void Update()
    {
        if (CoList.Count == 0 && LoadingPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            StartCoroutine("MoveIcon");  
        }
    }

    
    IEnumerator MoveIcon()
    {
        CoList.Add("MoveIcon");
        int i = 0;
        RectTransform IconImageRT = IconImage.GetComponent<RectTransform>();
        float floatX = IconImageRT.localPosition.x;
        float floatY = IconImageRT.localPosition.y;
        while (LoadingPanel.GetComponent<CanvasGroup>().alpha == 1)
        {                            
            IconImage.GetComponent<Image>().sprite = Icons[i];
            yield return new WaitForSeconds(time);
            IconImageRT.localPosition = new Vector3(floatX, floatY + 5.6f, 1);
            yield return new WaitForSeconds(time);
            IconImageRT.localPosition = new Vector3(floatX, floatY, 1);
            yield return new WaitForSeconds(time);
            IconImageRT.localPosition = new Vector3(floatX, floatY + 5.6f, 1);
            yield return new WaitForSeconds(time);
            IconImageRT.localPosition = new Vector3(floatX, floatY, 1);
            yield return new WaitForSeconds(time);
            i++;
            i = i%Icons.Count;
        }
        CoList.Clear();
        yield return null;
    }
}
