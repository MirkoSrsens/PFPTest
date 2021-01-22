using System;
using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public class GameInformation : IGameInformation
    {
        public GameObject Player { get; set; }

        public Camera Camera { get; set; }

        private string _score;

        private string _currentHighScore;

        private string _coinCollected;

        //Encrypt it so no outside manipulation cannot be easily performed, eg. cheat engine.
        // If it cannot be parsed back it will crash and that is expected behavior (dont mess with my code).
        public int Score { get { return int.Parse(Security.Decrypt(_score)); } set { _score = Security.Encrypt(value.ToString()); } }

        public int CoinCollected { get { return int.Parse(Security.Decrypt(_coinCollected)); } set { _coinCollected = Security.Encrypt(value.ToString()); } }

        public int CurrentHighScore { get { return int.Parse(Security.Decrypt(_currentHighScore)); } set { _currentHighScore = Security.Encrypt(value.ToString()); } }

        public GameInformation()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Camera = Camera.main;
            Score = 0;
            CoinCollected = 0;

            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData += AssignOldHighScore;
            }
        }

        private void AssignOldHighScore(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if(eventArgs.Data.ContainsKey("Highscore"))
            {
                CurrentHighScore = int.Parse(eventArgs.Data["Highscore"].Value);
            }
        }
    }
}
