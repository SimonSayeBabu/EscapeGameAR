using System;
using UnityEngine;

public class Book : MonoBehaviour, Interactible
{

    private UIHandler  uiHandler;
    public string txt;
    public bool isImage;
    public Sprite image;
    
    // Start is called before the first frame update
    void Start()
    {
        uiHandler = FindAnyObjectByType<UIHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Interact(Inventory playerInventory)
    {
        if (isImage)
        {
            Debug.Log(isImage);
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
