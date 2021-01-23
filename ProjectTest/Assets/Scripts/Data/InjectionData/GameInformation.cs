using System;
using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public class GameInformation : IGameInformation
    {
        /// </inheridoc>
        public GameObject Player { get; set; }

        /// </inheridoc>
        public Camera Camera { get; set; }

        private string _score;

        private string _currentHighScore;

        private string _coinCollected;

        //Encrypt it so no outside manipulation cannot be easily performed, eg. cheat engine.
        // If it cannot be parsed back it will crash and that is expected behavior (dont mess with my code).

        /// </inheridoc>
        public int Score { get { return int.Parse(Security.Decrypt(_score)); } set { _score = Security.Encrypt(value.ToString()); } }

        /// </inheridoc>
        public int CoinCollected { get { return int.Parse(Security.Decrypt(_coinCollected)); } set { _coinCollected = Security.Encrypt(value.ToString()); } }

        /// </inheridoc>
        public int CurrentHighScore { get { return int.Parse(Security.Decrypt(_currentHighScore)); } set { _currentHighScore = Security.Encrypt(value.ToString()); } }

        /// <summary>
        /// Creates new instance of <see cref="GameInformation"/>, defining all properties accordingly.
        /// </summary>
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

        /// <summary>
        /// Assigns highscore value to property <see cref="CurrentHighScore"/> allowing us to track down and compare user data.
        /// </summary>
        /// <param name="sender">The object that fires event. Usually <see cref="PlayfabManager"/.</param>
        /// <param name="eventArgs">The event data.</param>
        private void AssignOldHighScore(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if(eventArgs.Data.ContainsKey(Const.STATISTIC_NAME))
            {
                CurrentHighScore = int.Parse(eventArgs.Data[Const.STATISTIC_NAME].Value);
            }
        }
    }
}
