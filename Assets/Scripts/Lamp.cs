using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {


    public int ID = 0;
    public float R = 1;
    public float U = 0;
    public float I = 0;
    public float P = 0;
    public float S = 0;

    public float Umax = 0;
    public float Pmax = 0;
   

    public GameObject[] Visualiser;
    public string parallelOrSerial = "";
    Color color = Color.yellow;

    private bool isBroken = false; 

    void Start ()
    {
        S = Mathf.Sqrt(Mathf.Pow(Umax, 3)) / Pmax;
	}
	

	void LateUpdate ()
    {
        R = Mathf.Abs(Mathf.Sqrt(  Mathf.Abs(U)  ) * S);
        S = Mathf.Sqrt(Mathf.Pow(Umax, 3)) / Pmax;
        P = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(U), 3)) / S;

        if (R == 0) R = 0.1f;
        Visualise();
    }

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
        if (this.GetComponentInParent<SerialBlockCircuit>())
        {
            if (parallelOrSerial == "serial") GetComponentInParent<SerialBlockCircuit>().R[ID] = R;
            if (parallelOrSerial == "parallel") GetComponentInParent<ParallelBlockCircuit>().R[ID] = R;
        }
    }
    public void GetS()
    {
        if (this.GetComponentInParent<SerialBlockCircuit>())
        {
            if (parallelOrSerial == "serial") GetComponentInParent<SerialBlockCircuit>().S[ID] = S;
            if (parallelOrSerial == "parallel") GetComponentInParent<ParallelBlockCircuit>().S[ID] = S;
        }
    }
    public void Visualise()
    {
        float intensity;
        if (P <= Pmax && !isBroken)
        {
            intensity = (P / Pmax) * 1;
            color.r = intensity;
            color.g = intensity;
            Visualiser[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            Visualiser[1].GetComponent<Light>().intensity = intensity * 3;
        }
        else
        {
            isBroken = true;
            intensity = 0;
            color = Color.black;
            Visualiser[0].GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
            Visualiser[1].GetComponent<Light>().intensity = intensity * 3;
            R = Mathf.Infinity;
            Pmax = 0;

        }
    }
}
