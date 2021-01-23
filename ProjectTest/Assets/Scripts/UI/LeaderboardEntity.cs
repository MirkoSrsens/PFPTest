using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LeaderboardEntity : MonoBehaviour
    {
        /// <summary>
        /// Defines text used to display placement number.
        /// </summary>
        [SerializeField]
        private Text _placement;

        /// <summary>
        /// Defines text used to display name of user.
        /// </summary>
        [SerializeField]
        private Text _name;

        /// <summary>
        /// Defines text used to display number of points achieved by user.
        /// </summary>
        [SerializeField]
        private Text _points;

        private void OnEnable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnLeaderboardRefresh += Despawn;
            }
        }

        private void OnDisable()
        {
            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnLeaderboardRefresh -= Despawn;
            }
        }

        /// <summary>
        /// Setups leaderboard entity with provided data.
        /// </summary>
        /// <param name="data">The leaderboard data entity that cointains user data.</param>
        public void Setup(LeaderboardData data)
        {
            _placement.text = data.Placement.ToString();
            _name.text = data.Name;
            _points.text = data.Value.ToString();

            transform.SetSiblingIndex(data.Placement);
        }

        /// <summary>
        /// Despawns element on refresh.
        /// </summary>
        /// <param name="sender">The sender. Usually <see cref="PlayfabManager"/>.</param>
        /// <param name="eventArgs">The event data.</param>
        private void Despawn(object sender, PlayfabRefreshLeaderboardsDataEventArgs eventArgs)
        {
            if(sender is PlayfabManager)
            {
                if (Pool.Inst != null)
                {
                    Pool.Inst.Despawn(this);
                }
            }
        }
    }
}
