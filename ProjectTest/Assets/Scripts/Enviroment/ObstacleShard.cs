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

        public float sizeOfParalax { get; private set; }

        public float PositionOfParalax { get { return _paralax.transform.position.y; } }

        [SerializeField]
        private float _minDistance;

        [SerializeField]
        private bool disablePipes;

        public bool IsStartBlock { get { return disablePipes; } }

        private void Awake()
        {
            sizeOfParalax = _paralax.bounds.size.y;
        }

        private void OnEnable()
        {
            SetPositions();
        }

        public void SetPositions()
        {
            if (!disablePipes)
            {
                var position = UnityEngine.Random.Range(0, sizeOfParalax / 2);
                _pipeUp.transform.localPosition = new Vector2(0, (position + _pipeUp.bounds.size.y / 2) + _minDistance / 2);
                _pipeDown.transform.localPosition = new Vector2(0, (position - _pipeUp.bounds.size.y / 2) - _minDistance / 2);
            }
            else
            {
                _pipeDown.gameObject.SetActive(false);
                _pipeUp.gameObject.SetActive(false);
            }
        }
    }
}
