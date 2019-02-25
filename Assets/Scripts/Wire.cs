using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour {

    public int circuitID;       // ID контура
    public int wireID;          // ID каждого провода отдельно
    public GameObject[] wireEnds = new GameObject[2];
    public GameObject CurrentSource;
    public GameObject ElementHolder;
    public bool pole = false;
    public bool connected;
    public bool connectedToBattery = false;
    public bool alreadyAddedToWireControllerBatteryArray = false;
    public bool insideSerialBlock = false;

    private GameObject InsideBlockElement0;
    private GameObject InsideBlockElement1;

    private GameObject SerialBlock;

    void Update ()
    {
        DrawLine();
        CheckBatteryPole();
        CheckCircuitID();
        CheckConnection();
        CheckBatteryElementConnection();
    }

    void DrawLine()
    {
        this.GetComponent<LineRenderer>().SetPosition(0, wireEnds[0].transform.position);
        this.GetComponent<LineRenderer>().SetPosition(1, wireEnds[1].transform.position);
        if (pole)
            this.GetComponent<LineRenderer>().material.color = Color.red;
        else
            this.GetComponent<LineRenderer>().material.color = Color.blue;
    }

    void CheckBatteryPole()
    {
        if (wireEnds[0].GetComponent<WireEnd>().pole == "Minus" || wireEnds[1].GetComponent<WireEnd>().pole == "Minus")
        {
            pole = false;
            wireEnds[0].GetComponent<WireEnd>().pole = wireEnds[1].GetComponent<WireEnd>().pole = "Minus";
        }
        if (wireEnds[0].GetComponent<WireEnd>().pole == "Plus" || wireEnds[1].GetComponent<WireEnd>().pole == "Plus")
        {
            pole = true;
            wireEnds[0].GetComponent<WireEnd>().pole = wireEnds[1].GetComponent<WireEnd>().pole = "Plus";
        }
    }

    void CheckCircuitID()
    {
        if (connectedToBattery)
        {
            wireEnds[0].GetComponent<WireEnd>().circuitID = wireEnds[1].GetComponent<WireEnd>().circuitID = 0;

            if (pole) wireEnds[0].tag = wireEnds[1].tag = "BatteryPlus";
            else wireEnds[0].tag = wireEnds[1].tag = "BatteryMinus";
        }
        else
        {
            wireEnds[0].tag = wireEnds[1].tag = "Untagged";
        }


        if (wireEnds[0].GetComponent<WireEnd>().circuitID == wireEnds[1].GetComponent<WireEnd>().circuitID)
            circuitID = wireEnds[0].GetComponent<WireEnd>().circuitID;
        else
            Debug.LogError("CircuitID on wireEnds are not equal: " + wireEnds[0].GetComponent<WireEnd>().circuitID + " and " + wireEnds[1].GetComponent<WireEnd>().circuitID);
    }

    void CheckConnection()
    {
        if (wireEnds[0].GetComponent<WireEnd>().connected && wireEnds[1].GetComponent<WireEnd>().connected &&
            wireEnds[0].GetComponent<WireEnd>().CircuitElement != wireEnds[1].GetComponent<WireEnd>().CircuitElement)
            connected = true;
        else
            connected = false;
     
        if(connected && !connectedToBattery && !insideSerialBlock)
        {
            if (wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == false &&
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == false)
            {
                // Создать новый SerialBlock и добавить в него элементы
                this.GetComponentInParent<WireController>().SerialBlock.Add(SerialBlock = Instantiate(this.GetComponentInParent<WireController>().SerialBlockPrefab));
                SerialBlock.GetComponent<SerialBlockCircuit>().Element.Add(wireEnds[0].GetComponent<WireEnd>().CircuitElement);
                SerialBlock.GetComponent<SerialBlockCircuit>().Element.Add(wireEnds[1].GetComponent<WireEnd>().CircuitElement);
                wireEnds[0].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                insideSerialBlock = true;
                wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = true;
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = true;
                wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock;
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock;
                InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
            }
            else if (wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == true &&
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == false)
            {
                // Нулевой элемент уже в SerialBlock, нужно добавить в него первый элемент
                SerialBlock = wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock;

                int indexOfElementInsideSerialBlock;
                indexOfElementInsideSerialBlock = SerialBlock.GetComponent<SerialBlockCircuit>().Element.FindIndex(wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.Equals);

                if(indexOfElementInsideSerialBlock == (SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Элемент нужно добавить к концу списка SerialBlock
                    SerialBlock.GetComponent<SerialBlockCircuit>().Element.Add(wireEnds[1].GetComponent<WireEnd>().CircuitElement);
                    wireEnds[1].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                }
                else if(indexOfElementInsideSerialBlock == 0)
                {
                    // Элемент нужно добавить в начало списка
                    SerialBlock.GetComponent<SerialBlockCircuit>().Element.Insert(0, wireEnds[1].GetComponent<WireEnd>().CircuitElement);
                    wireEnds[1].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                    wireEnds[1].GetComponent<WireEnd>().CircuitElement.transform.SetSiblingIndex(0);
                }

                insideSerialBlock = true;
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = true;
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock;
                InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
            }
            else if (wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == false &&
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == true)
            {
                // Первый элемент уже в SerialBlock, нужно добавить к нему нулевой элемент
                SerialBlock = wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock;

                int indexOfElementInsideSerialBlock;
                indexOfElementInsideSerialBlock = SerialBlock.GetComponent<SerialBlockCircuit>().Element.FindIndex(wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.Equals);

                if (indexOfElementInsideSerialBlock == (SerialBlock.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Элемент нужно добавить к концу списка SerialBlock
                    SerialBlock.GetComponent<SerialBlockCircuit>().Element.Add(wireEnds[0].GetComponent<WireEnd>().CircuitElement);
                    wireEnds[0].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                }
                else if (indexOfElementInsideSerialBlock == 0)
                {
                    // Элемент нужно добавить в начало списка
                    SerialBlock.GetComponent<SerialBlockCircuit>().Element.Insert(0, wireEnds[0].GetComponent<WireEnd>().CircuitElement);
                    wireEnds[0].GetComponent<WireEnd>().CircuitElement.transform.parent = SerialBlock.transform;
                    wireEnds[0].GetComponent<WireEnd>().CircuitElement.transform.SetSiblingIndex(0);
                }

                insideSerialBlock = true;
                wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock = true;
                wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock;
                InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                SerialBlock.GetComponent<SerialBlockCircuit>().Initialise();
            }
            else if (wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == true &&
                wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().insideSerialBlock == true)
            {
                // Оба элемента находятся в своих SerialBlock. Нужно объединить их,
                // добавить элементы к тому SerialBlock, у которого провод подключен к крайнему элементу
                // и проранжировать элементы второго SerialBlock, чтобы они добавились в нужно порядке
                // !! Объединение SerialBlock может происходить только в крайних позициях
                Debug.Log("Оба элемента находятся в своих SerialBlock. Нужно объединить их, \nдобавить элементы к тому SerialBlock, у которого провод подключен к крайнему элементу \nи проранжировать элементы второго SerialBlock, чтобы они добавились в нужно порядке");
                GameObject SerialBlock0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock;
                GameObject SerialBlock1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().SerialBlock;

                int indexOfElementInsideSerialBlock0;
                indexOfElementInsideSerialBlock0 = SerialBlock0.GetComponent<SerialBlockCircuit>().Element.FindIndex(wireEnds[0].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.Equals);

                int indexOfElementInsideSerialBlock1;
                indexOfElementInsideSerialBlock1 = SerialBlock1.GetComponent<SerialBlockCircuit>().Element.FindIndex(wireEnds[1].GetComponent<WireEnd>().CircuitElement.GetComponentInChildren<ElementHolder>().circuitElement.Equals);

                if (indexOfElementInsideSerialBlock0 == (SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Элементы нужно добавлять к нулевому SerialBlock
                    // Выбран крайний элемент нулевого SerialBlock
                    Debug.Log("Элементы нужно добавлять к нулевому SerialBlock \nВыбран крайний элемент нулевого SerialBlock");
                    if (indexOfElementInsideSerialBlock1 == (SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                    {
                        // Элементы первого SerialBlock необходимо проранжировать (добавлять элементы с конца)
                        Debug.Log("Элементы первого SerialBlock необходимо проранжировать (добавлять элементы с конца)");
                        SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Reverse();
                        for (int i = 0; i < SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Count; i++)
                        {
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Add(SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i]); // Перенос елементов из 1 блока в 0
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = SerialBlock0.transform;
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock0;
                            insideSerialBlock = true;
                            InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                            InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                        }
                    }
                    else if (indexOfElementInsideSerialBlock1 == 0)
                    {
                        // Элементы первого SerialBlock уже стоят в правльном порядке. Ранжировать их не надо
                        Debug.Log("Элементы первого SerialBlock уже стоят в правльном порядке. Ранжировать их не надо");
                        for (int i = 0; i < SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Count; i++)
                        {
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Add(SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i]); // Перенос елементов из 1 блока в 0
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = SerialBlock0.transform;
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock0;
                            insideSerialBlock = true;
                            InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                            InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                        }
                    }
                    else
                    {
                        Debug.LogError("Wire.cs / 207 / Подключение двух элементов, стоящих в середине своих SerialBlock's");
                    }

                }
                else if(indexOfElementInsideSerialBlock1 == (SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                {
                    // Элементы нужно добавлять к первому SerialBlock
                    // Выбран крайний элемент первого SerialBlock
                    Debug.Log("Элементы нужно добавлять к первому SerialBlock \nВыбран крайний элемент первого SerialBlock");
                    if (indexOfElementInsideSerialBlock0 == (SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Count - 1))
                    {
                        // Элементы нулевого SerialBlock необходимо проранжировать (добавлять элементы с конца)
                        Debug.Log("Элементы нулевого SerialBlock необходимо проранжировать (добавлять элементы с конца)");
                        SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Reverse();
                        for (int i = 0; i < SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Count; i++)
                        {
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Add(SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i]); // Перенос элементов из 0 блока в 1
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = SerialBlock1.transform;
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock1;
                            insideSerialBlock = true;
                            InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                            InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                        }
                    }
                    else if (indexOfElementInsideSerialBlock0 == 0)
                    {
                        // Элементы нулевого SerialBlock уже стоят в правльном порядке. Ранжировать их не надо
                        Debug.Log("Элементы нулевого SerialBlock уже стоят в правльном порядке. Ранжировать их не надо");
                        for (int i = 0; i < SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Count; i++)
                        {
                            SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Add(SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i]); // Перенос элементов из 0 блока в 1
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = SerialBlock1.transform;
                            SerialBlock0.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock1;
                            insideSerialBlock = true;
                            InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                            InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                        }
                    }
                    else
                    {
                        Debug.LogError("Wire.cs / 247 / Подключение двух элементов, стоящих в середине своих SerialBlock's");
                    }

                }
                else if(indexOfElementInsideSerialBlock0 == 0 && indexOfElementInsideSerialBlock1 == 0)
                {
                    // Оба элемента являются нулевыми в своих SerialBlock
                    // Нужно проранжировать оба SerialBlock's и сложить друг с другом
                    Debug.Log("Оба элемента являются нулевыми в своих SerialBlock \n Нужно проранжировать оба SerialBlock's и сложить друг с другом");
                    SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Reverse();
                    SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Reverse();
                    for (int i = 0; i < SerialBlock1.GetComponent<SerialBlockCircuit>().Element.Count; i++)
                    {
                        SerialBlock0.GetComponent<SerialBlockCircuit>().Element.Add(SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i]); // Перенос елементов из 1 блока в 0
                        SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].transform.parent = SerialBlock0.transform;
                        SerialBlock1.GetComponent<SerialBlockCircuit>().Element[i].GetComponentInChildren<ElementHolder>().SerialBlock = SerialBlock0;
                        insideSerialBlock = true;
                        InsideBlockElement0 = wireEnds[0].GetComponent<WireEnd>().CircuitElement;
                        InsideBlockElement1 = wireEnds[1].GetComponent<WireEnd>().CircuitElement;
                    }
                }
                else
                {
                    Debug.LogError("Wire.cs / 270 / Ошибка объединения SerialBlock's");
                }

                SerialBlock0.GetComponent<SerialBlockCircuit>().Initialise();
                Object.Destroy(SerialBlock1);
            }
        }

    }



    void CheckBatteryElementConnection()
    {
        if (connectedToBattery && connected && !alreadyAddedToWireControllerBatteryArray)
        {
            this.GetComponentInParent<WireController>().connectedToBatteryWiresID.Add(wireID);
            alreadyAddedToWireControllerBatteryArray = true;
        }

        if(alreadyAddedToWireControllerBatteryArray && (!connected || !connectedToBattery))
        {
            this.GetComponentInParent<WireController>().connectedToBatteryWiresID.Remove(wireID);
            alreadyAddedToWireControllerBatteryArray = false;
        }
            
    }
}
