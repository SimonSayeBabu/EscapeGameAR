using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public List<Collectible> content = new List<Collectible>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public Collectible getItem(int index)
    {
        return content[index];
    }

    public int contains(int itemID)
    {
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].id == itemID)
            {
                return i;
            }
        }
        return -1;   
    }

    public void addItem(Collectible item)
    {
        content.Add(item);
    }

    public void removeItem(int index)
    {
        content.RemoveAt(index);
    }

    public Collectible useItem(int index)
    {
        Collectible temp = content[index];
        content.RemoveAt(index);
        return temp;
    }

}
