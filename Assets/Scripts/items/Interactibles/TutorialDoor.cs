using UnityEngine;
using UnityEngine.SceneManagement;  



public class TutorialDoor : MonoBehaviour, Interactible
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(Inventory playerInventory)
    {
        if (playerInventory.content.IndexOf(2) != -1)
        {
            SceneManager.LoadScene("Alchemy Room");
        }
    }

    public void LongInteract(Inventory playerInventory)
    {
    }
}
