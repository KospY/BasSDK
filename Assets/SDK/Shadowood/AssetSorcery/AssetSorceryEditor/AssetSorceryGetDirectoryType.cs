namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryGetDirectoryType
    {
        public enum eDirectoryType
        {
            WriteableAssetFolder = 0,
            ReadOnlyPackageCache = 1,
            WriteablePackage = 2
        }

        public static eDirectoryType GetDirectoryType(string path)
        {
            if (UnityEditor.PackageManager.PackageInfo.FindForAssetPath(path) == null)
            {
                return eDirectoryType.WriteableAssetFolder;
            }

            if (UnityEditor.PackageManager.PackageInfo.FindForAssetPath(path).source == UnityEditor.PackageManager.PackageSource.Embedded)
            {
                return eDirectoryType.WriteablePackage;
            }
            else
            {
                return eDirectoryType.ReadOnlyPackageCache;
            }
        }
    }
}
