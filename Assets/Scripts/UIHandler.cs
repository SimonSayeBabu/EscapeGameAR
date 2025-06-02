using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject SetupPanel;
    public GameObject TutorialPanel;
    
    public GameObject InventoryPanel;
    public GameObject InventoryButton;
    public List<GameObject> ItemPanels;
    public GameObject CauldronUi;
    public List<GameObject> CauldronPanels;
    
    public GameObject BookPanel;
    public Text bookTxt;
    public Image bookImg;

    public RaycastController controller;
    public PlaneDetectionController PlaneController;
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
        InventoryButton.SetActive(false);
        CauldronUi.SetActive(false);

        controller = FindAnyObjectByType<RaycastController>();
        prefabManager = FindAnyObjectByType<PrefabManager>();
        DontDestroyOnLoad(gameObject);
    }

    public void ButtonEndSetup()
    {
        PlaneController.StopPlaneDetection();
        controller.sceneController.isSetupDone = true;
        controller.sceneController.CalculatePlacementData();
        controller.sceneController.Setup();
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
        controller.PlacedPrefab = prefabManager.GetPrefab("DoorToStart");
    }

    public void ButtonValveClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("EnigmeValve");
    }

    public void ButtonBookClick()
    {
        controller.PlacedPrefab = prefabManager.GetPrefab("TemplateBook");
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
                if (item.active)
                {
                    img.sprite = item.icon;
                    img.enabled = true;
                }
                else
                {
                    img.enabled = false;
                }
            }
            else
            {
                img.enabled = false;
            }
        }
    }

    public void ShowCauldron(Inventory playerInventory, Cauldron cauldron)
    {
        Debug.Log(playerInventory, cauldron);
        UpdateCauldronUI(playerInventory, cauldron);
        CauldronUi.SetActive(true);
        InventoryPanel.SetActive(false);
        InventoryButton.SetActive(false);
    }
    
    public void UpdateCauldronUI(Inventory inventory, Cauldron cauldron)
    {
        HashSet<int> validIDs = new HashSet<int>();

        for (int i = 0; i < cauldron.ingredients.Length; i++)
        {
            int id = cauldron.ingredients[i];
            if (id > 0)
                validIDs.Add(id);
        }

        List<Collectible> validItems = new List<Collectible>();

        foreach (Collectible item in inventory.content)
        {
            if (validIDs.Contains(item.id))
            {
                validItems.Add(item);
            }
        }

        for (int i = 0; i < CauldronPanels.Count; i++)
        {
            Image img = CauldronPanels[i].transform.Find("Image").GetComponent<Image>();
            Button btn = CauldronPanels[i].GetComponent<Button>();

            if (i < validItems.Count && i < 8)
            {
                Collectible item = validItems[i];
                img.sprite = item.icon;
                img.enabled = true;

                int id = item.id; 
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    cauldron.addItem(id);
                });
            }
            else
            {
                img.sprite = null;
                img.enabled = false;
            }
        }
    }
    
    public void ShowBook(string content)
    {
        bookImg.gameObject.SetActive(false);
        bookTxt.gameObject.SetActive(true); 
        bookTxt.text = content;
        BookPanel.SetActive(true);
    }

    public void ShowBook(Sprite image)
    {
        bookTxt.gameObject.SetActive(false);
        bookImg.gameObject.SetActive(true); 
        bookImg.sprite = image;
        BookPanel.SetActive(true);
    }
    
    public void HideBook()
    {
        bookTxt.gameObject.SetActive(false);
        bookImg.gameObject.SetActive(false);
        BookPanel.SetActive(false);
    }

}