using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{

    public RaycastController placer;

    public GameObject sphere;
    public GameObject cube;
    public GameObject earth;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSphereClick()
    {
        this.placer.PlacedPrefab = this.sphere;
    }

    public void ButtonCubeClick()
    {
        this.placer.PlacedPrefab = this.cube;
    }

    public void ButtonEarthClick()
    {
        this.placer.PlacedPrefab = this.earth;
    }
}
