using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ThunderRoad
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonMergeKeyAttribute : System.Attribute
    {
    }

    public class KeyedListMergeConverter : JsonConverter
    {
        readonly IContractResolver contractResolver;

        public KeyedListMergeConverter(IContractResolver contractResolver)
        {
            if (contractResolver == null)
                throw new ArgumentNullException("contractResolver");
            this.contractResolver = contractResolver;
        }

        static bool CanConvert(IContractResolver contractResolver, Type objectType, out Type elementType, out JsonProperty keyProperty)
        {
            elementType = objectType.GetListType();
            if (elementType == null)
            {
                keyProperty = null;
                return false;
            }
            var contract = contractResolver.ResolveContract(elementType) as JsonObjectContract;
            if (contract == null)
            {
                keyProperty = null;
                return false;
            }
            keyProperty = contract.Properties.Where(p => p.AttributeProvider.GetAttributes(typeof(JsonMergeKeyAttribute), true).Count > 0).SingleOrDefault();
            return keyProperty != null;
        }

        public override bool CanConvert(Type objectType)
        {
            Type elementType;
            JsonProperty keyProperty;
            return CanConvert(contractResolver, objectType, out elementType, out keyProperty);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (contractResolver != serializer.ContractResolver)
                throw new InvalidOperationException("Inconsistent contract resolvers");
            Type elementType;
            JsonProperty keyProperty;
            if (!CanConvert(contractResolver, objectType, out elementType, out keyProperty))
                throw new JsonSerializationException(string.Format("Invalid input type {0}", objectType));

            if (reader.TokenType == JsonToken.Null)
                return existingValue;

            var list = existingValue as IList;
            if (list == null || list.Count == 0)
            {
                list = list ?? (IList)contractResolver.ResolveContract(objectType).DefaultCreator();
                serializer.Populate(reader, list);
            }
            else
            {
                var jArray = JArray.Load(reader);
                var comparer = new KeyedListMergeComparer();

                // Custom change to throw error if JsonMergeKey do not exist in the JSON
                if (jArray.FirstOrDefault(i => i[keyProperty.PropertyName] != null) == null)
                {
                    throw new InvalidOperationException("JsonMergeKey [" + keyProperty.PropertyName + "] is missing");
                }

                var lookup = jArray.ToLookup(i => i[keyProperty.PropertyName]?.ToObject(keyProperty.PropertyType, serializer), comparer);
                var done = new HashSet<JToken>();
                foreach (var item in list)
                {
                    var key = keyProperty.ValueProvider.GetValue(item);
                    var replacement = lookup[key].Where(v => !done.Contains(v)).FirstOrDefault();
                    if (replacement != null)
                    {
                        using (var subReader = replacement.CreateReader())
                            serializer.Populate(subReader, item);
                        done.Add(replacement);
                    }
                }
                // Populate the NEW items into the list.
                if (done.Count < jArray.Count)
                    foreach (var item in jArray.Where(i => !done.Contains(i)))
                    {
                        list.Add(item.ToObject(elementType, serializer));
                    }
            }
            return list;
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        class KeyedListMergeComparer : IEqualityComparer<object>
        {
            #region IEqualityComparer<object> Members

            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                if (x is string && y is string && StringComparer.OrdinalIgnoreCase.Equals(x, y)) return true; // Custom change to ignore case in string
                if (object.ReferenceEquals(x, y)) return true;
                else if (x == null || y == null) return false;
                return x.Equals(y);
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                if (obj == null) return 0;
                if (obj is string) return StringComparer.OrdinalIgnoreCase.GetHashCode(obj); // Custom change to ignore case in string
                return obj.GetHashCode();
            }

            #endregion
        }
    }

    public static class TypeExtensions
    {
        public static Type GetListType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    var genType = type.GetGenericTypeDefinition();
                    if (genType == typeof(List<>))
                        return type.GetGenericArguments()[0];
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
