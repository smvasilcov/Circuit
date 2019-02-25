using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : MonoBehaviour {

    public int ID = 0;
    public float R = 0;
    public float U = 0;
    public float I = 0;
    public float P = 0;

    public string parallelOrSerial = "";

    public void SetID(int i)
    {
        ID = i;
    }
    public void SetParallelOrSerial(string s)
    {
        parallelOrSerial = s;
    }
    public void SetI(float i)
    {
        I = i;
    }
    public void SetU(float u)
    {
        U = u;
    }
    public void GetR()
    {
        if (parallelOrSerial == "serial") GetComponentInParent<SerialBlockCircuit>().R[ID] = R;
        if (parallelOrSerial == "parallel") GetComponentInParent<ParallelBlockCircuit>().R[ID] = R;
    }
}
