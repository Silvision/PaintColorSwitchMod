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

        private static FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
        private static int colorIndex;
        
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
                
                if ((Keyboard.current.eKey).wasPressedThisFrame) {
                    if ( colorIndex < sprayPaintItem.sprayCanMats.Length - 1) {
                        colorIndex++;
                    } else {
                        colorIndex = 0;
                    }
                    fieldInfo.SetValue(sprayPaintItem, colorIndex);
                    sprayPaintItem.sprayParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
                    sprayPaintItem.sprayCanNeedsShakingParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
                }
                
            }
        }
    }
}
