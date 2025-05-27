using UnityEngine;
using UnityEngine.SceneManagement;  



public class TutorialDoor : MonoBehaviour, Interactible
{

    public int sceneID;
    public int keyID;
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
        if (keyID is int && keyID != null)
        {
            if (playerInventory.content.IndexOf(keyID) != -1)
            {
                SceneManager.LoadScene(sceneID);
            }
            
        }
    }

    public void LongInteract(Inventory playerInventory)
    {
    }
}
