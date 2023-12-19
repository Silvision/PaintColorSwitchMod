using HarmonyLib;
using Unity.Netcode;
using UnityEngine;


namespace PaintColorSwitchMod.Patches {
    
    [HarmonyPatch]
    public class NetworkObjectManagerPatch {
        static GameObject networkPrefab;
        
        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), "Start")]
        public static void Init()
        {
            if (networkPrefab != null)
                return;
            
            networkPrefab = PaintColorSwitchModBase.MainAssetBundle.LoadAsset<GameObject>("PaintColorSwitchNetworkHandler");
            networkPrefab.AddComponent<PaintColorSwitchNetworkHandler>();
            // mainAssetBundle.Unload(false);

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
        }
    
        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), "Awake")]
        static void SpawnNetworkHandler() {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer) {
                var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
                networkHandlerHost.GetComponent<NetworkObject>().Spawn();   
            }
        }
    }
}