using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilloscope : MonoBehaviour {

    public GameObject inputData;
    public GameObject ISphere;
    public GameObject VSphere;
    public bool I = false;
    public bool V = false;
    public bool diode = false;
    public bool diodeBridge = false;
    public float v_multiplicator = 1/150;

    private float time;
    private float iPhase = 0;
    private float iZero = 0;
    private float vPhase = 0;
    private float vZero = 0;

    void Update ()
    {

        iPhase = inputData.GetComponent<CurrentSource>().I;
        iZero = inputData.GetComponent<CurrentSource>().IZero;
        vPhase = inputData.GetComponent<CurrentSource>().U;
        vZero = inputData.GetComponent<CurrentSource>().UZero;
        time = inputData.GetComponent<CurrentSource>().time;


        if (I)
        {
            if (!diode && !diodeBridge)
            {
                GameObject.Instantiate(ISphere, new Vector3(-iPhase, 0, time), Quaternion.identity);
            }
            if (diode && iPhase > 0)
            {
                GameObject.Instantiate(ISphere, new Vector3(-iPhase, 0, time), Quaternion.identity);
            }
            if(diodeBridge)
            {
                if(iPhase > 0)
                {
                    GameObject.Instantiate(ISphere, new Vector3(-iPhase, 0, time), Quaternion.identity);
                }
                if(iZero > 0)
                {
                    GameObject.Instantiate(ISphere, new Vector3(-iZero, 0, time), Quaternion.identity);
                }
            }
        }	

        if(V)
        {
            if (diode && vPhase < 0) return;
            GameObject.Instantiate(VSphere, new Vector3(-vPhase*v_multiplicator, 0, time), Quaternion.identity);
        }
	}

}
