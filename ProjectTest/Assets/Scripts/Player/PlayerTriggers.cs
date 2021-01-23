using Assets.Scripts.Core;
using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Example how to externally manipulate object states. 
    /// </summary>
    public class PlayerTriggers : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets injection of <see cref="IGameInformation"/> object instance.
        /// </summary>
        private IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Death"/> state reference.
        /// </summary>
        private Death deathState { get; set; }

        /// <summary>
        /// Gets or sets <see cref="StateController"/> object reference.
        /// </summary>
        private StateController stateController { get; set; }

        private void Start()
        {
            // Example of non attribute injection.
            gameInformation = DiContainerInitializor.Register<IGameInformation>();
            deathState = GetComponent<Death>();
            stateController = GetComponent<StateController>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == Const.TAG_POINT)
            {
                gameInformation.Score += Const.POINT_WORTH;
                UIManager.Inst.UpdateInGameScore(gameInformation.Score);
            }

            if (other.gameObject.tag == Const.TAG_COIN)
            {
                gameInformation.CoinCollected += Const.COIN_WORTH;

                //Don't destroy just disable since we are pooling this stuff.
                other.gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Const.TAG_KILLER)
            {
                stateController.SwapState(deathState);
            }
        }
    }
}
