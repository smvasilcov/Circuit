using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void FinishHandler();
public delegate void ExecuteHandler();

public interface IBlock
{
    void Execute(FinishHandler OnFinish);
    IBlock Next { get; set; }

    event ExecuteHandler ExuteStarted;
    event ExecuteHandler ExecuteEnded;
}
