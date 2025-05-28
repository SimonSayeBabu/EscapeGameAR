using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class cauldron : MonoBehaviour, Interactible
{
    public Material material;
    private int activeRecipe = -1;
    private int[,] recipes = {{20,11,15,0,0},{10,12,20,21,0}};
    //Recipe 1 : Growth potion = Water, Red mushroom, Pale mushroom
    //Recipe 2 : Objective = Brown mushroom, White mushroom, Water, Special plant
    private int currentStep = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset() {
        activeRecipe = -1;
        changeColor(0);
    }

    public void addItem(int item)
    {
        if (activeRecipe == -1)
        {
            for (int i = 0; i < 2; i++)
            {
                if (item == recipes[i,0])
                {
                    currentStep++;
                    changeColor(item);
                    activeRecipe = i;
                }
            }
            if (activeRecipe == -1)
            {
                changeColor(-1);
                activeRecipe = -2;
            }
        }
        else if (activeRecipe != -2)
        {
            if (recipes[activeRecipe,currentStep] == item)
            {
                currentStep++;
                changeColor(item);
            }
            else
            {
                currentStep = 0;
                activeRecipe = -2;
                changeColor(-1);
            }
        }
        if (activeRecipe >= 0 && recipes[activeRecipe,currentStep] == 0)
        {
            finishRecipe(activeRecipe);
            changeColor(activeRecipe);
        }
    }

    private void changeColor(int value)
    {
        //TODO
    }

    private void finishRecipe(int recipe)
    {
        //TODO
    }


    public void Interact(Inventory playerInventory)
    {
        //Ouvre l'interface pour le chaudron
    }

    public void LongInteract(Inventory playerInventory)
    {
    }
}
