using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Used for injecting data into classes and game. Also allows different implementation
    /// depending on environment.
    /// </summary>
    public class InjectContainer : MonoBehaviour
    {
        private void Awake()
        {
            DiContainerInitializor.Container.BindInstance<IGameInformation, GameInformation>().AsSingle();
        }

        private void OnDestroy()
        {
            DiContainerInitializor.Container = new System.Collections.Generic.Dictionary<System.Type, ContainerData>();
        }
    }
}
