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
        
        
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void BindColorChangeKeyPatch(PlayerControllerB __instance)
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

                if (!sprayPaintItem.NetworkObject) return;
                NetworkObjectReference sprayPaintItemNOR = sprayPaintItem.NetworkObject;
                
                if ((Keyboard.current.tKey).wasPressedThisFrame) {
                    Debug.Log("T Key was Pressed");
                    if (__instance.IsHost || __instance.IsServer) {
                        Debug.Log("Does it get here x1");
                        PaintColorSwitchNetworkHandler.Instance.EventClientRpc(sprayPaintItemNOR);
                    } else {
                        Debug.Log("Does it get here x2");
                        PaintColorSwitchNetworkHandler.Instance.EventServerRpc(sprayPaintItemNOR);
                    }

                }
                
            }
        
        }
        
    }
    
}
