using Assets.Scripts.Core;
using Assets.Scripts.Data.Events;
using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    /// <summary>
    /// Represents small part of map used by <see cref="ObstacleSpawner"/> to generate map.
    /// </summary>
    public class ObstacleShard : MonoBehaviour
    {
        /// <summary>
        /// Defines upper pipe that user needs to avoid.
        /// </summary>
        [SerializeField]
        private SpriteRenderer _pipeUp;

        /// <summary>
        /// Defines lower pipe that user needs to avoid.
        /// </summary>
        [SerializeField]
        private SpriteRenderer _pipeDown;

        /// <summary>
        /// Defines background parallax.
        /// </summary>
        [SerializeField]
        private SpriteRenderer _paralax;

        /// <summary>
        /// Defines coin prefab used to acquire soft currency.
        /// </summary>
        [SerializeField]
        private GameObject _coin;

        /// <summary>
        /// Defines minimal distance between 2 pipes.
        /// </summary>
        [SerializeField]
        private float _minDistance;

        /// <summary>
        /// Defines if pipes should be disabled (used in prefab variant for start block).
        /// </summary>
        [SerializeField]
        private bool disablePipes;

        /// <summary>
        /// Gets size of parallax used to determine spawning point of next shard.
        /// </summary>
        public float sizeOfParalax { get; private set; }

        /// <summary>
        /// Gets paralax position in world space.
        /// </summary>
        public float PositionOfParalax { get { return _paralax.transform.position.y; } }

        /// <summary>
        /// Checks if block is used in starting point. (Usually destroyed instead of pooled).
        /// </summary>
        public bool IsStartBlock { get { return disablePipes; } }

        /// <summary>
        /// Defines constant amount of coin drop chance. 
        /// </summary>
        private const int CoinDropChance = 25;

        /// <summary>
        /// Defines encrypted value of increased drop chance.
        /// </summary>
        private string _dropIncreaseChance;

        /// <summary>
        /// Gets or sets encrypted value of coin drop chance increase.
        /// </summary>
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

        /// <summary>
        /// Sets position of pipes so thats they are always on random position.
        /// </summary>
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

        /// <summary>
        /// Checks if coin should be spawned or not.
        /// </summary>
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

        /// <summary>
        /// On stats received modify coin drop chance and reroll them.
        /// </summary>
        /// <param name="sender">The sender object. Usually <see cref="PlayfabManager"/>.</param>
        /// <param name="eventArgs">The event data.</param>
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
