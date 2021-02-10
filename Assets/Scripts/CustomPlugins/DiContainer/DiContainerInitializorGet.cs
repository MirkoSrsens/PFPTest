using System;
using System.Linq;
using System.Reflection;

namespace DiContainerLibrary.DiContainer
{
    public static partial class DiContainerInitializor
    {
        /// <summary>
        /// Registers object using interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface for which we are looking matching object implementation.</typeparam>
        /// <returns>Instance of object that implements <typeparam name="TInterface"/></returns>
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

        /// <summary>
        /// Registers object as a whole taking attribute <see cref="InjectDiContainter"/> from properties and injecting
        /// matching objects to them.
        /// </summary>
        /// <param name="objectToinitialize">Object to initialize.</param>
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

        /// <summary>
        /// Registers object using type
        /// </summary>
        /// <param name="data">The type of object we want to inject.</param>
        /// <returns>Object of type <paramref name="data"/></returns>
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
