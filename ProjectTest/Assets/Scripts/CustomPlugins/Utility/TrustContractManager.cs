using Assets.Scripts.Core;
using System;
using System.Collections;

/// <summary>
/// Defines async callback action
/// </summary>
public class TrustContractManager : SingletonBehaviour<TrustContractManager>
{
    /// <summary>
    /// Sign the routine to fire <param name="onCompleted"> after it is done.</param>
    /// </summary>
    /// <param name="routine">The routine to execute.</param>
    /// <param name="onCompleted">The on completed action.</param>
    public void Sign(IEnumerator routine, Action onCompleted)
    {
        StartCoroutine(Run(routine, onCompleted));
    }

    /// <summary>
    /// Runs routine with action firing on execution.
    /// </summary>
    /// <param name="routine">The routine to execute.</param>
    /// <param name="onCompleted">The on completed action.</param>
    /// <returns>Returns yields.</returns>
    private IEnumerator Run(IEnumerator routine, Action onCompleted)
    {
        yield return routine;
        onCompleted();
    }
}
