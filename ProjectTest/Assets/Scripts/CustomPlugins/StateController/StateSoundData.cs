﻿using General.State;
using UnityEngine;

/// <summary>
/// Contains sound data for specific states.
/// </summary>
public class StateSoundData : MonoBehaviour
{
    /// <summary>
    /// Fmod sound sample.
    /// </summary>
    [FMODUnity.EventRef]
    public string FModInputSound;

    /// <summary>
    /// Gets name of the state.
    /// </summary>
    public string StateName { get { return State.GetType().Name; } }

    /// <summary>
    /// Gets or sets state related to this sound
    /// </summary>
    public State State;

    /// <summary>
    /// Protect data from reseting
    /// </summary>
    public bool LockFromDestroy;
}
