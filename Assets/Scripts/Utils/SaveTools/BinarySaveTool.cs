using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utils.SaveTools
{
    public class BinarySaveTool
    {
        public static T Load<T> (string filename) where T : new()
        {
            string path = Path.Combine(Application.persistentDataPath, filename + ".dat");
            if(File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream fileStream = File.Open (path, FileMode.Open))
                {
                   return (T)binaryFormatter.Deserialize (fileStream);
                }
                

            }
            else
            {
                return new T();
            }
        }
        
        public static void Save<T>( T data, string filename)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath,  filename + ".dat");
            using (FileStream fileStream = File.Open (path, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize (fileStream, data);
            }
        }
    }
}