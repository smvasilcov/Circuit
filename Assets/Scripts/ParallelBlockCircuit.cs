using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBlockCircuit : MonoBehaviour
{
    public GameObject[] Element;
    public float U_source = 0;
    public float I_common = 0;
    public float R_common = 0;
    public float[] I;
    public float[] R;
    public float[] S;
    public float S_common = 0;


    public string parallelOrSerial = "parallel";
    

    void Start()
    {
        InicialiseVariables();
        SetIdToElements();
        SetParallelOrSerial();
    }


    void Update()
    {
        SetU();
        GetR();
        GetS();
        SetI();
    }

    void InicialiseVariables()
    {
        I = new float[Element.Length];
        R = new float[Element.Length];
        S = new float[Element.Length];
    }

    void SetIdToElements()
    {
        for (int i = 0; i < Element.Length; i++)
        {
            Element[i].SendMessage("SetID", i);
        }
    }

    void SetParallelOrSerial()
    {
        for (int i = 0; i < Element.Length; i++)
        {
            Element[i].SendMessage("SetParallelOrSerial", parallelOrSerial);
        }
    }

    void GetR()
    {
        R_common = 0;
        for (int i = 0; i < Element.Length; i++)
        {
            Element[i].SendMessage("GetR");
            R_common += 1 / R[i];
        }
        R_common = 1 / R_common;
    }

    void SetU()
    {
        for (int i = 0; i < Element.Length; i++)
        {
            Element[i].SendMessage("SetU", U_source);
        }
    }

    void SetI()
    {
        I_common = 0;
        for (int i = 0; i < Element.Length; i++)
        {
            I[i] = U_source / R[i];
            Element[i].SendMessage("SetI", I[i]);
            I_common += I[i];
        }
    }

    void GetS()
    {
        S_common = 0;
        float numenator = 1;
        float denumenator = 0;
        for (int i = 0; i < Element.Length; i++)
        {
            if (Element[i].tag == "Lamp")
            {
                Element[i].SendMessage("GetS");
                numenator *= S[i];
                denumenator += S[i];
            }
        }
        S_common = numenator / denumenator;
    }

    void SetU_source(float i)
    {
        U_source = i;
    }
}
