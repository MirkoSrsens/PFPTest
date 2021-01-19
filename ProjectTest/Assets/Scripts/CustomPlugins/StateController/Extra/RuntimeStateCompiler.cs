using General.State;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class RuntimeStateCompiler : MonoBehaviour
{
    // Update is called once per frame

    Transform designChild { get; set; }
    public List<StateSoundData> allSoundData;

    public void OnEnable()
    {
        designChild = gameObject.transform.GetChild(0);
        allSoundData = designChild.GetComponents<StateSoundData>().ToList();

        var soundDataToDestroy = allSoundData.Where(x => !x.LockFromDestroy || x.State == null).ToList();
        while (soundDataToDestroy.Count() > 0)
        {
            var soundToRemove = soundDataToDestroy.FirstOrDefault();
            soundDataToDestroy.Remove(soundToRemove);
            allSoundData.Remove(soundToRemove);
            DestroyImmediate(soundToRemove);
        }

        var allStates = gameObject.GetComponent<StateManager>().AllStates;
        foreach (var state in allStates)
        {
            if (allSoundData.FirstOrDefault(x => x.State == state) != null)
            {
                continue;
            }

            var soundComponent = designChild.gameObject.AddComponent<StateSoundData>();
            soundComponent.State = state;
        }

        this.enabled = false;
    }
}
