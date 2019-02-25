using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlug : MonoBehaviour {
    private bool _tryToConnect;

    public GameObject BlockViewGameObject;

    public IBlockView BlockView { get; private set; }

    private BlockSlot _connectedSlot;

    void Start()
    {
        BlockView = BlockViewGameObject.GetComponent<IBlockView>();
    }

    public void TryToConnect()
    {
        _tryToConnect = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_tryToConnect)
        {
            if (_connectedSlot != null)
            {
                _connectedSlot.Disconnect(this);
                _connectedSlot = null;
            }

            var slot = other.gameObject.GetComponent<BlockSlot>();
            if (slot == null)
            {
                
                return;
            }

            slot.Connect(this);
            _connectedSlot = slot;
        }

        _tryToConnect = false;
    }
}
