using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;

public class MinigameAddBall : MonoBehaviour
{
    public List<Sprite> BallImages;

    void Start()
    {
        ChooseImage();
    }

    void ChooseImage()
    {        
        gameObject.GetComponent<SpriteRenderer>().sprite = BallImages[Random.Range(0, BallImages.Count)];
    }
}