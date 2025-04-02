using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using System;

namespace SPTCorpseCleaner {
    /// <summary>corpse cleaner Mod for SPTarkov</summary>
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "1.0.3")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {
        //private AssemblyPatches__GetActionsClass.Smethod8Patch? Smethod8Patche{get;set;} = null;

        private AssemblyPatches__GetActionsClass.Smethod9Patch? Smethod9Patche{get;set;} = null;

        private AssemblyPatches_EFT__Interactive__Corpse.AwakePatch? AwakePatch{get;set;} = null;

        public static ConfigEntry<Boolean>? Debug{get;private set;} = null;

        protected void Awake () {
            SPTCorpseCleanerPlugin.Debug = this.Config.Bind<Boolean>("config","debug",false,"don't touch this");
            //this.Smethod8Patche = new AssemblyPatches__GetActionsClass.Smethod8Patch();
            this.Smethod9Patche = new AssemblyPatches__GetActionsClass.Smethod9Patch();
            this.AwakePatch = new AssemblyPatches_EFT__Interactive__Corpse.AwakePatch();
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            //this.Smethod8Patche?.Enable();
            this.Smethod9Patche?.Enable();
            this.AwakePatch?.Enable();
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            //
        }

        protected void OnDestroy() {
            //this.Smethod8Patche?.Disable();
            this.Smethod9Patche?.Disable();
            this.AwakePatch?.Disable();
            this.Logger.LogDebug("plugin deactived");
        }
    }
}
