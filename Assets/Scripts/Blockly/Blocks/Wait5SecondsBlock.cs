using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait5SecondsBlock : BaseAsyncBlock {
    protected override IEnumerator OnExecuteAsync()
    {
        yield return new WaitForSeconds(5);
    }
}
