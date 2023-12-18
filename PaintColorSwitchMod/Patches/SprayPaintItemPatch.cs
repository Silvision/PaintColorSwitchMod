using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace PaintColorSwitchMod.Patches {
    
    [HarmonyPatch(typeof(SprayPaintItem))]
    public class SprayPaintItemPatch {
        // private static int testingIndex;
        //
        // [HarmonyPatch("SprayPaintServerRpc")]
        // [HarmonyPostfix]
        // public static void SprayPaintServerRpcPatch(SprayPaintItem __instance) {
        //     Debug.Log("Called from ServerRpcPatch...");
        //     
        // }
        //
        // [HarmonyPatch("SprayPaintClientRpc")]
        // [HarmonyPostfix]
        // public static void SprayPaintClientRpcPatch(SprayPaintItem __instance, ref int ___sprayCanMatsIndex) {
        //     Debug.Log("Called from ClientRpcPatch...");
        //     Debug.Log("Testing Index: " + testingIndex);
        //
        //     FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
        //     // int colorIndex = (int)fieldInfo.GetValue(__instance);
        //     //
        //     // if ( colorIndex < __instance.sprayCanMats.Length - 1) {
        //     //     colorIndex++;
        //     // } else {
        //     //     colorIndex = 0;
        //     // }
        //     //
        //     // Debug.Log("ColorIndex: " + colorIndex);
        //     fieldInfo.SetValue(__instance, testingIndex);
        //         
        // }
        //
        // public static void SetIndexColor(int index) {
        //     testingIndex = index;
        // }
    }
}

