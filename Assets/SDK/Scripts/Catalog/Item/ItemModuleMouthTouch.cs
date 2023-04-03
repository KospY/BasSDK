namespace ThunderRoad
{
    [System.Serializable]
    public abstract class ItemModuleMouthTouch : ItemModule
    {
        public virtual void OnMouthTouch(Item item, CreatureMouthRelay mouthRelay)
        {
        }
    }
}