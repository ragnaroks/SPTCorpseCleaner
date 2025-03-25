using EFT;
using EFT.Interactive;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace SPTCorpseCleaner.AssemblyPatches.GetActionsClassPatches {
    /// <summary>allow search zombies</summary>
/*    
 *    [HarmonyPatch(typeof(GetActionsClass), nameof(GetActionsClass.smethod_7), MethodType.Normal)]
 *    
 *    use SPT wrapper not harmony directly
 */
    public class Smethod7Patch : ModulePatch {
        protected override MethodBase GetTargetMethod () {
            return AccessTools.Method(typeof(GetActionsClass),nameof(GetActionsClass.smethod_7));
        }

        [PatchPostfix]
        public static void Postfix (GamePlayerOwner owner, LootItem lootItem, ref ActionsReturnClass __result) {
            if(__result!=null){return;}
            __result = GetActionsClass.smethod_8(owner,lootItem.ItemOwner.RootItem,lootItem.ItemOwner,lootItem.ItemId,lootItem.Name,lootItem.LastOwner);
        }
    }
}
