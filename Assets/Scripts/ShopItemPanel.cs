using UnityEngine;

public class ShopItemPanel : MonoBehaviour
{
    Shop shop;

    void Awake()
    {
        shop = FindObjectOfType<Shop>();
    }
    
    public void ClickItemPanel()
    {
        shop.ClickItemPanel(int.Parse(this.name[(this.name.Length - 1)].ToString()));
    }
}
