using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoopBlock : BaseAsyncBlock
{
    public GameObject NextBlock = null;
    public GameObject LoopBodyBlock = null;

    public IBlock LoopBody { get; set; }

    void Start()
    {
        Next = NextBlock?.GetComponent<IBlock>();

        LoopBody = LoopBodyBlock?.GetComponent<IBlock>();
    }

    protected override IEnumerator OnExecuteAsync()
    {
        LoopBody?.Execute(LoopBodyBlockOnFinished);
        yield return null;
    }

    private void LoopBodyBlockOnFinished()
    {
        LoopBody?.Execute(LoopBodyBlockOnFinished);
    }
}
