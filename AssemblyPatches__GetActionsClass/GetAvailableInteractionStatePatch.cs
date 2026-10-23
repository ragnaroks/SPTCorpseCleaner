using Comfort.Common;
using EFT;
using EFT.Communications;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace SPTCorpseCleaner.AssemblyPatches__GetActionsClass {
    public class GetAvailableInteractionStatePatch : ModulePatch {
        protected override MethodBase GetTargetMethod () {
            return AccessTools.Method(typeof(InteractionContextHelper), nameof(InteractionContextHelper.GetAvailableInteractionState), new System.Type[] { typeof(GamePlayerOwner), typeof(Item), typeof(ItemController), typeof(string), typeof(string), typeof(IPlayer) });
        }

        [PatchPostfix]
        public static void Postfix (GamePlayerOwner owner, Item rootItem, ItemController lootItemOwner, String lootItemId, String lootItemName, IPlayer lootItemLastOwner, ref AvailableInteractionState __result) {
            if (lootItemName.ToLowerInvariant() != "corpse") {return;}
            if (rootItem.TemplateId != "55d7217a4bdc2d86028b456d") {return;}// this id means "默认物品栏"
            if (__result.Actions.FindIndex(x => x.Name.ToLowerInvariant() == "search") < 0) {return;}
            __result.Actions.Add(new InteractionAction() {
                Name = "arena/contextInteractions/card/delete".Localized(),
                TargetName = lootItemName,
                Action = GetAvailableInteractionStatePatch.DeleteCorpse
            });
        }

        private static void DeleteCorpse () {
            GameWorld? gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null) { return; }
            InteractableObject? interactableObject = gameWorld.MainPlayer?.InteractableObject;
            if (interactableObject == null || !interactableObject.isActiveAndEnabled) {
                NotificationManager.DisplayMessageNotification("no target or target invalid");
                return;
            }
            Corpse? corpse = interactableObject.GetComponent<Corpse>();
            if (corpse == null) {
                NotificationManager.DisplayMessageNotification("target is not a corpse");
                return;
            }
            corpse.Kill();
            corpse.gameObject.SetActive(false);
            corpse.gameObject.DestroyAllChildren();
            gameWorld.DestroyLoot(corpse);// Corpse is essentially the IKillableLootItem, pop it for Radar and DynamicMaps
            NotificationManager.DisplayMessageNotification(String.Concat("corpse <", interactableObject.name, "> has been deleted"));
        }
    }
}
