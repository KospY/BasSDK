using System;
using System.Collections.Generic;

namespace ThunderRoad
{
    /// <summary>
    /// A collection of parameters to be used by specific gamemodes levelInstances to hold data
    /// </summary>
    public abstract class LevelInstanceData
    {
        public Guid guid;

        public virtual LevelData.Mode BuildMode()
        {
            return null;
        }

        public virtual Dictionary<string, string> BuildOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            //Ensure we add the GUID for the instance to the options
            options.Add(LevelOption.InstanceGuid.ToString(), guid.ToString());
            return options;
        }

        public virtual List<UIAttributeData> BuildAttributes()
        {
            return new List<UIAttributeData>(0);
        }

    }
}
