using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ThunderRoad
{
    public static class AddressLocationCache
    {
        private static Dictionary<AddressKey, IResourceLocation> addressableLocations = new Dictionary<AddressKey, IResourceLocation>();

        public static void Clear()
        {
            addressableLocations.Clear();
        }
        
        //Get
        public static bool TryGetAddressLocation<T>(string address, out IResourceLocation location)
        {
            return TryGetAddressLocation(address, typeof(T), out location);
        }
        
        public static bool TryGetAddressLocation(string address, Type typeKey, out IResourceLocation location)
        {
            return addressableLocations.TryGetValue(new AddressKey(address, typeKey), out location);
        }
        
        //Add
        public static bool TryAddAddressLocation<T>(string address, IResourceLocation location)
        {
            return TryAddAddressLocation(address, typeof(T), location);
        }
        
        public static bool TryAddAddressLocation(string address, Type typeKey, IResourceLocation location)
        {
            return addressableLocations.TryAdd(new AddressKey(address, typeKey), location);
        }

        public static bool TryRemoveAddressLocation<T>(string address)
        {
            return TryRemoveAddressLocation(address, typeof(T));
        }
        
        public static bool TryRemoveAddressLocation(string address, Type typeKey)
        {
            return addressableLocations.Remove(new AddressKey(address, typeKey));
        }
        
        #region Structs
        public struct AddressKey : IEquatable<AddressKey>
        {
            private readonly string address;
            private readonly Type type;

            public string Address => address;

            public Type Type => type;

            public AddressKey(string address, Type type)
            {
                this.address = address;
                this.type = type;
            }
            
            public bool Equals(AddressKey other)
            {
                return address == other.address
                       && type.Equals(other.type);
            }
            public override bool Equals(object obj)
            {
                return obj is AddressKey other && Equals(other);
            }
            public override int GetHashCode()
            {
                unchecked
                {
                    return (address.GetHashCode() * 397) ^ type.GetHashCode();
                }
            }
            public static bool operator ==(AddressKey left, AddressKey right)
            {
                return left.Equals(right);
            }
            public static bool operator !=(AddressKey left, AddressKey right)
            {
                return !left.Equals(right);
            }
        }
        #endregion
    }
}
