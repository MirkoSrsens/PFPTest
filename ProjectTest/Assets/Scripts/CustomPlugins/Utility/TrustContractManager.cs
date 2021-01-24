using Assets.Scripts.Core;
using System;
using System.Collections;
using UnityEngine;

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
    public Coroutine Sign(IEnumerator routine, Action onCompleted)
    {
        return StartCoroutine(Run(routine, onCompleted));
    }

    /// <summary>
    /// Sign iterating calls that will loop until canceled
    /// </summary>
    /// <param name="onCompleted"></param>
    /// <param name="loopTime"></param>
    public Coroutine Sign(Action onCompleted, int loopTime)
    {
        return StartCoroutine(Run(onCompleted, loopTime));
    }

    /// <summary>
    /// Stops coroutine that is running
    /// </summary>
    /// <param name="routine">The routine to stop</param>
    public void BreakeContract(Coroutine routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
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

    /// <summary>
    /// Runs routine with action firing on execution.
    /// </summary>
    /// <param name="routine">The routine to execute.</param>
    /// <param name="onCompleted">The on completed action.</param>
    /// <returns>Returns yields.</returns>
    private IEnumerator Run(Action onCompleted, int loopTime)
    {
        while (true)
        {
            onCompleted();
            yield return new WaitForSeconds(loopTime);
        }
    }
}
