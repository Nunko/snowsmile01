using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemPanel : MonoBehaviour
{
    Shop shop;

    void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }
    
    public void ClickItemPanel()
    {
        shop.ClickItemPanel(int.Parse(gameObject.name[(gameObject.name.Length - 1)].ToString()));
    }
}
