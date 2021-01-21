using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public class GameInformation : IGameInformation
    {
        public GameObject Player { get; set; }

        public Camera Camera { get; set; }

        private string _score;

        //Encrypt it so no outside manipulation cannot be easily performed, eg. cheat engine.
        // If it cannot be parsed back it will crash and that is expected behavior (dont mess with my code).
        public int Score { get { return int.Parse(Security.Decrypt(_score)); } set { _score = Security.Encrypt(value.ToString()); } }

        public GameInformation()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Camera = Camera.main;
            _score = Security.Encrypt(0.ToString());
        }
    }
}
