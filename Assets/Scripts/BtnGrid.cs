using UnityEngine;

public class BtnGrid : MonoBehaviour
{
    public void ClickGrid()
    {
        CanvasGroup cg = this.transform.Find("BalloonImage").GetComponent<CanvasGroup>();
        if (cg.alpha == 0)
        {
            cg.alpha = 1;
            Debug.Log("말풍선 출현!");
        }
        else
        {
            cg.alpha = 0;
            Debug.Log("말풍선 사라짐!");
        }
    }
}
