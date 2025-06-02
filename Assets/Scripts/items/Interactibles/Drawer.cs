using UnityEngine;

public class Drawer : MonoBehaviour, Interactible
{
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        Open();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(Inventory playerInventory)
    {
        if (!isOpen)
        {
            Open();
        }
    }

    public void LongInteract(Inventory playerInventory) {}
    
    private void Open()
    {
        if (!isOpen)
        {
            gameObject.transform.Translate(Vector3.Scale(Vector3.forward, new Vector3(0.25f, 0.25f, 0.25f)));
        }       
        isOpen = true;
    }
}
