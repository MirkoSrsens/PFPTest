using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    public class ObstacleShard : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _pipeUp;

        [SerializeField]
        private SpriteRenderer _pipeDown;

        [SerializeField]
        private SpriteRenderer _paralax;

        [SerializeField]
        private GameObject _coin;

        public float sizeOfParalax { get; private set; }

        public float PositionOfParalax { get { return _paralax.transform.position.y; } }

        [SerializeField]
        private float _minDistance;

        [SerializeField]
        private bool disablePipes;

        public bool IsStartBlock { get { return disablePipes; } }

        private const int CoinDropChance = 25;

        private string _dropIncreaseChance;

        private int _dropIncreaseChanceSecure { get { return int.Parse(Security.Decrypt(_dropIncreaseChance)); } set { _dropIncreaseChance = Security.Encrypt(value.ToString()); } }

        private void Awake()
        {
            _dropIncreaseChanceSecure = 0;
            sizeOfParalax = _paralax.bounds.size.y;

            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData += OnStatsRecieved;
            }
        }

        private void OnEnable()
        {
            SetPositions();
        }

        private void OnDestroy()
        {
            SetPositions();

            if (PlayfabManager.Inst != null)
            {
                PlayfabManager.Inst.OnRefreshUserReadonlyData -= OnStatsRecieved;
            }
        }

        public void SetPositions()
        {
            if (!disablePipes)
            {
                var position = Random.Range(0, sizeOfParalax / 2);
                _pipeUp.transform.localPosition = new Vector2(0, (position + _pipeUp.bounds.size.y / 2) + _minDistance / 2);
                _pipeDown.transform.localPosition = new Vector2(0, (position - _pipeUp.bounds.size.y / 2) - _minDistance / 2);
            }
            else
            {
                _pipeDown.gameObject.SetActive(false);
                _pipeUp.gameObject.SetActive(false);
            }

            CanCoinDrop();
        }

        public void CanCoinDrop()
        {
            // No free coins you peasants. 
            if (disablePipes)
            {
                return;
            }

            var res = Random.Range(0, 100);

            if (res < CoinDropChance + _dropIncreaseChanceSecure)
            {
                _coin.SetActive(true);
            }
            else
            {
                _coin.SetActive(false);
            }
        }

        private void OnStatsRecieved(object sender, PlayfabUserReadonlyDataEventArgs eventArgs)
        {
            if(eventArgs.Data.ContainsKey("GoldMultiplier"))
            {
                _dropIncreaseChanceSecure = int.Parse(eventArgs.Data["GoldMultiplier"].Value);

                // Refresh all coin drop chances.
                CanCoinDrop();
            }
        }
    }
}
