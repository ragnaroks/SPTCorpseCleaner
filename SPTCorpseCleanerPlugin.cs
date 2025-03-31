﻿using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using System;

namespace SPTCorpseCleaner {
    /// <summary>corpse cleaner Mod for SPTarkov</summary>
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "1.0.3")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {
        private AssemblyPatches__GetActionsClass.Smethod8Patch? Smethod8Patche{get;set;} = null;

        private AssemblyPatches__GetActionsClass.Smethod9Patch? Smethod9Patche{get;set;} = null;

        public ConfigEntry<Boolean>? Debug{get;private set;} = null;

        protected void Awake () {
            this.Debug = this.Config.Bind<Boolean>("config","debug",false,"don't touch this");
            //this.Smethod8Patche = new AssemblyPatches__GetActionsClass.Smethod8Patch();
            this.Smethod9Patche = new AssemblyPatches__GetActionsClass.Smethod9Patch();
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.Smethod9Patche?.Enable();
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            if (this.Debug?.Value == true) {
                this.Logger.LogDebug(String.Concat("target: ",Singleton<GameWorld>.Instance?.MainPlayer?.InteractableObject?.name));
            }
        }

        protected void OnDestroy() {
            this.Smethod9Patche?.Disable();
            this.Logger.LogDebug("plugin deactived");
        }
    }
}
