using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveData
{
    public static class SaveAndLoadData<TData> where TData : class, IUniqueIndex
    {
        private static IList<TData> dataContext = new List<TData>();

        public static TData LoadSpecificData(string id)
        {
            if (!File.Exists(GetAttribute()))
            {
                return null;
            }

            using (var stream = File.OpenRead(GetAttribute()))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var result = (IList<TData>)formatter.Deserialize(stream);

                return result.FirstOrDefault(x => x.Id == id);
            }
        }

        public static IList<TData> LoadAllData() 
        {
            if(!File.Exists(GetAttribute()))
            {
                return dataContext;
            }

            using (var stream = File.OpenRead(GetAttribute()))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (IList<TData>)formatter.Deserialize(stream);
            }
        }

        public static void SaveData(TData data)
        {
            dataContext = LoadAllData();

            var exist = dataContext.FirstOrDefault(x => x.Id == data.Id);

            if (exist != null)
            {
                dataContext.Remove(exist);
            }

            dataContext.Add(data);
            using (var stream = File.OpenWrite(GetAttribute()))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, dataContext);
            }
        }

        private static string GetAttribute()
        {
            var dbAttribute = typeof(TData).GetCustomAttributes(typeof(DbContextConfiguration), true).FirstOrDefault() as DbContextConfiguration;
            if (dbAttribute != null)
            {
                return dbAttribute.FilePath;
            }

            throw new FileNotFoundException(string.Concat("Could not load attribute from the class"));
        }
    }
}
