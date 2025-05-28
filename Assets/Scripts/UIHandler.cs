using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    public Text invtxt;
    
    public GameObject UIPanel;
    public GameObject SetupPanel;
    public GameObject TutorialPanel;
    public GameObject StartSetup;
    
    public RaycastController controller;
    public PrefabManager prefabManager;
    
    public int showUI = -1;


    // Start is called before the first frame update
    void Start()
    {
        invtxt.text = "";
        UIPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        SetupPanel.SetActive(false);
        controller = FindAnyObjectByType<RaycastController>();
        prefabManager = FindAnyObjectByType<PrefabManager>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSetupClick()
    {
        StartSetup.SetActive(false);
        SetupPanel.SetActive(true);
    }

    public void ButtonSetCorner()
    {
        controller.SetupCorner();
    }
    
    public void ButtonEndSetup()
    {
        if (controller.sceneController.isCornerSetupDone)
        {
            SetupPanel.SetActive(false);
            TutorialPanel.SetActive(true);
            controller.sceneController.Setup();
        }
    }
    public void ButtonEndTuto()
    {
        TutorialPanel.SetActive(false);
    }

    public void ButtonSphereClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Sphere");
    }

    public void ButtonCubeClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Cube");
    }

    public void ButtonEarthClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Earth");
    }
    public void ButtonCauldronClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Big_Cauldron");
    }

    public void ButtonCollectibleClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Mushroom Planter");
    }
    
    public void ButtonDoorClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("DoorToAlchemy");
    }

    public void ButtonValveClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("EnigmeValve");
    }

    public void ShowUI()
    {
        showUI *= -1;
        UIPanel.SetActive(showUI > 0);
    }

    public IEnumerator UpdateInv(string content)
    {
        invtxt.text = "inv : " + content;
        yield return new WaitForSeconds(0f);
    }
}
