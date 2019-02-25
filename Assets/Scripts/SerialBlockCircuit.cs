using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialBlockCircuit : MonoBehaviour {

    public List<GameObject> Element;
    public GameObject SerialBlockElementHolder;
    public float U_source = 0;
    public float[] U;
    public float[] R;
    public float[] S;   // Variable for Lamp only
    public float R_common = 0;
    public float R_ofResistors = 0;
    public float I_common = 0;
    public float S_common = 0;
    public bool lampExist = false;
    public string parallelOrSerial = "serial";

    void Start ()
    {
        InicialiseVariables();
        SetIdToElements();
        SetParallelOrSerial();
	}

    private void OnEnable()
    {
        InicialiseVariables();
        SetIdToElements();
        SetParallelOrSerial();
    }

    public void Initialise()
    {
        InicialiseVariables();
        SetIdToElements();
        SetParallelOrSerial();
    }



    void Update ()
    {
        CheckLampExistance();
        GetR();
        GetS();
        SetI();
        SetU();
        SetupSerialBlockElementHolder();
    }

    void InicialiseVariables()
    {
        U = new float[Element.Count];
        R = new float[Element.Count];
        S = new float[Element.Count];
    }

    void SetIdToElements()
    {
        for (int i = 0; i < Element.Count ; i++)
        {
            Element[i].SendMessage("SetID", i);
        }
    }

    void SetParallelOrSerial()
    {
        for (int i = 0; i < Element.Count ; i++)
        {
            Element[i].SendMessage("SetParallelOrSerial", parallelOrSerial);
        }
    }

    /*void MonitoringU()
    {
        for (int i = 0; i < Element.Count  ; i++)
        {
            float previousU = U[i];
            Element[i].SendMessage("GetU");
            if (previousU != U[i])
            {
                Debug.Log("Element #" + i + " changed U to " + U[i]);
            }
        }
    }*/

    void GetR()
    {
        R_common = 0;
        R_ofResistors = 0;
        for (int i = 0; i < Element.Count  ; i++)
        {
            Element[i].SendMessage("GetR");
            R_common += R[i];
            if (Element[i].tag != "Lamp") R_ofResistors += R[i];
        }
    }

    void GetS()
    {
        S_common = 0;
        for (int i = 0; i < Element.Count  ; i++)
        {
            if (Element[i].tag == "Lamp")
            {
                Element[i].SendMessage("GetS");
                S_common += S[i] * S[i];
            }
        }
        S_common = Mathf.Sqrt(S_common);
    }

    void SetI()
    {
        if(lampExist)
        {
            I_common = Mathf.Sqrt(U_source) / S_common;
        }else
        {
            I_common = U_source / R_common;
        }
        for (int i = 0; i < Element.Count  ; i++)
        {
            Element[i].SendMessage("SetI", I_common);
        }
    }

    void SetU()
    {
        for (int i = 0; i < Element.Count  ; i++)
        {
            U[i] = I_common * R[i];

            if(Element[i].tag == "Lamp" && R_ofResistors != 0)
            {
                float R_ofOtherElements = R_common - R[i];
                float D = (R_ofOtherElements / S[i]) + (4 * U_source);
                if (D < 0)
                {
                    Debug.LogError("Cannot deal with quadratic equation");
                    return;
                }
                float x1 = (-(R_ofOtherElements / S[i]) + Mathf.Sqrt(D)) / 2;
                float x2 = (-(R_ofOtherElements / S[i]) - Mathf.Sqrt(D)) / 2;

                if (x1 >= 0) U[i] = Mathf.Pow(x1, 2);
                if (x2 >= 0) U[i] = Mathf.Pow(x2, 2);

                /*float D = (R_ofResistors / S_common) + (4 * U_source);
                if(D < 0)
                {
                    Debug.LogError("Cannot deal with quadratic equation");
                    return;
                }
                float x1 = (-(R_ofResistors / S_common) + Mathf.Sqrt(D)) / 2;
                float x2 = (-(R_ofResistors / S_common) - Mathf.Sqrt(D)) / 2;

                if (x1 >= 0) U[i] = Mathf.Pow(x1, 2);   
                if (x2 >= 0) U[i] = Mathf.Pow(x2, 2);*/
            }

            Element[i].SendMessage("SetU", U[i]);
        }
    }



    void CheckLampExistance()
    {
        for (int i = 0; i < Element.Count  ; i++)
        {
            if (Element[i].tag == "Lamp") lampExist = true;
        }
    }

    void SetU(float i)
    {
        U_source = i;
    }

    void SetU_source(float i)
    {
        U_source = i;
    }

    void SetupSerialBlockElementHolder()
    {
        if(Element.Count > 0)
        {
            GameObject ElementPlus;
            GameObject ElementMinus;
            foreach(Collider contact in SerialBlockElementHolder.GetComponentsInChildren<BoxCollider>())
            {
                if (contact.gameObject.CompareTag("ElementPlus")) ElementPlus = contact.gameObject;
                if (contact.gameObject.CompareTag("ElementMinus")) ElementMinus = contact.gameObject;
            }

            GameObject ZeroElementPlusContact;
            GameObject ZeroElementMinusContact;
            foreach (Collider contact in Element[0].GetComponentInChildren<ElementHolder>().gameObject.GetComponentsInChildren<BoxCollider>())
            {
                if (contact.gameObject.CompareTag("ElementPlus")) ZeroElementPlusContact = contact.gameObject;
                if (contact.gameObject.CompareTag("ElementMinus")) ZeroElementMinusContact = contact.gameObject;
            }

            GameObject LastElementPlusContact;
            GameObject LastElementMinusContact;
            foreach (Collider contact in Element[Element.Count - 1].GetComponentInChildren<ElementHolder>().gameObject.GetComponentsInChildren<BoxCollider>())
            {
                if (contact.gameObject.CompareTag("ElementPlus")) LastElementPlusContact = contact.gameObject;
                if (contact.gameObject.CompareTag("ElementMinus")) LastElementMinusContact = contact.gameObject;
            }

            //ElementPlus.transform = ZeroElementPlusContact.transform;
        }
    }
}
