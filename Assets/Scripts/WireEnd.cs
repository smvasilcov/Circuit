using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireEnd : MonoBehaviour {

    public bool connected;
    public string pole;
    public int circuitID = -1;
    public GameObject AnotherWireEnd;
    public GameObject CircuitElement;

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "BatteryMinus":
                AnotherWireEnd.GetComponent<WireEnd>().pole = pole = "Minus";
                connected = true;
                circuitID = 0;
                this.GetComponentInParent<Wire>().connectedToBattery = true;

                if (other.GetComponentInParent<CurrentSource>())
                    this.GetComponentInParent<Wire>().CurrentSource = other.GetComponentInParent<CurrentSource>().gameObject;
                else// if (other.GetComponentInParent<Wire>())
                    this.GetComponentInParent<Wire>().CurrentSource = other.GetComponentInParent<Wire>().CurrentSource;
                break;

            case "BatteryPlus":
                AnotherWireEnd.GetComponent<WireEnd>().pole = pole = "Plus";
                connected = true;
                circuitID = 0;
                this.GetComponentInParent<Wire>().connectedToBattery = true;

                if (other.GetComponentInParent<CurrentSource>())
                    this.GetComponentInParent<Wire>().CurrentSource = other.GetComponentInParent<CurrentSource>().gameObject;
                else// if (other.GetComponentInParent<Wire>())
                    this.GetComponentInParent<Wire>().CurrentSource = other.GetComponentInParent<Wire>().CurrentSource;
                break;

            case "ElementMinus":
                connected = true;

                this.GetComponentInParent<Wire>().ElementHolder = other.GetComponentInParent<ElementHolder>().gameObject;
                other.GetComponentInParent<ElementHolder>().thereIsNegativePole = true;

                if (this.GetComponentInParent<Wire>().connectedToBattery)
                    other.GetComponentInParent<ElementHolder>().wireID = circuitID;
                else
                    AnotherWireEnd.GetComponent<WireEnd>().circuitID = circuitID 
                        = other.GetComponentInParent<ElementHolder>().wireID;

                if (pole == "Plus")
                    other.GetComponentInParent<ElementHolder>().invercedCurrent = true;
                else
                    other.GetComponentInParent<ElementHolder>().invercedCurrent = false;

                other.GetComponentInParent<ElementHolder>().ElementMinusWire = this.GetComponentInParent<Wire>().gameObject;

                CircuitElement = other.GetComponentInParent<ElementHolder>().circuitElement;
                break;

            case "ElementPlus":
                connected = true;

                this.GetComponentInParent<Wire>().ElementHolder = other.GetComponentInParent<ElementHolder>().gameObject;
                other.GetComponentInParent<ElementHolder>().thereIsPositivePole = true;

                if (this.GetComponentInParent<Wire>().connectedToBattery)
                    other.GetComponentInParent<ElementHolder>().wireID = circuitID;
                else
                    AnotherWireEnd.GetComponent<WireEnd>().circuitID = circuitID
                        = other.GetComponentInParent<ElementHolder>().wireID;


                if (pole == "Plus")
                    other.GetComponentInParent<ElementHolder>().invercedCurrent = false;
                else
                    other.GetComponentInParent<ElementHolder>().invercedCurrent = true;

                other.GetComponentInParent<ElementHolder>().ElementPlusWire = this.GetComponentInParent<Wire>().gameObject;

                CircuitElement = other.GetComponentInParent<ElementHolder>().circuitElement;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BatteryMinus" || other.tag == "BatteryPlus")
        {
            connected = false;
            pole = " ";
            this.GetComponentInParent<Wire>().connectedToBattery = false;
            this.GetComponentInParent<Wire>().CurrentSource = null;
        }

        if (other.tag == "ElementMinus")
        {
            connected = false;
            this.GetComponentInParent<Wire>().ElementHolder = null;
            other.GetComponentInParent<ElementHolder>().thereIsNegativePole = false;
            CircuitElement = null;
        }

        if (other.tag == "ElementPlus")
        {
            connected = false;
            this.GetComponentInParent<Wire>().ElementHolder = null;
            other.GetComponentInParent<ElementHolder>().thereIsPositivePole = false;
            CircuitElement = null;
        }

        if((other.tag == "ElementMinus" || other.tag == "ElementPlus") && other.GetComponentInParent<ElementHolder>().insideSerialBlock)
        {
            int indexOfElementInsideSerialBlock;
            indexOfElementInsideSerialBlock = other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.FindIndex(other.GetComponentInParent<ElementHolder>().circuitElement.Equals);
            int indexOfAnotherWireEndElement;
            indexOfAnotherWireEndElement = other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.FindIndex(AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.Equals);
            Debug.LogWarning(indexOfElementInsideSerialBlock + " in " + (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1));

            if(indexOfAnotherWireEndElement != 0 && indexOfAnotherWireEndElement != (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1)
                && indexOfElementInsideSerialBlock != 0 && indexOfElementInsideSerialBlock != (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
            {
                // Разбить на разные SerialBlock
                if (indexOfElementInsideSerialBlock < indexOfAnotherWireEndElement) indexOfElementInsideSerialBlock = indexOfAnotherWireEndElement;

                GameObject AnotherSerialBlock;
                GameObject CurrentSerialBlock = other.GetComponentInParent<ElementHolder>().SerialBlock;
                this.GetComponentInParent<WireController>().SerialBlock.Add(AnotherSerialBlock = Instantiate(this.GetComponentInParent<WireController>().SerialBlockPrefab));
                int TempNumOfElementsInSerialBlock = CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element.Count;
                for (int i = indexOfElementInsideSerialBlock; i < TempNumOfElementsInSerialBlock; i++)
                {
                    // Добавляю элемент в новый SerialBlock
                    Debug.Log(i + " из " + TempNumOfElementsInSerialBlock);
                    Debug.Log("Добавляю элемент в новый SerialBlock " + CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element[i].name);
                    AnotherSerialBlock.GetComponent<SerialBlockCircuit>().Element.Add(CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element[i]);

                    // Перемещаю объект физически из одного SerialBlock в другой
                    Debug.Log("Перемещаю объект физически из одного SerialBlock в другой");
                    CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = AnotherSerialBlock.transform;
                    CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = AnotherSerialBlock;
                }
                foreach(GameObject element in AnotherSerialBlock.GetComponent<SerialBlockCircuit>().Element)
                {
                    // Удаляю элемент из старого SerialBlock
                    Debug.Log("Удаляю элемент из старого SerialBlock " + element.name);
                    CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(element);
                }
                CurrentSerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                AnotherSerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                this.GetComponentInParent<Wire>().insideSerialBlock = false;

            }
            else // if(indexOfAnotherWireEndElement == 0 || indexOfAnotherWireEndElement == (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
            {
                if(indexOfAnotherWireEndElement == 0 && indexOfElementInsideSerialBlock != (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Нулевой объект вычленить отдельно / нулевой объект находится на AnotherWireEnd
                    Debug.LogWarning("Вычленить нулевой объект отдельно");
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = false;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.transform.parent = transform.root;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement);
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = null;
                    this.GetComponentInParent<Wire>().insideSerialBlock = false;
                }
                else if(indexOfElementInsideSerialBlock == 0 && indexOfAnotherWireEndElement != (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Нулевой объект вычленить отдельно / нулевой объект находится на текущем конце провода
                    Debug.LogWarning("Вычленить нулевой объект отдельно");
                    other.GetComponentInParent<ElementHolder>().insideSerialBlock = false;
                    other.GetComponentInParent<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    other.GetComponentInParent<ElementHolder>().circuitElement.transform.parent = transform.root;
                    other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(other.GetComponentInParent<ElementHolder>().circuitElement);
                    other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                    other.GetComponentInParent<ElementHolder>().SerialBlock = null;
                    this.GetComponentInParent<Wire>().insideSerialBlock = false;
                }
                else if(indexOfAnotherWireEndElement == (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1) && indexOfElementInsideSerialBlock != 0)
                {
                    // Вычленить последний объект отдельно / последний объект находится на AnotherWireEnd
                    Debug.LogWarning("Вычленить последний объект отдельно");
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = false;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.transform.parent = transform.root;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement);
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = null;
                    this.GetComponentInParent<Wire>().insideSerialBlock = false;
                }
                else if (indexOfElementInsideSerialBlock == (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1) && indexOfAnotherWireEndElement != 0)
                {
                    // Вычленить последний объект отдельно / последний объект находится на текущем конце провода
                    Debug.LogWarning("Вычленить последний объект отдельно");
                    other.GetComponentInParent<ElementHolder>().insideSerialBlock = false;
                    other.GetComponentInParent<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    other.GetComponentInParent<ElementHolder>().circuitElement.transform.parent = transform.root;
                    other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(other.GetComponentInParent<ElementHolder>().circuitElement);
                    other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
                    other.GetComponentInParent<ElementHolder>().SerialBlock = null;
                    this.GetComponentInParent<Wire>().insideSerialBlock = false;
                }

                else if((indexOfAnotherWireEndElement == 0 && indexOfElementInsideSerialBlock == (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                    || (indexOfAnotherWireEndElement == (other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1) && indexOfElementInsideSerialBlock == 0))
                {
                    // В списке всего 2 объекта. Разделить их на части и удалить SerialBlock
                    other.GetComponentInParent<ElementHolder>().insideSerialBlock = false;
                    other.GetComponentInParent<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    other.GetComponentInParent<ElementHolder>().circuitElement.transform.parent = transform.root;
                    other.GetComponentInParent<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(other.GetComponentInParent<ElementHolder>().circuitElement);
                    other.GetComponentInParent<ElementHolder>().SerialBlock = null;

                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = false;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.SendMessage("SetU", 0f);
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.transform.parent = transform.root;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock.GetComponent<SerialBlockCircuit>().Element.Remove(AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement);
                    GameObject TempSerialBlock = AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock;
                    AnotherWireEnd.GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = null;
                    this.GetComponentInParent<Wire>().insideSerialBlock = false;
                    Object.Destroy(TempSerialBlock);
                }
            }
        }
    }
}
