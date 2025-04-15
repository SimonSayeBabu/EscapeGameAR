using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class cauldron : MonoBehaviour, Interactible
{
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Inventory playerInventory)
    {
        int IsItemOwned = playerInventory.content.IndexOf(11);
        if (IsItemOwned != -1)
        {
            this.material.color = Color.red;
            playerInventory.useItem(IsItemOwned);
        }
    }

    public void LongInteract(Inventory playerInventory)
    {
    }
}
