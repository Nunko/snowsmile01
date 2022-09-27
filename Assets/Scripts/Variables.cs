using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables : MonoBehaviour
{
    public int LoadInk()
    {
        Debug.Log("불러오기: 잉크");
        if(ES3.KeyExists("currentInkAmount") == false)
        {
            ES3.Save<int>("currentInkAmount", 0);
        }

        return ES3.Load<int>("currentInkAmount");
    }

    public void SaveInk(int amount)
    {
        Debug.Log("저장하기: 잉크");        
        ES3.Save<int>("currentInkAmount", amount);
    }

    public List<string> LoadWorldItem()
    {
        Debug.Log("불러오기: 출현 수집품");
        if(ES3.KeyExists("myWorldItems") == false)
        {
            List<string> myWorldItems = new List<string>();
            for (int i = 0 ; i < 30; i++)
            {
                myWorldItems.Add("blank");                
            }
            Debug.Log("출현 수집품 " + myWorldItems.Count + "칸");
            ES3.Save("myWorldItems", myWorldItems);
        }

        return ES3.Load<List<string>>("myWorldItems");        
    }

    public void SaveWorldItem(List<string> myWorldItems)
    {
        Debug.Log("저장하기: 출현 수집품");
        ES3.Save<List<string>>("myWorldItems", myWorldItems);
    }

    public Dictionary<string, int> LoadBag()
    {
        Debug.Log("불러오기: 가방");
        if(ES3.KeyExists("bag") == false)
        {
            Dictionary<string, int> bag = new Dictionary<string, int>();
            ES3.Save("bag", bag);
        }

        return ES3.Load<Dictionary<string, int>>("bag");
    }

    public void SaveBag(Dictionary<string, int> bag)
    {
        Debug.Log("저장하기: 가방");
        ES3.Save<Dictionary<string, int>>("bag", bag);
    }
}
