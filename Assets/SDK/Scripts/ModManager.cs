using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Text.RegularExpressions;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public static class ModManager
    {
        public const string debugLine = "[ModManager]";
        public static string modFolderName = "Mods";
        public static bool editorLoadAddressableBundles = false;

        public static bool modCatalogAddressablesLoaded;

        public static readonly HashSet<ModData> loadedMods = new HashSet<ModData>();
        public static bool gameModsLoaded;


        public delegate void ModLoadEvent(EventTime eventTime, ModLoadEventType eventType, ModData modData = null);
        public static event ModLoadEvent OnModLoad;

#pragma warning disable CS0067 // Not used
        public static event ModLoadEvent OnModUnload;
#pragma warning restore CS0067

        public static bool isGameModsCatalogRefreshed;

        public enum ModLoadEventType
        {
            ModManager,
            Assembly,
            Addressable,
            Catalog,
            ModOption,
            ThunderScript,
            AddressableAsset
        }


        public class ModData
        {
            public string Name;
            public string Description;
            public string Author;
            public string ModVersion;
            public string GameVersion;
            public string Thumbnail;

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public string folderName;

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public string fullPath;

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public bool Incompatible;
#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public HashSet<Error> errors = new HashSet<Error>();


            public class Error
            {
                public ErrorType type;
                public string description;
                public string innerMessage;
                public string filePath;

                public Error(ErrorType type, string description, string innerMessage, string filePath)
                {
                    this.type = type;
                    this.description = description;
                    this.innerMessage = innerMessage;
                    this.filePath = filePath;
                }

                #region Equality

                protected bool Equals(Error other)
                {
                    return type == other.type
                           && description == other.description
                           && innerMessage == other.innerMessage
                           && filePath == other.filePath;
                }
                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj))
                        return false;
                    if (ReferenceEquals(this, obj))
                        return true;
                    if (obj.GetType() != this.GetType())
                        return false;
                    return Equals((Error)obj);
                }
                public override int GetHashCode()
                {
                    return HashCode.Combine((int)type, description, innerMessage, filePath);
                }
                public static bool operator ==(Error left, Error right)
                {
                    return Equals(left, right);
                }
                public static bool operator !=(Error left, Error right)
                {
                    return !Equals(left, right);
                }

                #endregion
            }

            public enum ErrorType
            {
                Json,
                Catalog,
                Assembly,
                Manifest,
                Option,
                ThunderScript
            }

            protected bool Equals(ModData other)
            {
                return Name == other.Name
                       && Description == other.Description
                       && Author == other.Author
                       && ModVersion == other.ModVersion
                       && GameVersion == other.GameVersion
                       && Thumbnail == other.Thumbnail;
            }
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != this.GetType())
                    return false;
                return Equals((ModData)obj);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Name, Description, Author, ModVersion, GameVersion, Thumbnail);
            }
            public static bool operator ==(ModData left, ModData right)
            {
                return Equals(left, right);
            }
            public static bool operator !=(ModData left, ModData right)
            {
                return !Equals(left, right);
            }
        }

        public class LoadOrder
        {
            public List<string> modNames;
        }

    }
}
