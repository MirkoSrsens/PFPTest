using Assets.Scripts.Data.InjectionData;
using DiContainerLibrary.DiContainer;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class InjectContainer : MonoBehaviour
    {
        private void Awake()
        {
            DiContainerInitializor.Container.BindInstance<IGameInformation, GameInformation>().AsSingle();
        }
    }
}
