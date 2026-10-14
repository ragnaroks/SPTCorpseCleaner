using BepInEx;
using BepInEx.Configuration;
using System;

namespace SPTCorpseCleaner {
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "1.0.6")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {
        private AssemblyPatches__GetActionsClass.GetAvailableActionsPatch? GetAvailableActionsPatche { get;set;} = null;

        private AssemblyPatches__GetActionsClass.GetAvailableInteractionStatePatch? GetAvailableInteractionStatePatche { get;set;} = null;

        public static ConfigEntry<Boolean>? Debug{get;private set;} = null;

        protected void Awake () {
            SPTCorpseCleanerPlugin.Debug = this.Config.Bind<Boolean>("config","debug",false,"don't touch this");
            this.GetAvailableActionsPatche = new AssemblyPatches__GetActionsClass.GetAvailableActionsPatch();
            this.GetAvailableInteractionStatePatche = new AssemblyPatches__GetActionsClass.GetAvailableInteractionStatePatch();
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.GetAvailableActionsPatche?.Enable();
            this.GetAvailableInteractionStatePatche?.Enable();
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            //
        }

        protected void OnDestroy() {
            this.GetAvailableActionsPatche?.Disable();
            this.GetAvailableInteractionStatePatche?.Disable();
            this.Logger.LogDebug("plugin deactived");
        }
    }
}
