namespace ThunderRoad.AI.Action
{
	public class AttackFirearm : AttackRanged
    {
        public enum FirearmOptions
        {
            MainHand,
            OffHand,
            DualWield
        }

        public FirearmOptions firearmToShoot = FirearmOptions.MainHand;

    }
}
