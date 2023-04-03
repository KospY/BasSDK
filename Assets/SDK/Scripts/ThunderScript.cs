using System;
using System.Collections;

namespace ThunderRoad
{

    public abstract class ThunderScript
    {
        public Type ThunderScriptType
        {
            get
            {
                if (_type is null)
                {
                    _type = this.GetType();
                }
                return _type;
            }
        }
        
        //The actual type of this object
        private Type _type;
        protected ModManager.ModData ModData { get; private set; }
        
        public bool IsEnabled => _enabled;
        private bool _enabled;
        
        /// <summary>
        /// Can only be instantiated by the game
        /// </summary>
        protected ThunderScript()
        { }
        
        
        /// <summary>
        /// Executed once when the instance is created by the mod manager
        /// </summary>
        /// <param name="modData"></param>
        public virtual void ScriptLoaded(ModManager.ModData modData)
        {
            ModData = modData;
        }

        /// <summary>
        /// Registers the mod with the manager
        /// </summary>
        public void Enable()
        {
            if(IsEnabled) return;
            ScriptEnable();
            _enabled = true;
        }

        /// <summary>
        /// Unregisters the mod with the manager
        /// </summary>
        public void Disable()
        {
            if(!IsEnabled) return;
            ScriptDisable();
            
            _enabled = false;
        }

        /// <summary>
        /// Executed when the mod is activated
        /// </summary>
        public virtual void ScriptEnable()
        {
            
        }

        /// <summary>
        /// Executed when the mod is temporarily disabled, but not destroyed
        /// </summary>
        public virtual void ScriptDisable()
        {
            
        }

        /// <summary>
        /// Executed each Fixed Update frame
        /// </summary>
        public virtual void ScriptFixedUpdate()
        {
            
        }
        
        /// <summary>
        /// Executed each Update frame
        /// </summary>
        public virtual void ScriptUpdate()
        {
            
        }
        
        /// <summary>
        /// Executed each Late frame
        /// </summary>
        public virtual void ScriptLateUpdate()
        {
            
        }
        
        /// <summary>
        /// Executed when a mod is completely unloaded and destroyed
        /// </summary>
        public virtual void ScriptUnload()
        { }
        
        
    }
}
