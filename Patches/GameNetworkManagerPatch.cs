// using HarmonyLib;
// using Unity.Netcode;
// using UnityEngine;
//
// namespace PaintColorSwitchMod.Patches {
//
//     [HarmonyPatch(typeof(GameNetworkManager))]
//     public class GameNetworkManagerPatch {
//         
//         [HarmonyPatch("Start")]
//         [HarmonyPostfix]
//         static void StartPatch()
//         {
//             GameNetworkManager.Instance.GetComponent<NetworkManager>().AddNetworkPrefab(PaintColorSwitchModBase.PaintColorSwitchNetworkHandlerPrefab); // Register the networker prefab
//         }
//         
//         
//         [HarmonyPatch(typeof(StartOfRound), "Awake")]
//         [HarmonyPostfix]
//         static void SpawnNetworkHandler()
//         {
//             if(NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer) {
//                 GameObject networkHandlerHost = Object.Instantiate(PaintColorSwitchModBase.PaintColorSwitchNetworkHandlerPrefab);
//                 networkHandlerHost.GetComponent<NetworkObject>().Spawn(true);
//             }
//         }
//     }
//     
// }