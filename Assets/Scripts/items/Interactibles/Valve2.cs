using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve2 : MonoBehaviour, Interactible
{
    EnigmeValve enigmeValve;
    private int status = 0;
    private bool cooldown = false;
    private int rotation = 0;
    private int nValve = 2;


    // Start is called before the first frame update
    void Start()
    {
        enigmeValve = GameObject.FindGameObjectWithTag("EnigmeValve").GetComponent<EnigmeValve>();

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
        if ((status == 1 | !(enigmeValve.isOpen())) && cooldown == false)
        {
            Invoke("ResetCooldown", 0.5f);
            status = (status + 1) % 2;
            if (status == 0) rotation = 45; else rotation = -45;
            gameObject.transform.Rotate(0, 0, rotation);
            cooldown = true;
        }
        enigmeValve.setValve(status, nValve);

    }
        

    public void LongInteract(Inventory playerInventory)
    {
    }
}
