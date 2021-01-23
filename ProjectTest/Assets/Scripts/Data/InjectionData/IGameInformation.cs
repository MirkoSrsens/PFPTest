using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public interface IGameInformation
    {
        /// <summary>
        /// Gets or sets instance of player on scene.
        /// </summary>
        GameObject Player { get; set; }

        /// <summary>
        /// Gets or sets camera instance inside of the scene.
        /// </summary>
        Camera Camera { get; set; }

        /// <summary>
        /// Gets or sets encypted value of score.
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// Gets or sets encypted value of high score.
        /// </summary>
        int CurrentHighScore { get; set; }

        /// <summary>
        /// Gets or sets numbers of coins collected.
        /// </summary>
        int CoinCollected { get; set; }
    }
}
