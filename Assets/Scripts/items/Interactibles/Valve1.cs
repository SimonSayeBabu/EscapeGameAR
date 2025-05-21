using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve1 : MonoBehaviour, Interactible
{
    int valveStatus = 0;
    private bool cooldown = false;
    int rotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (cooldown == false)
        {
            Invoke("ResetCooldown",0.5f);
            valveStatus = (valveStatus+1)%2;
            if (valveStatus == 0) rotation = 45; else rotation = -45;
            gameObject.transform.Rotate(0,0,rotation);
            cooldown = true;
        }
    }
        

    public void LongInteract(Inventory playerInventory)
    {
    }
}
