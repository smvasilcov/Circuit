using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SizeChangedHandler(Vector2 newSize);

public interface IBlockView
{
    Vector2 Size { get; }
    Vector2 FullSize { get; }
    Vector2 Position { get; set; }

    void TryToConnect();

    IBlock Block { get; }

    event SizeChangedHandler SizeChanged;
}
