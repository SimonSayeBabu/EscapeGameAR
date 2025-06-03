using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantes : MonoBehaviour
{

    public List<GameObject> plantes;
    public GameObject collectible;
    private Vector3 initialVector;
    private bool isFinished = false;
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        initialVector = plantes[0].transform.localScale;
        sceneController = FindAnyObjectByType<SceneController>();
        updateStatus(sceneController.solvedTubes,sceneController.isUndergroundPuzzleSolved);
        Debug.Log("nb tubes="+sceneController.solvedTubes+" isSolved="+sceneController.isUndergroundPuzzleSolved);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setFernScale(int scale)
    {
        float s = (float)scale / 100;
        foreach (GameObject item in plantes)
        {
            item.transform.localScale = Vector3.Scale(initialVector, new Vector3(s, s, s));
        }
    }

    private void spawnCollectible()
    {
        foreach (GameObject item in plantes)
        {
            Instantiate(collectible, item.transform.position, Quaternion.identity);
        }
    }

    public void updateStatus(int tuyau, bool serum)
    {
        if (!isFinished & serum)
        {
            if (tuyau == 0)
            {
                setFernScale(50);
            }
            if (tuyau == 1)
            {
                setFernScale(100);
            }
            if (tuyau == 2)
            {
                setFernScale(0);
                spawnCollectible();
                isFinished = true;
            }
        }

    }

}
