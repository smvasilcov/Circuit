using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAsyncBlock : MonoBehaviour, IBlock {
    public void Execute(FinishHandler onFinish)
    {
        StopAllCoroutines();
        StartCoroutine(ExecuteAsync(onFinish));
    }

    public event ExecuteHandler ExuteStarted;
    public event ExecuteHandler ExecuteEnded;

    protected abstract IEnumerator OnExecuteAsync();

    private IEnumerator ExecuteAsync(FinishHandler OnFinish)
    {
        ExuteStarted?.Invoke();
        yield return StartCoroutine(OnExecuteAsync());
        if (Next != null)
        {
            Next.Execute(OnFinish);
        }
        else
        {
            OnFinish?.Invoke();
        }
        ExecuteEnded?.Invoke();
    }

    public IBlock Next { get; set; }
    public event EventHandler Finished;
}
