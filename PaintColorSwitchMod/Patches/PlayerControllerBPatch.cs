using System;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PaintColorSwitchMod.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PlayerControllerBPatch
    {
        
        private static GrabbableObject currentItem;
        private static SprayPaintItem sprayPaintItem = null;
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePaintColorPatch(PlayerControllerB __instance)
        {
            if ((!(__instance).IsOwner || !__instance.isPlayerControlled || ((__instance).IsServer && !__instance.isHostPlayerObject)) && !__instance.isTestingPlayer)
            {
                currentItem = __instance.ItemSlots[__instance.currentItemSlot];
                sprayPaintItem = currentItem as SprayPaintItem;
                if (sprayPaintItem != null)
                {
                    Debug.Log(sprayPaintItem.GetType());
                }
                
            }
        }
    }
}
