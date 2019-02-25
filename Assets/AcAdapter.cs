using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcAdapter : MonoBehaviour {

    public GameObject ACSource;
    public GameObject Diode;
    public GameObject Lamp;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Diode.GetComponent<DiodeBridge>().U_input = ACSource.GetComponent<CurrentSource>().U;
        Lamp.GetComponent<BlockHolder>().U_source = Diode.GetComponent<DiodeBridge>().U_output;

    }
}
