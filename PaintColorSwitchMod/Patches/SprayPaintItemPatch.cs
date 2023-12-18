using System;
using System.Reflection;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace PaintColorSwitchMod.Patches {
    
    [HarmonyPatch(typeof(SprayPaintItem))]
    public class SprayPaintItemPatch {
        // static FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
        //
        // [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.GenerateNewLevelClientRpc))]
        // [HarmonyPostfix]
        // private static void SubscribeToHandler() {
        //     PaintColorSwitchNetworkHandler.ColorChangeEvent += ReceivedEventFromServer;
        // }
        //
        // [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        // [HarmonyPostfix]
        // static void UnsubscribeFromHandler() {
        //     PaintColorSwitchNetworkHandler.ColorChangeEvent -= ReceivedEventFromServer;
        // }
        //
        // public static void ReceivedEventFromServer() {
        //     Debug.Log("ReceivedEventFromServer");
        // }

        
        
    }
}

