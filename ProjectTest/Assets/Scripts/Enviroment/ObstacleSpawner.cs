using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    /// <summary>
    /// Used to spawn obstacle shards.
    /// </summary>
    public class ObstacleSpawner : State
    {
        /// <summary>
        /// Gets or sets injected <see cref="GameInformation"/> data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Defines obstacle shard prefab.
        /// </summary>
        [SerializeField]
        private ObstacleShard _blockPrefab;

        /// <summary>
        /// Defines start block prefab.
        /// </summary>
        [SerializeField]
        private ObstacleShard _startBlockPrefab;

        /// <summary>
        /// Defines number of start blocks.
        /// </summary>
        [SerializeField]
        private int _numberOfStartBlocks = 7;

        /// <summary>
        /// Defines start offset from player (in backwards position).
        /// </summary>
        [SerializeField]
        private int _startOffsetFromPlayer = 5;

        /// <summary>
        /// Gets or sets list of all active block shards.
        /// </summary>
        private List<ObstacleShard> _activeBlocks { get; set; }

        /// <summary>
        /// Gets or sets average position of all active blocks (used to determine when to change blocks positions;
        /// </summary>
        private float AvaragePosition { get; set; }

        /// <inheritdoc/>
        protected override void Initialization_State()
        {
            base.Initialization_State();
            _activeBlocks = new List<ObstacleShard>();
            SpawnBlocks(10);

            AvaragePosition = CalculateAvarage();
        }

        /// <inheritdoc/>
        public override void OnEnter_State()
        {
            base.OnEnter_State();

            // Some kind of object pooling but very primitive.
            for (int i=0; i< 2; i++)
            {
                //find block with lowest X position
                var selectedBlock = default(ObstacleShard);
                foreach(var block in _activeBlocks)
                {
                    if(selectedBlock == null)
                    {
                        selectedBlock = block;
                    }
                    else if(selectedBlock.transform.position.x > block.transform.position.x)
                    {
                        selectedBlock = block;
                    }
                }


                if (selectedBlock.IsStartBlock)
                {
                    _activeBlocks.Remove(selectedBlock);
                    Destroy(selectedBlock.gameObject);
                }
                else
                {
                    var lastInLine = GetFurthestPosition();
                    selectedBlock.gameObject.SetActive(false);
                    selectedBlock.transform.position = lastInLine.transform.position + new Vector3(lastInLine.sizeOfParalax / 2, 0, 0);
                    selectedBlock.gameObject.SetActive(true);
                }
            }
            controller.EndState(this);
        }

        /// <inheritdoc/>
        public override void Update_State()
        {
            base.Update_State();

            if (gameInformation.Player.transform.position.x > AvaragePosition)
            {
                controller.SwapState(this);
            }
        }

        /// <summary>
        /// Spawns specific number of blocks.
        /// </summary>
        /// <param name="numberOfBlocks">Number of blocks to be spawned.</param>
        private void SpawnBlocks(int numberOfBlocks)
        {
            var lastInLine = default(ObstacleShard);
            if (_activeBlocks.Count > 0)
            {
                lastInLine = _activeBlocks[0];
            }
            else
            {
                lastInLine = Instantiate(_startBlockPrefab, new Vector3(gameInformation.Player.transform.position.x - _startOffsetFromPlayer, 0, 0), Quaternion.identity, transform);
                _activeBlocks.Add(lastInLine);

                // Adjust camera to center of screen so there is no clippings of environment (blue color).
                gameInformation.Camera.transform.position = new Vector3(gameInformation.Camera.transform.position.x, lastInLine.PositionOfParalax, gameInformation.Camera.transform.position.z);

                // Spawn few empty parallaxes so user dont hit pipe instantly
                for (int i = 0; i < _numberOfStartBlocks; i++)
                {
                    lastInLine = Instantiate(_startBlockPrefab, lastInLine.transform.position + new Vector3(lastInLine.sizeOfParalax / 2, 0, 0), Quaternion.identity, transform);
                    _activeBlocks.Add(lastInLine);
                }

            }
            
            for(int i =0; i< numberOfBlocks; i++)
            {
                lastInLine = Instantiate(_blockPrefab, lastInLine.transform.position + new Vector3(lastInLine.sizeOfParalax / 2, 0, 0), Quaternion.identity, transform);
                _activeBlocks.Add(lastInLine);
            }
        }

        /// <summary>
        /// Calculates average position on x axis.
        /// </summary>
        private float CalculateAvarage()
        {
            var result = 0f;
            foreach (var block in _activeBlocks)
            {
                result += block.transform.position.x;
            }

            return result/_activeBlocks.Count;
        }

        /// <summary>
        /// Gets the block with furthest position.
        /// </summary>
        /// <returns>Returns furthes obstacle shard.</returns>
        private ObstacleShard GetFurthestPosition()
        {
            var result = default(ObstacleShard);
            foreach(var block in _activeBlocks)
            {
                if(result == null || block.transform.position.x > result.transform.position.x)
                {
                    result = block;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public override void OnExit_State()
        {
            base.OnExit_State();
            AvaragePosition = CalculateAvarage();
        }

    }
}
