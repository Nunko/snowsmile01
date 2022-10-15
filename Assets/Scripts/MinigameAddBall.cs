using System.Collections.Generic;
using UnityEngine;

public class MinigameAddBall : MonoBehaviour
{
    public List<Sprite> BallImages;

    void Start()
    {
        ChooseImage();
    }

    void ChooseImage()
    {        
        this.GetComponent<SpriteRenderer>().sprite = BallImages[Random.Range(0, BallImages.Count)];
    }
}