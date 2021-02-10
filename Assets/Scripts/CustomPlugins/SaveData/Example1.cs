using System;

namespace SaveData
{
    /// <summary>
    /// Holds implementation of <see cref="ISlotData"/>.
    /// </summary>
    [Serializable]
    public class SlotData : ISlotData
    {
        /// <inheritdoc />
        public int CurrentCapacity { get; set; }

        /// <inheritdoc />
        public string ItemsResource { get; set; }

        /// <inheritdoc />
        public string Id { get; set; }
    }
}
