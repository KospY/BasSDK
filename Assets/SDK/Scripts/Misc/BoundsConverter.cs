using Newtonsoft.Json;
using Newtonsoft.Json.UnityConverters;
using UnityEngine;
namespace ThunderRoad
{
    
    
    /// <summary>
    /// Custom Newtonsoft.Json converter <see cref="JsonConverter"/> for the Unity Bounds type <see cref="Bounds"/>.
    /// </summary>
    public class BoundsConverter : PartialConverter<Bounds>
    {
        protected override void ReadValue(ref Bounds value, string name, JsonReader reader, JsonSerializer serializer)
        {
            switch (name)
            {
                case nameof(value.center):
                    reader.Read();
                    value.center = serializer.Deserialize<Vector3>(reader);
                    break;
                case nameof(value.size):
                    reader.Read();
                    value.size = serializer.Deserialize<Vector3>(reader);
                    break;
                case nameof(value.extents):
                    reader.Read();
                    value.extents = serializer.Deserialize<Vector3>(reader);
                    break;
                case nameof(value.min):
                    reader.Read();
                    value.min = serializer.Deserialize<Vector3>(reader);
                    break;
                case nameof(value.max):
                    reader.Read();
                    value.max = serializer.Deserialize<Vector3>(reader);
                    break;
            }
        }

        protected override void WriteJsonProperties(JsonWriter writer, Bounds value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.center));
            serializer.Serialize(writer, value.center, typeof(Vector3));
            writer.WritePropertyName(nameof(value.size));
            serializer.Serialize(writer, value.size, typeof(Vector3));
        }
    }


}
