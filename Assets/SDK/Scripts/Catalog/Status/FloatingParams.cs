namespace ThunderRoad
{
    public struct FloatingParams
    {
        public float gravity;
        public float drag;
        public float mass;
        public bool noSlamAtEnd;

        public FloatingParams(float gravity = 1, float drag = 1, float mass = 1, bool noSlamAtEnd = false)
        {
            this.gravity = gravity;
            this.drag = drag;
            this.mass = mass;
            this.noSlamAtEnd = noSlamAtEnd;
        }

        public static FloatingParams Identity => new(1, 1, 1);
        public static FloatingParams operator *(FloatingParams a, FloatingParams b)
            => new(a.gravity * b.gravity, a.drag * b.drag, a.mass * b.mass);
    }
}
