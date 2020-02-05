using System.IO;
using UnityEngine;

namespace Utils.SaveTools
{
    public class JsonSaveTool
    {
        public static T Load<T>(string filename) where T : new()
        {
            string path = Path.Combine(Application.persistentDataPath, filename + ".txt");
            if (File.Exists(path))
            {
                using (StreamReader streamReader = File.OpenText(path))
                {
                    string jsonString = streamReader.ReadToEnd();
                    return JsonUtility.FromJson<T>(jsonString);
                }
            }

            return new T();
        }

        public static void Save<T>(T data, string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, filename + ".txt");
            string jsonString = JsonUtility.ToJson(data);
            using (StreamWriter streamWriter = File.CreateText(path))
            {
                streamWriter.Write(jsonString);
            }
        }
    }
}