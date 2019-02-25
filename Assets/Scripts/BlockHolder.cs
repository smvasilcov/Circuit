using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHolder : MonoBehaviour {

    public GameObject[] Element = new GameObject[2];
    public bool[] elementIsSerial = new bool[2];
    public bool[] elementIsBlockHolder = new bool[2];
    public float U_source = 0;
    public float R_common = 0;
    public float I_common = 0;
    public float[] R = new float[2];
    public float[] U = new float[2];
    public float[] goalU = new float[2];
    //public float[] goalU_test = new float[2];
    public bool serial = true;
    public bool check = false;

    void Start ()
    {

        if (Element[0].GetComponent<BlockHolder>() == null)
        {
            elementIsSerial[0] = true;
            if (Element[0].GetComponent<ParallelBlockCircuit>() != null)
            {
                elementIsSerial[0] = false;
            }
        }
        else
        {
            elementIsBlockHolder[0] = true;
        }

        if(Element[1].GetComponent<BlockHolder>() == null)
        { 
            elementIsSerial[1] = true;
            if (Element[1].GetComponent<ParallelBlockCircuit>() != null)
            {
                elementIsSerial[1] = false;
            }
        }
        else
        {
            elementIsBlockHolder[1] = true;
        }
    }

    private void Update()
    {

       Check();

    }

    void Check()
    {
        
            if (!elementIsBlockHolder[0])
            {
                if (elementIsSerial[0])
                {
                    R[0] = Element[0].GetComponent<SerialBlockCircuit>().R_common;
                    U[0] = Element[0].GetComponent<SerialBlockCircuit>().U_source;
                }
                else
                {
                    R[0] = Element[0].GetComponent<ParallelBlockCircuit>().R_common;
                    U[0] = Element[0].GetComponent<ParallelBlockCircuit>().U_source;
                }
            }
            else
            {
                R[0] = Element[0].GetComponent<BlockHolder>().R_common;
                U[0] = Element[0].GetComponent<BlockHolder>().U_source;
            }

            if (!elementIsBlockHolder[1])
            {
                if (elementIsSerial[1])
                {
                    R[1] = Element[1].GetComponent<SerialBlockCircuit>().R_common;
                    U[1] = Element[1].GetComponent<SerialBlockCircuit>().U_source;
                }
                else
                {
                    R[1] = Element[1].GetComponent<ParallelBlockCircuit>().R_common;
                    U[1] = Element[1].GetComponent<ParallelBlockCircuit>().U_source;
                }
            }
            else
            {
                R[1] = Element[1].GetComponent<BlockHolder>().R_common;
                U[1] = Element[1].GetComponent<BlockHolder>().U_source;
            }

        if (serial)
        {
            //goalU_test[1] = (U_source * (R[1] - R[0]) + U[0] * R[0]) / R[1];
            goalU[1] = U_source * R[1] / (R[1] + R[0]);
            goalU[0] = U_source - goalU[1];
                Element[0].SendMessage("SetU_source", goalU[0]);
                Element[1].SendMessage("SetU_source", goalU[1]);
                R_common = R[0] + R[1];
                I_common = U_source / R_common;
            

        }
        else if (!serial)
        {
            Element[0].SendMessage("SetU_source", U_source);
            Element[1].SendMessage("SetU_source", U_source);
            R_common = 1 / (1 / R[0] + 1 / R[1]);
            I_common = U_source / R_common;
        }
    }

    

    void SetU_source(float u)
    {
        U_source = u;
    }
}
