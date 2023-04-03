using ThunderRoad;
using UnityEngine;

namespace TestThunderScript
{
    public class MyThunderScript : ThunderScript
    {
        public static MyThunderScript instance;
        
        [ModOption(name: "MyThunderScript", tooltip: "Enable or disable TRMod", valueSourceName: nameof(ModOptionBool.defaultValues), valueSourceType: typeof(ModOptionBool))]
        public static void Toggle(bool value)
        {
            if(value) instance.Enable();
            else instance.Disable();
        }
        
        public override void ScriptLoaded(ModManager.ModData modData)
        {
            instance = this;
            base.ScriptLoaded(modData);
            Debug.Log($"MyThunderScript loaded");
        }
        public override void ScriptEnable()
        {
            Debug.Log($"MyThunderScript ScriptEnable");
        }
        public override void ScriptDisable()
        {
            Debug.Log($"MyThunderScript ScriptDisable");
        }
    }
    
    public class MyOtherThunderScript : ThunderScript
    {
        public override void ScriptUpdate()
        {
            // if (UpdateManager.frameCount % 100 == 0)
            // {
            //     Debug.Log($"MyFloatField: {MyFloatField}");
            //     Debug.Log($"MyColorProperty: {MyColorProperty}");
            // }
        }
        public static ModOptionFloat[] floatOptions = {
            new ModOptionFloat ("0.0",0.0f),
            new ModOptionFloat ("0.25",0.25f),
            new ModOptionFloat ("0.5", 0.5f),
            new ModOptionFloat ("0.75", 0.75f),
            new ModOptionFloat ("1.0", 1.0f)
        };
        
        [ModOption(name: "MyFloat", tooltip: "Float Field Option", valueSourceName: nameof(floatOptions), defaultValueIndex = 2)]
        private static float MyFloatField;
        
        [ModOption(name: "MyColorProperty", tooltip: "MyColorProperty", 
            valueSourceName: nameof(ModOptionColor.defaultValues), valueSourceType = typeof(ModOptionColor), defaultValueIndex = 3)]
        private static Color MyColorProperty { get; set; }

        //this tries to pass a colour to a bool
        [ModOption(name: "MyBoolWrongField", tooltip: "MyBoolWrongField", 
            valueSourceName: nameof(ModOptionColor.defaultValues), valueSourceType = typeof(ModOptionColor), defaultValueIndex = 3)]
        private static bool MyBoolWrongField;
        
        public override void ScriptLoaded(ModManager.ModData modData)
        {
            base.ScriptLoaded(modData);
            Debug.Log($"MyOtherThunderScript loaded");
        }
        public override void ScriptEnable()
        {
            Debug.Log($"MyOtherThunderScript ScriptEnable");
        }
        public override void ScriptDisable()
        {
            Debug.Log($"MyOtherThunderScript ScriptDisable");
        }
        
        
    }
}
