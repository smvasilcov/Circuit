using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour {

    public int ID = 0;
    public float R = 0;
    public float U = 0;
    public float I = 0;
    public float P = 0;

    public float speed = 0;
    public float k_speed = 0;
    public float rotationAngle = 0;
    public GameObject Rotor;

    public string parallelOrSerial = "";

    void FixedUpdate()
    {
        rotationAngle += Time.deltaTime * speed;
        if (rotationAngle > 360.0f) rotationAngle = 0;


        speed = U * k_speed;
            
        Rotor.transform.localRotation = Quaternion.Euler(rotationAngle, 0, 0);

        I = U / R;
        P = I * U;
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
        if (parallelOrSerial == "serial") GetComponentInParent<SerialBlockCircuit>().R[ID] = R;
        if (parallelOrSerial == "parallel") GetComponentInParent<ParallelBlockCircuit>().R[ID] = R;
    }


}
