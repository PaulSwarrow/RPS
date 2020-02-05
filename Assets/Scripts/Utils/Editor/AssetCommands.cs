using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public class AssetCommands
    {
        public static T SaveAsset<T>(T ojb, string path, string name = null) where T : ScriptableObject
        {
            if (name == null)
            {
                name = typeof(T).Name;
            }

            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "New " + name + ".asset", "asset",
                "Enter a file name for the " + name, path);
            if (path == "") return null;
            AssetDatabase.CreateAsset(ojb, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(ojb);
            return ojb;
        }
    }
}