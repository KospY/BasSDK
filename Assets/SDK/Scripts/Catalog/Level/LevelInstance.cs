using System;

namespace ThunderRoad
{
    /// <summary>
    /// A LevelInstance is an instance of a LevelData
    /// </summary>
    [Serializable]
    public class LevelInstance
    {
        public readonly Guid guid;
        //The level Data this is an instance of
        public readonly LevelData levelData;
        //Level mode, options and attributes for the instance
        public readonly LevelInstanceData instanceData;
        
        //Fully configurable flow
        //Only the levelData is specified, so its up to the user  to select options/mode on the mapboard
        public LevelInstance(LevelData levelData)
        {
            this.levelData = levelData;
            this.guid = Guid.NewGuid();
        }
        
        //Preselected flow, has the levelMode and instanceData set which will provide the preselected options
        public LevelInstance(LevelData levelData, LevelInstanceData instanceData)
        {
            this.levelData = levelData;
            this.instanceData = instanceData;
            this.guid = Guid.NewGuid();
            //assign the guid to the instanceData
            this.instanceData.guid = this.guid;
        }

        /// <summary>
        /// The Level should be configurable on the MapBoard if there is no predefined data in the LevelInstance
        /// </summary>
        /// <returns></returns>
        public bool IsUserConfigurable() => instanceData == null;
        
    }
}
