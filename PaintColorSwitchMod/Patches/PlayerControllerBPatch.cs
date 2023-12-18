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
        
        // private static FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
        // private static int colorIndex;
        
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
                
                if ((Keyboard.current.tKey).wasPressedThisFrame) {
                    Debug.Log("T Key was Pressed");
                    NetworkObjectReference sprayPaintItemNOR = sprayPaintItem.NetworkObject;
                    PaintColorSwitchNetworkHandler.Instance.EventServerRpc(sprayPaintItemNOR);
                    
                    // colorIndex = (int)fieldInfo.GetValue(sprayPaintItem);
                    //
                    // Debug.Log("Value retrieved in PlayerControllerB: " + colorIndex);
                    // if ( colorIndex < sprayPaintItem.sprayCanMats.Length - 1) {
                    //     colorIndex++;
                    // } else {
                    //     colorIndex = 0;
                    // }
                    // Debug.Log("Color set from PlayerControllerB: " + colorIndex);
                    // fieldInfo.SetValue(sprayPaintItem, colorIndex);
                    // sprayPaintItem.sprayParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
                    // sprayPaintItem.sprayCanNeedsShakingParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
        
                    // SprayPaintItemPatch.SetIndexColor(colorIndex);
                    // sprayPaintItem.SprayPaintServerRpc(GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.position, GameNetworkManager.Instance.localPlayerController.gameplayCamera.transform.forward);
                }
                
            }
        
        }
        
    }
    
}
