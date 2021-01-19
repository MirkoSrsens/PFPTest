using System;
using System.Collections.Generic;

namespace DiContainerLibrary.DiContainer
{
    public static partial class DiContainerInitializor
    {
        public delegate KeyValuePair<Type, ContainerData> BindCommand<TInterface, TClass>(Dictionary<Type, ContainerData> data);

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

        public static void AsSingle(this KeyValuePair<Type, ContainerData> container)
        {
            container.Value.IsStatic = true;
        }

        public static void AsTransient(this KeyValuePair<Type, ContainerData> container)
        {
            container.Value.IsStatic = false;
        }

        public static Dictionary<Type, ContainerData> Container = new Dictionary<Type, ContainerData>();
    }
}
