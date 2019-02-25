using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour {

    public List<GameObject> Wire;
    public GameObject WirePrefab;
    public GameObject SerialBlockPrefab;
    public List<GameObject> SerialBlock;  
    public bool addWire = false;
    public bool connectedToBatteryAndElement;
    public List<int> connectedToBatteryWiresID; //= new List<int>;
    public int numCircuitID = 0;

    private void Start()
    {
        /*for(int i = 0; i<connectedToBatteryWiresID.Length; i++)
        {
            connectedToBatteryWiresID[i] = new List<int>(null);
        }*/
    }
    void Update ()
    {
		if(addWire)
        {
            addWire = false;
            AddWire();
        }
        //CheckConnectionToBattery();
        //DebugShow();
	}

    void AddWire()
    {
        GameObject newWire;
        newWire = Instantiate(WirePrefab, this.transform);
        newWire.transform.parent = this.transform;
        Wire.Add(newWire);
        newWire.GetComponent<Wire>().wireID = Wire.Count - 1;
    }

    void DebugShow()
    {
        //Debug.Log("Alabama");
        //for (int circuitID = 0; circuitID < numCircuitID; circuitID++)
        //{
           // Debug.Log("CircuitID = " + circuitID);
            foreach (int wireID in connectedToBatteryWiresID)//[circuitID])
            {
                Debug.Log("Wire ID = " + wireID);
            }
        //}
    }

    /*void CheckConnectionToBattery()
    {
        bool thereIsBatteryPositivePole = false;
        bool thereIsBatteryNegativePole = false;
        bool thereIsElementNegativePole = false;
        bool thereIsElementPositivePole = false;

        //for (int circuitID = 0; circuitID < numCircuitID; circuitID++)
        //{
        foreach (int wireID in connectedToBatteryWiresID)//[circuitID])
        {
            if (Wire[wireID].GetComponent<Wire>().pole)
                thereIsBatteryPositivePole = true;
            else
                thereIsBatteryNegativePole = true;
            if (Wire[wireID].GetComponent<Wire>().ElementHolder)
            {
                if (Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().thereIsNegativePole)
                    thereIsElementNegativePole = true;

                if (Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().thereIsPositivePole)
                    thereIsElementPositivePole = true;
            }
        }
        
            if (thereIsBatteryNegativePole && thereIsBatteryPositivePole && thereIsElementNegativePole && thereIsElementPositivePole)
            {
                connectedToBatteryAndElement = true;

                foreach (int wireID in connectedToBatteryWiresID)//[circuitID])
                {
                if (Wire[wireID].GetComponent<Wire>().CurrentSource)
                {
                    float U = Wire[wireID].GetComponent<Wire>().CurrentSource.GetComponent<CurrentSource>().U;
                    if (Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().invercedCurrent)
                    {
                        U = -U;
                        Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().circuitElement.SendMessage("SetU", U);
                    }
                    else
                    {
                        Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().circuitElement.SendMessage("SetU", U);
                    }
                    Debug.Log(U);
                }
                }

            }
            else
            {
                float U = 0;
                foreach (int wireID in connectedToBatteryWiresID)//[circuitID])
                {
                if(Wire[wireID].GetComponent<Wire>().ElementHolder)
                    Wire[wireID].GetComponent<Wire>().ElementHolder.GetComponent<ElementHolder>().circuitElement.SendMessage("SetU", U);
                }
            }*/
        //}
    //}
}
