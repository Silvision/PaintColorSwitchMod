using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace PaintColorSwitchMod.Patches {
    
    [HarmonyPatch(typeof(SprayPaintItem))]
    public class SprayPaintItemPatch {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePaintColorPatch(SprayPaintItem __instance) {
            
        }

    }
}