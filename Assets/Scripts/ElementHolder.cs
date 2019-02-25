using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementHolder : MonoBehaviour {

    public bool invercedCurrent = false;
    public bool thereIsPositivePole;
    public bool thereIsNegativePole;
    public bool insideSerialBlock = false;
    
    public int wireID;
    public float U = 0;

    public GameObject circuitElement;
    public GameObject ElementPlusWire;
    public GameObject ElementMinusWire;
    public GameObject SerialBlock;

    private void Update()
    {
        if (thereIsNegativePole && thereIsPositivePole && // Питание подведено к обоим полюсам элемента
            ElementPlusWire.GetComponent<Wire>().CurrentSource == ElementMinusWire.GetComponent<Wire>().CurrentSource && // Один и тот же источник питания
            ElementPlusWire.GetComponent<Wire>().pole != ElementMinusWire.GetComponent<Wire>().pole) // Питание разных полюсов
        {
            U = ElementMinusWire.GetComponent<Wire>().CurrentSource.GetComponent<CurrentSource>().U;
        }
        else U = 0;

        if (invercedCurrent) U = -U;
        circuitElement.SendMessage("SetU", U);
    }
}
