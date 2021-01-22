using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public interface IGameInformation
    {
        GameObject Player { get; set; }

        Camera Camera { get; set; }

        int Score { get; set; }

        int CurrentHighScore { get; set; }

        int CoinCollected { get; set; }
    }
}
