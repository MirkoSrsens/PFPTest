using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    public class ObstacleSpawner : State
    {
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        [SerializeField]
        private ObstacleShard _blockPrefab;

        [SerializeField]
        private ObstacleShard _startBlockPrefab;

        [SerializeField]
        private int _numberOfStartBlocks = 7;

        [SerializeField]
        private int _startOffsetFromPlayer = 5;

        private List<ObstacleShard> _activeBlocks;


        private float AvaragePosition { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            _activeBlocks = new List<ObstacleShard>();
            SpawnBlocks(10);

            CalculateAvarage();
        }

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

        public override void Update_State()
        {
            base.Update_State();

            if (gameInformation.Player.transform.position.x > AvaragePosition)
            {
                controller.SwapState(this);
            }
        }

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

        private void CalculateAvarage()
        {
            AvaragePosition = 0;
            foreach (var block in _activeBlocks)
            {
                AvaragePosition += block.transform.position.x;
            }

            AvaragePosition /= _activeBlocks.Count;
        }

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

        public override void OnExit_State()
        {
            base.OnExit_State();
            CalculateAvarage();
        }

    }
}
