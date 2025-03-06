using BepInEx;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;


namespace SPTCorpseCleaner {
    [BepInPlugin("net.skydust.SPTCorpseCleanerPlugin", "SPTCorpseCleanerPlugin", "3.10.5.20250303")]
    public class SPTCorpseCleanerPlugin : BaseUnityPlugin {
        protected void Awake () {
            this.Logger.LogDebug("plugin loaded");
        }

        protected void Start () {
            this.Logger.LogDebug("plugin actived");
        }

        protected void Update () {            
            if(!Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyUp(KeyCode.P)){return;}
            this.DeleteCorpse();
        }

        protected void OnDestroy() {
            this.Logger.LogDebug("plugin deactived");
        }

        private void DeleteCorpse () {
            ClientLocalGameWorld? gw = Singleton<GameWorld>.Instance as ClientLocalGameWorld;
            if(gw==null || gw.MainPlayer==null || gw.MainPlayer.InteractableObject==null || !gw.MainPlayer.InteractableObject.isActiveAndEnabled){
                this.Logger.LogWarning("target invalid");
                return;
            }
            Corpse? corpse = gw.MainPlayer.InteractableObject.GetComponent<Corpse>();
            if(corpse==null){
                this.Logger.LogWarning("not a corpse");
                return;
            }
            //gw.MainPlayer.CurrentManagedState.OnInventory(false);
            //gw.MainPlayer.CurrentManagedState.Cancel();
            corpse.gameObject.SetActive(false);
            corpse.gameObject.DestroyAllChildren();
            gw.DestroyLoot(corpse);
        }
    }
}
