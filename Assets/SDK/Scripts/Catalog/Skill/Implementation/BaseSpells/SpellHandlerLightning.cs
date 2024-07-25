namespace ThunderRoad.Skill.Spell
{
    public class SpellHandlerLightning : SpellHandlerData
    {
        public virtual bool OnSprayZap()
        {
            return true;
        }

        public virtual bool OnSprayChainZap()
        {
            return true;
        }

        public virtual bool OnArcwireHit()
        {
            return true;
        }

        public virtual bool OnStaffSlamZap()
        {
            return true;
        }
    }
}
