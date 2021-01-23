using General.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Runs in edit mode and creates new <see cref="StateSoundData"/> for every <see cref="State"/> on object.
/// </summary>
[ExecuteInEditMode]
public class RuntimeStateCompiler : MonoBehaviour
{
    /// <summary>
    /// Gets or sets child on which <see cref="StateSoundData"/> data will be applied. 
    /// </summary>
    private Transform _designChild { get; set; }

    /// <summary>
    /// Used to see which state sound data exists on child.
    /// </summary>
    [SerializeField]
    private List<StateSoundData> allSoundData;

    public void OnEnable()
    {
        _designChild = gameObject.transform.GetChild(0);
        allSoundData = _designChild.GetComponents<StateSoundData>().ToList();

        var soundDataToDestroy = allSoundData.Where(x => !x.LockFromDestroy || x.State == null).ToList();
        while (soundDataToDestroy.Count() > 0)
        {
            var soundToRemove = soundDataToDestroy.FirstOrDefault();
            soundDataToDestroy.Remove(soundToRemove);
            allSoundData.Remove(soundToRemove);
            DestroyImmediate(soundToRemove);
        }

        var allStates = gameObject.GetComponents<State>();
        foreach (var state in allStates)
        {
            if (allSoundData.FirstOrDefault(x => x.State == state) != null)
            {
                continue;
            }

            var soundComponent = _designChild.gameObject.AddComponent<StateSoundData>();
            soundComponent.State = state;
        }

        this.enabled = false;
    }
}
