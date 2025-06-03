using UnityEngine;

public class Soup : MonoBehaviour, Interactible
{
    private SceneController sceneController;
    
    void Start()
    {
        sceneController = FindAnyObjectByType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Inventory inventory)
    {
        sceneController.SwitchScenes(7);
    }

    public void LongInteract(Inventory inventory)
    {
        sceneController.SwitchScenes(7);
    }
}
