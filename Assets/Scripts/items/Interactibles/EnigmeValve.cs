using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeValve : MonoBehaviour
{
    int[] valves = new int[6];
    int[] correctValves = { 1, 2 };

    // Start is called before the first frame update
    void Start()
    {

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

    public bool amIOpen(int valve)
    {
        return valves[valve - 1] == 1;
    }
}
