using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintHelloBlock : BaseAsyncBlock {
    protected override IEnumerator OnExecuteAsync()
    {
        Debug.Log("Hello");
        yield return new WaitForSeconds(0.5f);
    }

    void Start()
    {
        Next = NextBlock?.GetComponent<IBlock>();
    }

    public GameObject NextBlock = null;
}
