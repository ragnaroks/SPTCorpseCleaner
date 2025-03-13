using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT;
using EFT.Interactive;
using System;
using System.Numerics;
using UnityEngine;
using static CommonAssets.Scripts.Game.EndByExitTrigerScenario;

namespace SPTCorpseCleaner {
    /// <summary>corpse cleaner Mod for SPTarkov</summary>
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "3.10.5.20250311")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {        
        private Boolean IsBusy{get;set;} = false;

        public ConfigEntry<KeyboardShortcut>? CleanShortcutKey{get;private set;} = null;
        public ConfigEntry<KeyboardShortcut>? ExfilShortcutKey{get;private set;} =null;

        protected void Awake () {
            this.CleanShortcutKey = this.Config.Bind<KeyboardShortcut>("config","clean-shortcut",new KeyboardShortcut(KeyCode.Slash,KeyCode.LeftControl),"shortcut for delete target corpse, default is [CTRL + /]");
            this.ExfilShortcutKey = this.Config.Bind<KeyboardShortcut>("config","exfil-shortcut",new KeyboardShortcut(KeyCode.Backslash,KeyCode.LeftControl),"shortcut for exfil now, default is [CTRL + \\]");
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            if(this.IsBusy){return;}
            if(this.CleanShortcutKey!=null && this.CleanShortcutKey.Value.IsUp()) {
                ThreadingHelper.Instance.StartSyncInvoke(this.DeleteCorpse);
                return;
            }
            if(this.ExfilShortcutKey!=null && this.ExfilShortcutKey.Value.IsUp()) {
                ThreadingHelper.Instance.StartSyncInvoke(this.ExfilNow);
                return;
            }
            _ = false;            
        }

        protected void OnDestroy() {
            this.Logger.LogDebug("plugin deactived");
        }

        private void DeleteCorpse () {
            this.IsBusy = true;
            GameWorld? gameWorld = Singleton<GameWorld>.Instance;
            if(gameWorld==null){return;}
            InteractableObject? interactableObject = gameWorld.MainPlayer?.InteractableObject;
            if(interactableObject==null){
                NotificationManagerClass.DisplayMessageNotification("no target");
                this.IsBusy = false;
                this.Logger.LogWarning("no target");
                return;
            }
            if(!interactableObject.isActiveAndEnabled){
                NotificationManagerClass.DisplayMessageNotification("target invalid");
                this.IsBusy = false;
                this.Logger.LogWarning("target invalid");
                return;
            }
            Corpse? corpse = interactableObject.GetComponent<Corpse>();
            if(corpse==null){
                NotificationManagerClass.DisplayMessageNotification("target is not a corpse");
                this.IsBusy = false;
                this.Logger.LogWarning("target is not a corpse");
                return;
            }
            this.IsBusy = false;
            //gw.MainPlayer.CurrentManagedState.OnInventory(false);
            //gw.MainPlayer.CurrentManagedState.Cancel();
            corpse.gameObject.SetActive(false);
            corpse.gameObject.DestroyAllChildren();
            gameWorld.DestroyLoot(corpse);// Corpse is essentially the IKillableLootItem, pop it for Radar and DynamicMaps
            String str4 = String.Concat("corpse \"",interactableObject.name,"\" has been deleted");
            NotificationManagerClass.DisplayMessageNotification(str4);
            this.Logger.LogInfo(str4);
        }

        private void ExfilNow () {
            this.IsBusy = true;
            GameWorld? gameWorld = Singleton<GameWorld>.Instance;
            if(gameWorld==null){
                this.IsBusy = false;
                return;
            }
            // copy from "BufferInnerZone.cs"
            if(!(Singleton<AbstractGame>.Instance is GInterface122 ginterface)){
                this.IsBusy = false;
                return;
            }
            this.IsBusy = false;
            ginterface.StopSession(gameWorld.MainPlayer.ProfileId, ExitStatus.Survived,String.Empty);
            NotificationManagerClass.DisplayMessageNotification("exfil at now");
            this.Logger.LogInfo("exfil at now");
        }
    }
}
