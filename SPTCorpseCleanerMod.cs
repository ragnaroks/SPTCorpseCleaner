using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using System;
using UnityEngine;

namespace SPTCorpseCleaner {
    /// <summary>corpse cleaner Mod for SPTarkov</summary>
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "3.10.5.20250303")]
    [BepInProcess("EscapeFromTarkov")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {        
        private Boolean IsBusy{get;set;} = false;

        public ConfigEntry<KeyCode>? ShortcutKey{get;private set;} = null;

        protected void Awake () {
            this.ShortcutKey = this.Config.Bind<KeyCode>("config","shortcut",KeyCode.Backslash,"shortcut, default is the \"\\\", must hold CTRL then press this shortcut key");
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {
            if (!Input.GetKey(KeyCode.LeftControl)){return;}
            KeyCode keyCode = this.ShortcutKey?.Value ?? KeyCode.Backslash;
            if(!Input.GetKeyUp(keyCode)){return;}
            if(this.IsBusy){return;}
            //this.DeleteCorpse();
            ThreadingHelper.Instance.StartSyncInvoke(this.DeleteCorpse);
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
                this.IsBusy = false;
                this.Logger.LogWarning("no target");
                return;
            }
            if(!interactableObject.isActiveAndEnabled){
                this.IsBusy = false;
                this.Logger.LogWarning("target invalid");
                return;
            }
            Corpse? corpse = interactableObject.GetComponent<Corpse>();
            if(corpse==null){
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
            this.Logger.LogInfo(String.Concat("corpse \"",interactableObject.name,"\" has been deleted"));
        }
    }
}
