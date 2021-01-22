using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class SingletonBehaviour<T> : MonoBehaviour where T: UnityEngine.Object
    {
        private static T _inst;
        public static T Inst
        {
            get
            {
                if(_inst == null)
                {
                    _inst = GameObject.FindObjectOfType<T>();
                    //Debug.LogWarning(string.Format("Object of type {0} was null assigning it over FindObjectOfType \n performance might suffer", typeof(T)));
                }

                return _inst;
            }
        }
    }
}
