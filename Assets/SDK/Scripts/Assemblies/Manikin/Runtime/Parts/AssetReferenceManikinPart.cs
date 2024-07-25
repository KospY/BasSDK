using System;
using Object = UnityEngine.Object;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// Simple class to restrict AssetRefernces to requiring a specific component, ie. ManikinPart.
    /// Surprisingly, this works will polymorphism.
    /// </summary>
    [System.Serializable]
    public class AssetReferenceManikinPart : AssetReference
    {
        public AssetReferenceManikinPart(string guid) : base(guid) { }

        //We don't need this if we want to return the gameobject and not the component.
        /*public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(position, Quaternion.identity, parent), GameObjectReady);
        }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
        }
        public AsyncOperationHandle<TComponent> LoadAssetAsync()
        {
            return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.LoadAssetAsync<GameObject>(), GameObjectReady);
        }

        AsyncOperationHandle<GameObject> GameObjectReady(AsyncOperationHandle<GameObject> arg)
        {
            //var comp = arg.Result.GetComponent<TComponent>();
            //return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(comp, string.Empty);
            return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(arg.Result, string.Empty);
        }*/

        public override bool ValidateAsset(Object obj)
        {
            var go = obj as GameObject;
            return go != null && go.GetComponent<ManikinPart>() != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            //this load can be expensive...
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && go.GetComponent<ManikinPart>() != null;
#else
            return false;
#endif
        }

        public void ReleaseInstance(AsyncOperationHandle<ManikinPart> op)
        {
            // Release the instance
            var component = op.Result as Component;
            if (component != null)
            {
                Addressables.ReleaseInstance(component.gameObject);
            }

            // Release the handle
            Addressables.Release(op);
        }
    }
}
