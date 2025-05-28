using UnityEngine;

public class Livre : MonoBehaviour, Interactible
{

    private UIHandler  uiHandler;
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
        uiHandler.ShowBook("test !");
    }

    public void LongInteract(Inventory playerInventory) 
    {
        
    }
}
