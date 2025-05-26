using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour, Interactible
{
    public EnigmeValve enigmeValve;
    private int isOpen = 0;
    private bool cooldown = false;
    private int rotation = 0;
    public int nValve;

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
        if ((isOpen == 1 | !(enigmeValve.isOpen())) && cooldown == false)
        {
            Invoke("ResetCooldown", 0.5f);
            cooldown = true;
            isOpen = (isOpen + 1) % 2;
            if (isOpen == 0) rotation = 45; else rotation = -45;
            gameObject.transform.Rotate(0, 0, rotation);
        }
        enigmeValve.setValve(isOpen, nValve);
    }
        

    public void LongInteract(Inventory playerInventory)
    {
    }
}
