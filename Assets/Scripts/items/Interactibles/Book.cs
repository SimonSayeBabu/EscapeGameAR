using System;
using UnityEngine;

public class Book : MonoBehaviour, Interactible
{

    public UIHandler  uiHandler;
    public string txt;
    public bool isImage;
    public Sprite image;
    
    void Awake()
    {
        Debug.Log($"[Book] Awake called on {gameObject.name}");
    }

    void Start()
    {
        Debug.Log($"[Book] Start called on {gameObject.name}");
        uiHandler = FindAnyObjectByType<UIHandler>();
        if (uiHandler == null)
        {
            Debug.LogError("[Book] No UIHandler found!");
        }
        else
        {
            Debug.Log("[Book] UIHandler successfully assigned");
        }
    }
    
    public void Interact(Inventory playerInventory)
    {
        if (!uiHandler)
        {
            Debug.LogError("UIHandler is NULL in Book.Interact()");
            return;
        }

        if (isImage)
        {
            uiHandler.ShowBook(image);
        }
        else
        {
            uiHandler.ShowBook(txt);
        }
    }

    public void LongInteract(Inventory playerInventory) 
    {
        
    }
}
