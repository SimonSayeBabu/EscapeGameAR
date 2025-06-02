using UnityEngine;


public class Cauldron : MonoBehaviour, Interactible
{
    public Material material;
    private int currentStep;
    private int activeRecipe = -1;
    public int[,] recipes = {{20,11,15,0,0},{20,10,12,21,0}};
    public int[] ingredients = {20,11,12,15,21,13,14,10,21};
    //Recipe 1 : Growth potion = Water, Red mushroom, Pale mushroom
    //Recipe 2 : Objective = Water, Brown mushroom, White mushroom, Special plant
    public UIHandler uiHandler;
    private bool cooldown;
    
    public GameObject resultat1;
    public GameObject resultat2;

    public ParticleSystem bulles;
    private bool bullesStatus;
    public ParticleSystem fumee;
    private ParticleSystem.MainModule mainBulles;
    private ParticleSystem.MainModule mainFumee;

    // Start is called before the first frame update
    void Start()
    {
        material.color = new Color(0f, 0f, 0f, 0f);
        uiHandler = FindAnyObjectByType<UIHandler>();
        bulles.Stop();
        fumee.Stop();
        mainBulles = bulles.main;
        mainFumee = fumee.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset()
    {
        activeRecipe = -1;
        changeColor(0);
        cooldown = false;
    }

    public void addItem(int item)
    {
        if (!cooldown)
        {
            if (currentStep == 0)
            {
                if (item == 20)
                {
                    currentStep++;
                    changeColor(item);
                }
            }
            else
            {
                if (activeRecipe == -1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (item == recipes[i, currentStep])
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
                    if (recipes[activeRecipe, currentStep] == item)
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
                if (activeRecipe >= 0 && recipes[activeRecipe, currentStep] == 0)
                {
                    finishRecipe(activeRecipe);
                }
            }
            
        }
        
        
    }

    private void changeColor(int value)
    {
        if (value == 0)
        {
            this.material.color = new Color(0f,0f,0f,0f);
            bulles.Stop();
            bullesStatus = false;
            return;
        }
        Color selectedColor = Color.white;

        if (value == 20)
        {
            selectedColor = new Color(0.6784314f, 0.8470589f, 0.9019608f, 1f);
        }
        else if (value == -1)
        {
            selectedColor = Color.black;
        }
        else if (value == 11)
        {
            selectedColor = Color.red;
        }
        else if (value == 15)
        {
            selectedColor = new Color(0.8627452f, 0.1921569f, 0.1960784f, 1f);
        }
        else if (value == 10)
        {
            selectedColor = new Color(0.8235295f, 0.4117647f, 0.1176471f, 1f);
        }
        else if (value == 12)
        {
            selectedColor = new Color(0.9607844f, 0.9607844f, 0.8627452f, 1f);
        }
        else if (value == 21)
        {
            selectedColor = new Color(0.627451f, 0.1254902f, 0.9411765f, 1f);
        }


        if (!bullesStatus)
        {
            bulles.Play();
        }
        mainBulles.startColor = selectedColor;
        this.material.color = selectedColor;
    }

    private void finishRecipe(int recipe)
    {
        Invoke("reset", 1f);

        fumee.Play();
        recipes[recipe, 0] = -1;
        if (recipe == 0)
        {
            Instantiate(resultat1, new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z), Quaternion.identity);
        }
        else if (recipe == 1)
        {
            Instantiate(resultat2, new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z), Quaternion.identity);
        }
        cooldown = true;
    }


    public void Interact(Inventory playerInventory)
    {
        //finishRecipe(0);
        uiHandler.ShowCauldron(playerInventory, this);
        //changeColor(11);
    }

    public void LongInteract(Inventory playerInventory)
    {
        
    }
}
