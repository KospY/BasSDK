using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BuildSettings : ScriptableObject
    {

        public enum SingleContentFlag
        {
            Blood = 0,
            Burns = 1,
            Dismemberment = 2,
            Desecration = 3, // Makes bodies despawn after 3 seconds to prevent "misuse" of dead bodies
            Skeleton = 4,
            Spider = 5,
            Insect = 6,
            Snake = 7,
            Bird = 8,
            Fright = 9, // This should be a catch-all for anything we'd consider to be a "jumpscare" or otherwise potentially scary
            SelfHarm = 10, // For anything where the player may harm themselves
        }

        [System.Flags]
        public enum ContentFlag
        {
            None = 0,
            Blood = (1 << SingleContentFlag.Blood),
            Burns = (1 << SingleContentFlag.Burns),
            Dismemberment = (1 << SingleContentFlag.Dismemberment),
            Desecration = (1 << SingleContentFlag.Desecration), // Makes bodies despawn after 3 seconds to prevent "misuse" of dead bodies
            Skeleton = (1 << SingleContentFlag.Skeleton),
            Spider = (1 << SingleContentFlag.Spider),
            Insect = (1 << SingleContentFlag.Insect),
            Snake = (1 << SingleContentFlag.Snake),
            Bird = (1 << SingleContentFlag.Bird),
            Fright = (1 << SingleContentFlag.Fright), // This should be a catch-all for anything we'd consider to be a "jumpscare" or otherwise potentially scary
            SelfHarm = (1 << SingleContentFlag.SelfHarm),  // For anything where the player may harm themselves
        }

        public enum ContentFlagBehaviour
        {
            Discard = 0, // Remove the content only if their sensitive content flags matches
            Keep = 1 // Use the content only if their sensitive content flags matches. Acts as a replacement
        }
        
        /// <summary>
        ///   <para>Scripting implementation (backend).</para>
        /// </summary>
        public enum ScriptingImplementation
        {
            /// <summary>
            ///   <para>The standard Mono 2.6 runtime.</para>
            /// </summary>
            Mono2x,
            /// <summary>
            ///   <para>Unity's .NET runtime.</para>
            /// </summary>
            IL2CPP,
            /// <summary>
            ///   <para>Microsoft's .NET runtime.</para>
            /// </summary>
            WinRTDotNET,
        }
    }
}