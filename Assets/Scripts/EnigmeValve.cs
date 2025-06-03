using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeValve : MonoBehaviour
{
    private int[] valves = new int[6];
    private int[] correctValves = { 1, 2 }; 
    private SceneController sceneController;

    void Start()
    {
        sceneController = FindAnyObjectByType<SceneController>();
        
    }

    private int SumValves()
    {
        int sum = 0;
        foreach (int v in valves)
        {
            sum += v;
        }
        return sum;
    }

    public void SetValve(int value, int valveIndex) // valveIndex is 1-based
    {
        if (SumValves() < 2 || value == 0)
        {
            valves[valveIndex - 1] = value;
        }
    
        sceneController.solvedTubes = CountCorrectValves();
    }

    public bool CanOpenAnotherValve()
    {
        return SumValves() < 2;
    }

    public bool IsValveOpen(int index)
    {
        return valves[index - 1] == 1;
    }

    private int CountCorrectValves()
    {
        int count = 0;
        foreach (int valveID in valves)
        {
            if (valveID != 0 && System.Array.Exists(correctValves, id => id == valveID))
            {
                count++;
            }
        }
        return count;
    }


}
