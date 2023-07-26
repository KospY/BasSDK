namespace ThunderRoad
{
    public class UIAttributeData
    {
        public string iconAddressId;
        public string name;
        public string value;

        public override string ToString()
        {
            return $"{nameof(iconAddressId)}: {iconAddressId}, {nameof(name)}: {name}, {nameof(value)}: {value}";
        }
    }
}
