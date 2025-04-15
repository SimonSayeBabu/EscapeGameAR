using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    public Text invtxt;
    public GameObject UIPanel;
    public GameObject SetupPanel;
    public GameObject TutorialPanel;
    
    public RaycastController controller;
    public GameObject start;
    public GameObject sphere;
    public GameObject cube;
    public GameObject earth;
    public GameObject TestCollectible;
    public GameObject Cauldron;
    public GameObject TutoDesk ;


    public int showUI = -1;


    // Start is called before the first frame update
    void Start()
    {
        invtxt.text = "";
        UIPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        SetupPanel.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSetupClick()
    {
        SetupPanel.SetActive(true);
        start.SetActive(false);
    }

    public void ButtonSetCorner()
    {
        controller.SetupCorner();
    }
    
    public void ButtonEndSetup()
    {
        if (controller.isCornerSetupDone)
        {
            SetupPanel.SetActive(false);
            TutorialPanel.SetActive(true);
            controller.Setup();
        }
    }
    public void ButtonEndTuto()
    {
        TutorialPanel.SetActive(false);
    }

    public void ButtonSphereClick()
    {
        controller.PlacedPrefab = sphere;
    }

    public void ButtonCubeClick()
    {
        controller.PlacedPrefab = cube;
    }

    public void ButtonEarthClick()
    {
        controller.PlacedPrefab = earth;
    }
    public void ButtonCauldronClick()
    {
        controller.PlacedPrefab = Cauldron;
    }

    public void ButtonCollectibleClick()
    {
        controller.PlacedPrefab = TestCollectible;
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
