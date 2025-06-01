using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject SetupPanel;
    public GameObject TutorialPanel;
    public GameObject StartSetup;
    
    public GameObject InventoryPanel;
    public GameObject InventoryButton;
    public List<GameObject> ItemPanels;
    public GameObject CauldronUi;
    
    public GameObject BookPanel;
    public Text bookTxt;
    public Image bookImg;

    public RaycastController controller;
    public PrefabManager prefabManager;
    public int showUI = -1;


    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        SetupPanel.SetActive(false);
        BookPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        CauldronUi.SetActive(false);

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

    public void ButtonEarthClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Earth");
    }

    public void ButtonCauldronClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Cauldron");
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
        controller.PlacedPrefab = prefabManager.GetPrefab("Indice");
    }

    public void ButtonBookClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("Indice");
    }

    public void ShowUI()
    {
        showUI *= -1;
        UIPanel.SetActive(showUI > 0);
    }

    public void UpdateInv(Inventory inventory)
    {
        for (int i = 0; i < ItemPanels.Count; i++)
        {
            Image img = ItemPanels[i].transform.Find("Image").GetComponent<Image>();

            if (i < inventory.content.Count && i < 8)
            {
                Collectible item = inventory.content[i];
                img.sprite = item.icon;
                img.enabled = true;
            }
            else
            {
                img.enabled = false;
            }
        }
    }

    public void ShowCauldron()
    {
        CauldronUi.SetActive(true);
        InventoryPanel.SetActive(true);
        InventoryButton.SetActive(false);
    }

    public void ShowBook(string content)
    {
        bookImg.gameObject.SetActive(false);
        bookTxt.text = content;
        BookPanel.SetActive(true);
    }

    public void ShowBook(Sprite image)
    {
        bookTxt.gameObject.SetActive(false);
        bookImg.sprite = image;
        BookPanel.SetActive(true);
    }

    public void HideBook()
    {
        BookPanel.SetActive(false);
    }
}