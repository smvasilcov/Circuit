using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSource : MonoBehaviour {

    public bool DCSource = true;
    public float frequency = 0;
    public float Imax = 0;
    public float Umax = 0;
    public float time = 0;
    public float I = 0;
    public float IZero = 0;
    public float U = 0;
    public float UZero = 0;
    public bool positiveCurrent;


	void Update ()
    {

        if (!DCSource)
        {
            time = Time.time;
            if (I >= 0)
            {
                positiveCurrent = true;
            }
            else
            {
                positiveCurrent = false;
            }

            I = Imax * Mathf.Sin(2 * Mathf.PI * frequency * time);
            IZero = Imax * Mathf.Sin(2 * Mathf.PI * frequency * time + Mathf.PI);
            U = Umax * Mathf.Sin(2 * Mathf.PI * frequency * time);
            UZero = Umax * Mathf.Sin(2 * Mathf.PI * frequency * time + Mathf.PI);
        } else
        {
            U = Umax;
        }
    }
   
}
