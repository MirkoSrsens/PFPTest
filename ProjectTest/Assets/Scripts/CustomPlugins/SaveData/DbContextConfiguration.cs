using System;

namespace SaveData
{
    public class DbContextConfiguration : Attribute
    {
        /// <summary>
        /// Holds definition of file path.
        /// </summary>
        public string FilePath { get; set; }

        public DbContextConfiguration(string filePath)
        {
            this.FilePath = string.Concat(filePath,".bin");
        }
    }
}
