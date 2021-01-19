using System;

namespace DiContainerLibrary.DiContainer
{
    public class ContainerData
    {
        /// <summary>
        /// Gets or sets data type.
        /// </summary>
        public Type dataType { get; set; }

        /// <summary>
        /// Gets or sets actual value.
        /// </summary>
        public object actualValue { get; set; }

        /// <summary>
        /// gets or sets is property static.
        /// </summary>
        public bool IsStatic { get; set; }
    }
}
