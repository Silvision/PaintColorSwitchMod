using System;
using System.Reflection;
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
        private static int colorIndex;

        static FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
        // private static SprayPaintItem sprayPaintItem = null;
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePaintColorPatch(PlayerControllerB __instance)
        {
            // Simple filter, but credit still goes to Ryokune (https://github.com/VisualError/LethalParrying) as I learned from looking at his code. Thanks!
            if ((__instance.IsOwner && __instance.isPlayerControlled && (!__instance.IsServer || __instance.isHostPlayerObject)) || __instance.isTestingPlayer)
            {
                currentItem = __instance.ItemSlots[__instance.currentItemSlot];
                if (currentItem == null) {
                    return;     
                }   
                sprayPaintItem = (currentItem is SprayPaintItem) ? currentItem as SprayPaintItem : null;
                if (sprayPaintItem == null) {
                    return; 
                }

                colorIndex = (int)fieldInfo.GetValue(sprayPaintItem);
                Debug.Log("color index is now: " + colorIndex);
                
                if ((Keyboard.current.eKey).wasPressedThisFrame) {
                    if ( colorIndex < sprayPaintItem.sprayCanMats.Length - 1) {
                        fieldInfo.SetValue(sprayPaintItem, colorIndex+1);
                        Debug.Log("color index is now: " + colorIndex);

                    } else {
                        fieldInfo.SetValue(sprayPaintItem, 0);
                        Debug.Log("color index is now: " + colorIndex);
                    }
                }
                
            }
        }
    }
}
