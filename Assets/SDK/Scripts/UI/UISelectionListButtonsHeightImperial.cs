namespace ThunderRoad
{
    public class UISelectionListButtonsHeightImperial : UISelectionListButtons
    {
        public UISelectionListButtonsHeightImperial otherHeightSection = null;

        public enum ImperialType
        {
            FEET,
            INCH,
        }

        public ImperialType imperialType;

    }
}