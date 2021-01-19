using Assets.Scripts.Core;
using System;
using System.Collections;

/// <summary>
/// Defines async callback action
/// </summary>
public class TrustContractManager : SingletonBehaviour<TrustContractManager>
{
    public void Sign<Suc,Err>(Action<Action<Suc>, Action<Err>> method, Action<Suc> onSuccess, Action<Err> onFailed)
    {
        method(onSuccess, onFailed);
    }

    public void Sign(IEnumerator routine, Action onCompleted)
    {
        StartCoroutine(Run(routine, onCompleted));
    }

    private IEnumerator Run(IEnumerator routine, Action onCompleted)
    {
        yield return routine;
        onCompleted();
    }
}
