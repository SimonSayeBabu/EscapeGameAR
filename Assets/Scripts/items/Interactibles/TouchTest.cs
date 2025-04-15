using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class TouchTest : MonoBehaviour, Interactible
{

    public GameObject PlanetObject;

    public Vector3 RotationVector;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlanetObject.transform.Rotate(RotationVector * Time.deltaTime);
    }

    public void Interact(Inventory playerInventory)
    {
        RotationVector *= -1;
    }

    public void LongInteract(Inventory playerInventory) 
    {
        Destroy(this.gameObject);
    }
}
