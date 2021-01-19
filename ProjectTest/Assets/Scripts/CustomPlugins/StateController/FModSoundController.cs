using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;

namespace General.State
{
    public class FModSoundController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets
        /// </summary>
        private Dictionary<string, EventInstance> stateSoundData { get; set; }

        /// <summary>
        /// Gets ot sets all sound data from object.
        /// </summary>
        private List<StateSoundData> soundData { get; set; }

        private void Awake()
        {
            soundData = GetComponentsInChildren<StateSoundData>().ToList();
            stateSoundData = new Dictionary<string, EventInstance>();
        }

        /// <summary>
        /// Plays the sound with given key.
        /// </summary>
        /// <param name="key">Key of the audio clips from <see cref="ISoundData"/></param>
        public void PlaySound(string stateName)
        {
            var selectedSound = soundData.FirstOrDefault(x => x.StateName == stateName);

            if (selectedSound == null || string.IsNullOrEmpty(selectedSound.FModInputSound)) return;

            if (!stateSoundData.ContainsKey(stateName))
            {
                stateSoundData.Add(stateName, FMODUnity.RuntimeManager.CreateInstance(selectedSound.FModInputSound));
            }

            stateSoundData[stateName].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            stateSoundData[stateName].start();
        }

        /// <summary>
        /// Stops the selected Fmod sound.
        /// </summary>
        /// <param name="stateName">The name of the state to stop.</param>
        /// <param name="stopMode">The mode on which it will be stoped.</param>
        public void StopSound(string stateName, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
        {
            var selectedSound = soundData.FirstOrDefault(x => x.StateName == stateName);

            if (selectedSound == null || string.IsNullOrEmpty(selectedSound.FModInputSound)) return;

            stateSoundData[stateName].stop(stopMode);
        }

        /// <summary>
        /// Sets specific parameter in fmod studio.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <param name="paramterName">The parameter name.</param>
        /// <param name="numericValue">The numeric value (can only be float).</param>
        public void SetParameter(string stateName, string paramterName, float numericValue)
        {
            var selectedSound = soundData.FirstOrDefault(x => x.StateName == stateName);

            if (selectedSound == null || string.IsNullOrEmpty(selectedSound.FModInputSound)) return;

            if (!stateSoundData.ContainsKey(stateName))
            {
                return;
            }

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(paramterName, numericValue);             
        }
    }
}
