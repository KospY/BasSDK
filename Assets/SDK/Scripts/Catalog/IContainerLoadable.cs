namespace ThunderRoad
{
    public interface IContainerLoadable<T> where T : CatalogData
    {
        public abstract void OnLoadedFromContainer(Container container);

        public abstract ContainerContent InstanceContent();
    }
}
