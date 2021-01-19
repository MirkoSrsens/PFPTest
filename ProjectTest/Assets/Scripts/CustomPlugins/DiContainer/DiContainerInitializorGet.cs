using System;
using System.Linq;
using System.Reflection;

namespace DiContainerLibrary.DiContainer
{
    public static partial class DiContainerInitializor
    {
        public static TInterface Register<TInterface>()
           where TInterface : class
        {
            var containerData = Container[typeof(TInterface)];
            var instance = Activator.CreateInstance(containerData.dataType);
            if (containerData.IsStatic)
            {
                if (containerData.actualValue == null)
                {
                    containerData.actualValue = instance;
                }
                return (TInterface)containerData.actualValue;
            }

            var result = Activator.CreateInstance(containerData.dataType);
            return (TInterface)result;
        }

        public static void RegisterObject(object objectToinitialize)
        {
            var properties = objectToinitialize.GetType()
                .GetProperties(
                BindingFlags.NonPublic | BindingFlags.Public 
                | BindingFlags.Static | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(typeof(InjectDiContainter), false)).ToList();

            foreach (var property in properties)
            {
                var getType = property.PropertyType;
                property.SetValue(objectToinitialize, getType.Register(), null);
            }
        }

        private static object Register(this Type data)
        {
            var containerData = Container[data];
            var instance = Activator.CreateInstance(containerData.dataType);
            if (containerData.IsStatic)
            {
                if (containerData.actualValue == null)
                {
                    containerData.actualValue = instance;
                }
                return containerData.actualValue;
            }

            var result = Activator.CreateInstance(containerData.dataType);
            return result;
        }
    }
}
