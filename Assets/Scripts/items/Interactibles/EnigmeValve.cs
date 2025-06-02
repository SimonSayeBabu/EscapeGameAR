using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeValve : MonoBehaviour
{
    private int[] valves = new int[6];
    private int[] correctValves = { 1, 2 };
    private bool serumStatus = false;
    private SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindAnyObjectByType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int somme()
    {
        int s = 0;
        foreach (int v in valves)
        {
            s += v;
        }
        return s;
    }

    public void setValve(int v, int n)
    {
        if (somme() < 2 || v == 0)
        {
            valves[n - 1] = v;
        }
    }

    public bool isOpen()
    {
        if (somme() >= 1)
        {
            return true;
        }
        return false;
    }

    public void setSerum(bool s)
    {
        serumStatus = s;
        sceneController.isUndergroundPuzzleSolved = s;
    }

    public bool getSerumStatus()
    {
        return serumStatus;
    }

    public bool amIOpen(int valve)
    {
        return valves[valve - 1] == 1;
    }

    public int nbCorrectValves()
    {
        int ret = 0;
        if (valves[correctValves[0]] == 1)
        {
            ret += 1;
        }
        if (valves[correctValves[1]] == 1)
        {
            ret += 1;
        }
        return ret;
    }
}
