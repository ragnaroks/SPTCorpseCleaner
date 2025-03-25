using BepInEx;

namespace SPTCorpseCleaner {
    /// <summary>corpse cleaner Mod for SPTarkov</summary>
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "1.0.2")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {        
        private AssemblyPatches.GetActionsClassPatches.Smethod8Patch? Smethod8PatcheInstance{get;set;} = null;

        protected void Awake () {
            this.Smethod8PatcheInstance = new AssemblyPatches.GetActionsClassPatches.Smethod8Patch();
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.Smethod8PatcheInstance?.Enable();
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            //
        }

        protected void OnDestroy() {
            this.Smethod8PatcheInstance?.Disable();
            this.Logger.LogDebug("plugin deactived");
        }
    }
}
