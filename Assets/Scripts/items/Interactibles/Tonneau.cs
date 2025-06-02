using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serum : MonoBehaviour, Interactible
{

    public EnigmeValve enigmeValve;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        material.color = new Color(0f,0f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Interact(Inventory playerInventory)
    {
        if (playerInventory.contains(30) != -1)
        {
            enigmeValve.setSerum(true);
            material.color = Color.green;
        }
    }

    public void LongInteract(Inventory playerInventory)
    {
        
    }

}
