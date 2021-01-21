using Assets.Scripts.Core;
using Assets.Scripts.CustomPlugins.Utility;
using Assets.Scripts.Data.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LeaderboardEntity : MonoBehaviour
    {
        [SerializeField]
        private Text placement;

        [SerializeField]
        private Text name;

        [SerializeField]
        private Text points;

        private void OnEnable()
        {
            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnLeaderboardRefresh += nDestroy;
            }
        }

        private void OnDisable()
        {
            if(PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnLeaderboardRefresh += nDestroy;
            }
        }

        public void Setup(LeaderboardData data)
        {
            // Its zero based.
            placement.text = (++data.Placement).ToString();
            name.text = data.Name;
            points.text = data.Value.ToString();

            transform.SetSiblingIndex(data.Placement);
        }

        private void nDestroy(object sender, PlayfabRefreshLeaderboardsDataEventArgs eventArgs)
        {
            if(sender is PlayfabManager)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
