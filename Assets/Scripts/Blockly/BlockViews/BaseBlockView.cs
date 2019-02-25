using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockView : MonoBehaviour, IBlockView {

    public Vector2 Position {
        set { transform.position = value; }
        get { return transform.position; }
    }

    public IBlock Block { get; private set; }
    public event SizeChangedHandler SizeChanged;

    public BlockPlug BlockPlug;
    public BlockSlot ChildSlot;

    private IBlockView _connectedView;

    [SerializeField]
    private float _height;

    void Start()
    {
        ChildSlot.TryToConnect += BodyBlockSlotOnTryToConnect;
        ChildSlot.TryToDisconnect += BodyBlockSlotOnTryToDisconnect;

        Block = GetComponent<IBlock>();
    }

    public void TryToConnect()
    {
        BlockPlug.TryToConnect();
    }

    private void BodyBlockSlotOnTryToDisconnect(BlockPlug plug)
    {
        plug.BlockViewGameObject.transform.SetParent(null);
        _connectedView = null;
        Block.Next = null;
        SizeChanged?.Invoke(FullSize);
    }

    private void BodyBlockSlotOnTryToConnect(BlockPlug plug)
    {
        plug.BlockViewGameObject.transform.SetParent(ChildSlot.transform);
        plug.BlockViewGameObject.transform.localPosition = Vector3.zero;
        _connectedView = plug.BlockView;
        SizeChanged?.Invoke(FullSize);
        plug.BlockView.SizeChanged += BlockViewOnSizeChanged;
        Block.Next = plug.BlockView.Block;
    }

    private void BlockViewOnSizeChanged(Vector2 newSize)
    {
        SizeChanged?.Invoke(FullSize);
    }

    public Vector2 Size => new Vector2(0, _height);
    public Vector2 FullSize => Size + (_connectedView?.FullSize ?? Vector2.zero);
}
