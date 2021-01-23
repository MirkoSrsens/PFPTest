using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptable
{
    /// <summary>
    /// Contains intro shard data (shard meaning 1 peace). 
    /// </summary>
    [Serializable]
    public struct IntroShardData
    {
        public GameObject SplashImage;

        /// <summary>
        /// Total duration of image on screen.
        /// </summary>
        public float Duration;

        /// <summary>
        /// Fmod sound sample to play.
        /// </summary>
        [FMODUnity.EventRef]
        public string FModInputSound;
    }

    /// <summary>
    /// Defines scriptable object used for defining sequence of logos displayed on intro.
    /// </summary>
    [CreateAssetMenu(fileName = "IntroSequence", menuName = "ScriptableObjects/CreateNewIntroSequence", order = 1)]
    public class IntroSequence : ScriptableObject
    {
        public List<IntroShardData> SequenceOfObjects;
    }
}
