﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode : MonoBehaviour {

    public float U_input = 0;
    public float U_output = 0;


    void Start ()
    {
		
	}
	
	
	void Update ()
    {
        if (U_input > 0) U_output = U_input;	
	}
}
