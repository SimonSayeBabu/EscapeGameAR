using UnityEngine;

public class Drawers : MonoBehaviour, Interactible
{
    public int side = 1;
    public bool isLocked;
    private bool isOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script on " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(Inventory playerInventory)
    {
        Debug.Log("touched " + gameObject.name);
        if (!isLocked)
        {
            Open();
        }
        else
        {
            if (playerInventory.content.IndexOf(1) != -1)
            {
                Open();
            }
        }

        
    }

    public void LongInteract(Inventory playerInventory) {}
    
    private void Open()
    {
        if (!isOpen)
        {
            gameObject.transform.Rotate(0,0,120*side);
        }
        else
        {
            gameObject.transform.Rotate(0,0,-120*side);
        }
        isOpen = !isOpen;
    }
}
