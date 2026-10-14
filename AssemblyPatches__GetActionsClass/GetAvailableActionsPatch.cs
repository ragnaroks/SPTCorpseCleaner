using EFT;
using EFT.Interactive;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace SPTCorpseCleaner.AssemblyPatches__GetActionsClass {
    /// <summary>allow search zombies</summary>
    public class GetAvailableActionsPatch : ModulePatch {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InteractionContextHelper), nameof(InteractionContextHelper.GetAvailableActions), new System.Type[] { typeof(GamePlayerOwner), typeof(LootItem) });
        }

        [PatchPostfix]
        public static void Postfix(GamePlayerOwner owner, LootItem lootItem, ref AvailableInteractionState __result)
        {
            if (__result != null) { return; }
            __result = InteractionContextHelper.GetAvailableInteractionState(owner, lootItem.ItemOwner.RootItem, lootItem.ItemOwner, lootItem.ItemId, lootItem.Name, lootItem.LastOwner);
        }
    }
}
