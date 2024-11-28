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

    public void Interact()
    {
        RotationVector *= -1;
    }

    public void LongInteract() 
    {
        Destroy(this.gameObject);
    }
}
