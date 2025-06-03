using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonneau : MonoBehaviour, Interactible
{
    public Material material;
    private SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        material.color = new Color(0f,0f,0f,0f);
        sceneController = FindAnyObjectByType<SceneController>();
        if (sceneController.isUndergroundPuzzleSolved)
        {
            material.color = Color.green;
        }
    }
    
    public void Interact(Inventory playerInventory)
    {
        if (playerInventory.contains(30) != -1)
        {
            material.color = Color.green;
            sceneController.isUndergroundPuzzleSolved = true;
        }
    }

    public void LongInteract(Inventory playerInventory){}
}
