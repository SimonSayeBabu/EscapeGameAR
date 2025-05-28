using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Livre : MonoBehaviour, Interactible
{

    public GameObject Indice;
    private bool cooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        Indice.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetCooldown(){
        cooldown = false;
    }
    
    public void Interact(Inventory playerInventory)
    {
        if (cooldown == true)
        {
            Invoke("ResetCooldown", 0.5f);
            cooldown = true;
            Indice.SetActive(true);
        }
    }

    public void LongInteract(Inventory playerInventory) 
    {
        
    }
}
