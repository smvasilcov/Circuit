using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBlockView : MonoBehaviour, IBlockView {
    public Vector2 Position
    {
        set { transform.position = value; }
        get { return transform.position; }
    }

    public Transform LeftPlane;
    public Transform BottomPlane;

    public IBlock Block => _loopBlock;
    public event SizeChangedHandler SizeChanged;

    private LoopBlock _loopBlock;
    
    public BlockPlug BlockPlug;
    public BlockSlot BodyBlockSlot;

    void Awake()
    {
        _loopBlock = GetComponent<LoopBlock>();
    }

    void Start()
    {
        BodyBlockSlot.TryToConnect += BodyBlockSlotOnTryToConnect;
        BodyBlockSlot.TryToDisconnect += BodyBlockSlotOnTryToDisconnect;
    }

    public void Execute()
    {
        _loopBlock.Execute(null);
    }

    private void BodyBlockSlotOnTryToDisconnect(BlockPlug plug)
    {
        plug.BlockViewGameObject.transform.SetParent(null);
        _loopBlock.LoopBody = null;
    }

    public void TryToConnect() 
    {
        BlockPlug.TryToConnect();
    }

    private void BodyBlockSlotOnTryToConnect(BlockPlug plug)
    {
        plug.BlockViewGameObject.transform.SetParent(BodyBlockSlot.transform);
        plug.BlockViewGameObject.transform.localPosition = Vector3.zero;

        Size = plug.BlockView.FullSize;
        FullSize = Size + plug.BlockView.FullSize;
        SizeChanged?.Invoke(FullSize);

        plug.BlockView.SizeChanged += BlockViewOnSizeChanged;
        BlockViewOnSizeChanged(Size);

        _loopBlock.LoopBody = plug.BlockView.Block;
    }

    private void BlockViewOnSizeChanged(Vector2 newSize)
    {
        LeftPlane.localScale = new Vector3(1, newSize.y, 1);
        BottomPlane.localPosition = new Vector3(0, -newSize.y, 0);
    }

    public Vector2 Size { get; private set; }
    public Vector2 FullSize { get; private set; }
}
