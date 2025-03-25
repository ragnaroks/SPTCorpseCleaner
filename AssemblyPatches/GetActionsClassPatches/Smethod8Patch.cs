using Comfort.Common;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace SPTCorpseCleaner.AssemblyPatches.GetActionsClassPatches {
/*    
 *    [HarmonyPatch(typeof(GetActionsClass), nameof(GetActionsClass.smethod_8), MethodType.Normal)]
 *    
 *    use SPT wrapper not harmony directly
 */
    public class Smethod8Patch : ModulePatch {
        protected override MethodBase GetTargetMethod () {
            return AccessTools.Method(typeof(GetActionsClass),nameof(GetActionsClass.smethod_8));
        }

        [PatchPostfix]
        public static void Postfix (GamePlayerOwner owner, Item rootItem, TraderControllerClass lootItemOwner, String lootItemId, String lootItemName, IPlayer lootItemLastOwner, ref ActionsReturnClass __result) {
            //String? playerId = Singleton<GameWorld>.Instance?.MainPlayer?.AccountId;
            //Console.WriteLine("^ playerId: {0}",playerId);
            //if (owner.Player.AccountId != playerId) { return; }
            Console.WriteLine("^^ lootItemName: {0}",lootItemName.ToLowerInvariant());
            if(lootItemName.ToLowerInvariant()!="corpse"){return;}
            Console.WriteLine("^^^^ rootItem.Name: {0}, rootItem.TemplateId: {1}",rootItem.Name,rootItem.TemplateId);   
            if(rootItem.TemplateId!="55d7217a4bdc2d86028b456d"){return;}// this id means "默认物品栏"
            if(__result.Actions.FindIndex(x=>x.Name.ToLowerInvariant()=="search")<0){return;}
            __result.Actions.Add(new ActionsTypesClass(){
                Name="arena/contextInteractions/card/delete".Localized(),
                TargetName=lootItemName,
                Action=Smethod8Patch.DeleteCorpse
            });
        }

        private static void DeleteCorpse () {
            GameWorld? gameWorld = Singleton<GameWorld>.Instance;
            if(gameWorld==null){return;}
            InteractableObject? interactableObject = gameWorld.MainPlayer?.InteractableObject;
            if(interactableObject==null || !interactableObject.isActiveAndEnabled){
                NotificationManagerClass.DisplayMessageNotification("no target or target invalid");
                return;
            }
            Corpse? corpse = interactableObject.GetComponent<Corpse>();
            if(corpse==null){
                NotificationManagerClass.DisplayMessageNotification("target is not a corpse");
                return;
            }
            corpse.gameObject.SetActive(false);
            corpse.gameObject.DestroyAllChildren();
            gameWorld.DestroyLoot(corpse);// Corpse is essentially the IKillableLootItem, pop it for Radar and DynamicMaps
            NotificationManagerClass.DisplayMessageNotification(String.Concat("corpse \"",interactableObject.name,"\" has been deleted"));
        }
    }
}
