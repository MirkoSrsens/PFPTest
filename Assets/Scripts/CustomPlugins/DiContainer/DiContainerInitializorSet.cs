using System;
using System.Collections.Generic;

namespace DiContainerLibrary.DiContainer
{
    /// <summary>
    /// Contains login for injecting data in container. 
    /// </summary>
    public static partial class DiContainerInitializor
    {
        public delegate KeyValuePair<Type, ContainerData> BindCommand<TInterface, TClass>(Dictionary<Type, ContainerData> data);

        /// <summary>
        /// Bind interface to a class.
        /// </summary>
        /// <typeparam name="TInterface">Interface to inject.</typeparam>
        /// <typeparam name="TClass">Object class which will be injected.</typeparam>
        /// <param name="data">The container in which injection will be stored.</param>
        /// <returns>Returns <paramref name="data"/></returns>
        public static KeyValuePair<Type, ContainerData> BindInstance<TInterface, TClass>(this Dictionary<Type, ContainerData> data)
            where TClass : TInterface , new()
        {
            var result = new ContainerData()
            {
                dataType = typeof(TClass),
                IsStatic = false,
            };

            var keyValuePair = new KeyValuePair<Type, ContainerData>(typeof(TInterface), result);
            data.Add(keyValuePair.Key, keyValuePair.Value);

            return keyValuePair;
        }

        /// <summary>
        /// Only use one instance for every request. Singleton.
        /// </summary>
        /// <param name="container"></param>
        public static void AsSingle(this KeyValuePair<Type, ContainerData> container)
        {
            container.Value.IsStatic = true;
        }

        /// <summary>
        /// Provide new instance every time.
        /// </summary>
        /// <param name="container"></param>
        public static void AsTransient(this KeyValuePair<Type, ContainerData> container)
        {
            container.Value.IsStatic = false;
        }

        /// <summary>
        /// Container that holds data.
        /// </summary>
        public static Dictionary<Type, ContainerData> Container = new Dictionary<Type, ContainerData>();
    }
}
