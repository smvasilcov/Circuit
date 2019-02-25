using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    public delegate void ConnectHandler(BlockPlug plug);

    public event ConnectHandler TryToConnect;
    public event ConnectHandler TryToDisconnect;

    public void Disconnect(BlockPlug plug)
    {
        TryToDisconnect?.Invoke(plug);
    }

    public void Connect(BlockPlug plug)
    {
        TryToConnect?.Invoke(plug);
    }
}
