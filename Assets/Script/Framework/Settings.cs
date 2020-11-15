using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 指定游戏所需的各个路径
    /// </summary>
    public class Paths
    {
        public static char Div = '\\';   // 默认路径分隔符

        public static string DocumentPath;
        public static string BundleBasePath;
        public static string LogPath;

        public static string DllBundle;
        public static string PrefabBundle;
        public static string UIBundle;
        public static string ValuesBundle;
        public static string DatabaseBundle;
        public static string LuaBundle;
        public static string Normal = "normal";

        static Paths()
        {
            LoadFromXml(Config.ConfigXmlPath);

            if (!Directory.Exists(DocumentPath))
                Directory.CreateDirectory(DocumentPath);
        }

        private static void LoadFromXml(string path)
        {
            var root = XDocument.Load(path).Root;
            BundleBasePath = root.Element(nameof(BundleBasePath)).Attribute("path").Value.Replace(Div,Path.PathSeparator);
            LogPath = root.Element(nameof(LogPath)).Attribute("path").Value.Replace(Div, Path.PathSeparator);

            var t = typeof(Path);
            foreach (var e in root.Element("SysBundleName").Elements())
                t.GetField(e.Name.ToString(), BindingFlags.Static | BindingFlags.Public)
                    .SetValue(null,e.Attribute("name").Value);

            var platform = "";
#if UNITY_STANDALONE || UNITY_EDITOR
            DocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            platform = "Standalone";
#elif UNITY_ANDROID
            DocumentPath = Application.persistentDataPath;
            platform = "Android";
#else
            Debug.LogError("不支持当前平台");
#endif
            var docRelativePath = root.Element("DocumentRelativePath").Element(platform).Attribute("path").Value;
            DocumentPath = DocumentPath.PCombine(docRelativePath);
        }
    }
}
