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

#if UNITY_ANDROID && !UNITY_EDITOR
            DiContainerInitializor.Container.BindInstance<IInputController, MobileInputController>().AsSingle();
#else
            DiContainerInitializor.Container.BindInstance<IInputController, PCInputController>().AsSingle();
#endif
        }

        private void OnDestroy()
        {
            DiContainerInitializor.Container = new System.Collections.Generic.Dictionary<System.Type, ContainerData>();
        }
    }
}
