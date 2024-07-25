namespace ThunderRoad
{
    public class AirHelper : ThunderBehaviour
    {
        public float minHeight = 1;

        public delegate void AirEvent(Creature creature);

        public event AirEvent OnAirEvent;
        public event AirEvent OnGroundEvent;
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;
        public Locomotion locomotion;
        public bool inAir;

    }
}