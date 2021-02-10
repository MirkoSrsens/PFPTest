using System.Collections.Generic;

namespace SaveData
{
    [DbContextConfiguration("SlotData")]
    public interface ISlotData : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets slot capacity. <code>null if slot is empty.</code>
        /// </summary>
        int CurrentCapacity { get; set; }

        /// <summary>
        /// Gets or sets items in slot.
        /// </summary>
        string ItemsResource { get; set; }
    }
}
