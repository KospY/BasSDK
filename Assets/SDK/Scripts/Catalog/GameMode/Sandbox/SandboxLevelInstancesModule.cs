using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ThunderRoad.Modules
{
    public class SandboxLevelInstancesModule : LevelInstancesModule
    {
        //This just uses the base implementation of LevelInstancesModule

        public override List<LevelInstance> BuildLevelInstances()
        {
            base.BuildLevelInstances();
            return levelInstances;
        }
        
#if UNITY_EDITOR
        public class DungeonData : LevelInstanceData
        {
            
            public LevelData.Mode dungeonLevelMode;
            
            public DungeonData(LevelData.Mode dungeonLevelMode)
            {
                this.dungeonLevelMode = dungeonLevelMode;
            }
            
            public override LevelData.Mode BuildMode() =>  dungeonLevelMode;

            public override Dictionary<string, string> BuildOptions()
            {
                var options = base.BuildOptions();
                
                options.Add(LevelOption.Difficulty.ToString(), "3");
                options.Add(LevelOption.DungeonLength.ToString(), "3");

                return options;
            }
            
            public override List<UIAttributeData> BuildAttributes()
            {
                List<UIAttributeData> attributeDatas = new List<UIAttributeData>();
                
                attributeDatas.Add(new UIAttributeData {
                    name = "Reward",
                    value = "Dalgarian Map Piece",
                    iconAddressId = "BAS.Icon.MapPiece"
                });
                
                attributeDatas.Add(new UIAttributeData {
                    name = "Difficulty",
                    value = "Hard",
                    iconAddressId = "BAS.Icon.DifficultyHard"
                });
                
                attributeDatas.Add(new UIAttributeData {
                    name = "Expires",
                    value = "3 Days",
                    iconAddressId = "BAS.Icon.Timer"
                });
                
                return attributeDatas;
            }
        }
#endif

    }
}
