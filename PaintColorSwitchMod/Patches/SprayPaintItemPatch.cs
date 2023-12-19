using System;
using System.Reflection;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace PaintColorSwitchMod.Patches {
    
    [HarmonyPatch]
    public class SprayPaintItemPatch {
        
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.GenerateNewLevelClientRpc))]
        [HarmonyPostfix] 
        static void SubscribeToHandler() {
            PaintColorSwitchNetworkHandler.ColorChangeEvent -= ReceivedEventFromServer;
            PaintColorSwitchNetworkHandler.ColorChangeEvent += ReceivedEventFromServer;
        }
        
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPostfix]
        static void UnsubscribeFromHandler() {
            PaintColorSwitchNetworkHandler.ColorChangeEvent -= ReceivedEventFromServer;
        }
        
        [HarmonyPatch(typeof(SprayPaintItem))]
        public static void ReceivedEventFromServer(NetworkObjectReference sprayPaintItemNOR) {
            Debug.Log("ReceivedEventFromServer");
            if (sprayPaintItemNOR.TryGet(out NetworkObject networkObject)) {
                SprayPaintItem sprayPaintItem = networkObject.GetComponent<SprayPaintItem>();
            
                FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
                int colorIndex = (int)fieldInfo.GetValue(sprayPaintItem);
                
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

