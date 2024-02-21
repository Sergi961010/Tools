using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using System.Net.Http;
using System.IO;

namespace sergiborras
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders() 
        {
            Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "Scripts/ScriptableObjects", "Scripts/UI");
            Refresh();
        }
        [MenuItem("Tools/Setup/Load New Manifest")]
        static async void LoadNewManifest() => await Packages.ReplacePackagesFromGist("1417a5af27d5648b3981be1735723af5");
        [MenuItem("Tools/Setup/Import My Favorite Assets")]
        public static void ImportMyFavoriteAssets()
        {
            Assets.ImportAsset("Editor Enhancers Bundle.unitypackage", "kubacho lab/Editor ExtensionsUtilities");
        }
    }
    static class Folders 
    {
        public static void CreateDefault(string root, params string[] folders)
        {
            var fullpath = Combine(Application.dataPath, root);
            if (!Exists(fullpath))
            {
                CreateDirectory(fullpath);
            }
            foreach (var folder in folders)
            {
                CreateSubFolders(fullpath, folder);
            }
        }
        private static void CreateSubFolders(string rootPath, string folderHierarchy)
        {
            var folders = folderHierarchy.Split('/');
            var currentPath = rootPath;
            foreach (var folder in folders)
            {
                currentPath = Combine(currentPath, folder);
                if (!Exists(currentPath))
                {
                    CreateDirectory(currentPath);
                }
            }
        }
    }
    static class Packages
    {
        public static async Task ReplacePackagesFromGist(string id, string user = "Sergi961010")
        {
            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }
        static string GetGistUrl(string id, string user = "Sergi961010") =>
            $"https://gist.githubusercontent.com/{user}/{id}/raw";
        static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        static void ReplacePackageFile(string contents)
        {
            var existing = Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            Client.Resolve();
        }
        public static void InstallUnityPackage(string name)
        {
            Client.Add($"com.unity.{name}");
        }
    }
    static class Assets
    {
        public static void ImportAsset(string asset, string subfolder, string rootFolder = "C:/Users/Sergi/AppData/Roaming/Unity/Asset Store-5.x")
        {
            ImportPackage(Combine(rootFolder, subfolder, asset), false);
        }
    }
}