using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public class GameInformation : IGameInformation
    {
        public GameObject Player { get; set; }

        public Camera Camera { get; set; }

        public GameInformation()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Camera = Camera.main;
        }
    }
}
