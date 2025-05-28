using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public List<int> content = new List<int>();

    public int getItem(int index)
    {
        return this.content[index];
    }

    public bool contains(int item)
    {
        return content.IndexOf(item) != -1;    
    }

    public int whereIs(int item)
    {
        return content.IndexOf(item);
    }

    public void addItem(int item)
    {
        this.content.Add(item);
    }

    public void removeItem(int index)
    {
        this.content.RemoveAt(index);
    }

    public int useItem(int index)
    {
        int temp = content[index];
        this.content.RemoveAt(index);
        return temp;
    }

    //Marche Pas 
    public string toString() 
    {
        if (this.content.Count == 0)
        {
            return "0";
        }
        return string.Join(", ", content);
    }
}
