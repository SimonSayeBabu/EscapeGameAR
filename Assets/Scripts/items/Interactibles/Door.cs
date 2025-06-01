using UnityEngine;
using UnityEngine.SceneManagement;  



public class Door: MonoBehaviour, Interactible
{

    public int sceneID;
    public int keyID;
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindAnyObjectByType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(Inventory playerInventory)
    {
        if (keyID != -1)
        {
            if (playerInventory.contains(keyID) != -1)
            {
                playerInventory.content[playerInventory.contains(keyID)].active = false;
                sceneController.SwitchScenes(sceneID);
            }
            
        }
        else
        {
            sceneController.SwitchScenes(sceneID);
        }
            
    }

    public void LongInteract(Inventory playerInventory)
    {
    }
}
