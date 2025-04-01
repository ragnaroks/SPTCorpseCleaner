using SPT.Reflection.Patching;
using SPTCorpseCleaner.UnityComponents;
using System.Reflection;

namespace SPTCorpseCleaner.AssemblyPatches_EFT__Interactive__Corpse {
    public class AwakePatch : ModulePatch {
        protected override MethodBase GetTargetMethod () {
            return typeof(EFT.Interactive.Corpse).GetMethod(nameof(EFT.Interactive.Corpse.Awake), BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        public static void Postfix (ref EFT.Interactive.Corpse __instance) {
            //_ = __instance.GetOrAddComponent<CorpseNameLabelComponent>();
            _ = __instance.gameObject.AddComponent<CorpseNameLabelComponent>();
        }
    }
}
